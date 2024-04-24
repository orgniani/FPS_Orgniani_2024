using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeleeAttack attack;
    [SerializeField] private HealthController HP;

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
    private bool shouldStop = false;

    private NavMeshAgent agent;
    private HealthController playerHP;

    private int currentpatrolPointIndex = 0;

    //public event Action onAttack = delegate { };

    public static event Action<Enemy> onSpawn;
    public static event Action<Enemy> onDeath;

    private enum MovementState { PATROL = 0, FOLLOW, STOP }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerHP = target.gameObject.GetComponent<HealthController>();

        onSpawn?.Invoke(this);
    }

    private void OnEnable()
    {
        HP.onHurt += HandleHurt;
        HP.onDead += HandleDeath;
    }

    private void OnDisable()
    {
        HP.onHurt -= HandleHurt;
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
            if (playerHP.Health <= 0) return;
                //onAttack?.Invoke();

            float waitTime = attack.AttackNow();
            agent.SetDestination(target.position);
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

        if (targetDistance < 3f)
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
        Gizmos.color = playerSpotted ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * offset, visionRadius);

        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }

    private void HandleDeath()
    {
        onDeath?.Invoke(this);

        agent.isStopped = true;
        enabled = false;
    }

    private void HandleHurt()
    {
        if (HP.Health <= 0) return;

        StartCoroutine(StopMovingAfterHit());
    }

    private IEnumerator StopMovingAfterHit()
    {
        agent.isStopped = true;
        enabled = false;

        yield return new WaitForSeconds(0.5f);

        agent.isStopped = false;
        enabled = true;
    }

}
