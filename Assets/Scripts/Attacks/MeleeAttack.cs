using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : MonoBehaviour, IAttack
{
    [Header("Parameters")]
    [SerializeField] private float damage;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackProximity = 5f;

    [SerializeField] private float fieldOfAttackAngle = 90f;
    [SerializeField] private float offset = 0.5f;

    [SerializeField] private float maxStoppingDistance = 2f;
    [SerializeField] private float minStoppingDistance = 1f;

    [SerializeField] private LayerMask playerLayer;

    private Transform playerTransform;
    private HealthController playerHP;

    private bool shouldAttack = true;

    private NavMeshAgent agent;

    private Vector3 hitPoint;

    private float occupiedTimeAfterAttack = 1;

    public event Action onPunch = delegate { };

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void HandleAttack()
    {
        if (!shouldAttack) return;
        if (playerHP.Health <= 0) return;
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        shouldAttack = false;

        Vector3 spherePosition = transform.position + transform.forward * offset;
        bool playerIsInAttackRange = Physics.CheckSphere(spherePosition, attackProximity, playerLayer);

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < fieldOfAttackAngle)
        {
            //Debug.Log("IN ANGLE" + angleToPlayer);
            agent.stoppingDistance = maxStoppingDistance;

            if (playerIsInAttackRange)
            {
                RaycastHit hit;
                Vector3 sourcePos = transform.position;

                Physics.Raycast(sourcePos, transform.forward, out hit, attackProximity, playerLayer);
                hitPoint = hit.point;

                StartCoroutine(StopToPunch());
            }
        }

        else
        {
            //Debug.Log("OUT OF ANGLE" + angleToPlayer);
            agent.stoppingDistance = minStoppingDistance;
        }


        yield return new WaitForSeconds(attackCooldown);

        shouldAttack = true;
    }

    private IEnumerator StopToPunch()
    {
        agent.isStopped = true;
        enabled = false;

        onPunch?.Invoke();
        Debug.Log("Attack!");

        playerHP.ReceiveDamage(damage, hitPoint);

        yield return new WaitForSeconds(0.5f);

        agent.isStopped = false;
        enabled = true;
    }

    public float AttackNow(Transform target, HealthController targetHP)
    {
        playerTransform = target;
        playerHP = targetHP;

        HandleAttack();
        return occupiedTimeAfterAttack;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * offset, attackProximity);
    }
}
