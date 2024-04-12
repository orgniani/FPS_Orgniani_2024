using System;
using System.Collections;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthController HP;

    [SerializeField] private bool playerSpotted = false;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float visionRadius = 5f;
    [SerializeField] private float offset = 0.5f;

    [SerializeField] private float proximityRadius = 5f;
    [SerializeField] private float fieldOfViewAngle = 90f;

    [SerializeField] private float stopAndWaitTime = 1f;
    public bool shouldStop = false;

    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    public float offsetTargetDistance = 1.0f;

    [SerializeField] private Transform[] patrolPoints;
    private int currentpatrolPointIndex = 0;

    public event Action onAttack = delegate { };

    private enum MovementState { PATROL = 0, FOLLOW, STOP }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        HP.onDead += HandleDeath;
    }

    private void OnDisable()
    {
        HP.onDead -= HandleDeath;
    }

    private void Update()
    {
        CheckIfPlayerSpotted();

        if (shouldStop) return;

        if (!playerSpotted)
        {
            Patrol();
        }

        else
        {
            onAttack.Invoke();

            // Calculate the offset vector
            Vector3 offsetDirection = (target.position - transform.position).normalized;
            Vector3 offsetPosition = target.position + offsetDirection * offsetTargetDistance;

            // Sample a valid position near the offset position
            NavMeshHit hit;
            if (NavMesh.SamplePosition(offsetPosition, out hit, offsetTargetDistance, NavMesh.AllAreas))
            {
                // Set the destination to the sampled position
                agent.SetDestination(hit.position);
            }
        }


    }

    private void CheckIfPlayerSpotted()
    {
        bool playerIsTooClose = Physics.CheckSphere(transform.position, proximityRadius, playerLayer);

        Vector3 spherePosition = transform.position + transform.forward * offset;
        bool playerIsInVisionRange = Physics.CheckSphere(spherePosition, visionRadius, playerLayer);

        Vector3 directionToPlayer = target.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (playerIsTooClose) playerSpotted = true;

        else
        {
            if (playerIsInVisionRange && angleToPlayer < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if (!playerSpotted)
                {
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRadius*2f, ~playerLayer))
                    {
                        playerSpotted = false;
                    }

                    else
                    {
                        playerSpotted = true;

                    }
                }
            }

            else
            {
                if(playerSpotted)
                {
                    StartCoroutine(StopAndWait());
                    playerSpotted = false;
                }
            }
        }

    }

    private IEnumerator StopAndWait()
    {
        shouldStop = true;

        Vector3 stopPoint = transform.position + transform.forward * 2f;
        agent.SetDestination(stopPoint);

        yield return new WaitForSeconds(stopAndWaitTime);

        shouldStop = false;
    }

    private void Patrol()
    {
        Vector3 nextPoint = patrolPoints[currentpatrolPointIndex].position;

        float targetDistance = Vector2.Distance(transform.position, nextPoint);

        if (targetDistance < 1f)
        {
            currentpatrolPointIndex++;

            if (currentpatrolPointIndex >= patrolPoints.Length)
            {
                currentpatrolPointIndex = 0;
            }
        }

        agent.SetDestination(nextPoint);

    }

    private void OnDrawGizmos()
    {
        // Draw the sphere in the scene view
        Gizmos.color = playerSpotted ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * offset, visionRadius);

        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }

    private void HandleDeath()
    {
        agent.isStopped = true;
        enabled = false;
    }

}
