using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeleeAttack attack;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform target;

    [SerializeField] private LayerMask playerLayer;

    [Header("Parameters")]
    [SerializeField] private float visionRadius = 5f;
    [SerializeField] private float offset = 0.5f;

    [SerializeField] private float proximityRadius = 5f;
    [SerializeField] private float fieldOfViewAngle = 90f;

    [SerializeField] private float stopAndWaitTime = 1f;

    private bool playerSpotted = false;

    private NavMeshAgent agent;
    private HealthController playerHP;

    private int currentPatrolPointIndex = 0;

    private enum MovementState { PATROL = 0, FOLLOW, STOP }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerHP = target.gameObject.GetComponent<HealthController>();
    }

    private void Update()
    {
        CheckIfPlayerSpotted();

        if (!playerSpotted)
        {
            Patrol();
        }

        else
        {
            if (playerHP.Health <= 0) return;

            //float waitTime = attack.AttackNow();
            attack.AttackNow(target, playerHP);
            agent.SetDestination(target.position);
        }
    }

    private void CheckIfPlayerSpotted()
    {
        agent.isStopped = false;

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
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRadius * 2f, ~playerLayer))
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
                if (playerSpotted)
                {
                    playerSpotted = false;
                }
            }
        }
    }

    private void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            SetNextPatrolPoint();
    }

    private void SetNextPatrolPoint()
    {
        agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = playerSpotted ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * offset, visionRadius);

        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }
}
