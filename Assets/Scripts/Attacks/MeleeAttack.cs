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

    [SerializeField] private LayerMask player;

    private bool shouldAttack = true;

    private NavMeshAgent agent;

    private HealthController playerHP;
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
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        shouldAttack = false;

        RaycastHit hit;

        Vector3 sourcePos = transform.position;


        // TODO: SPHERECAST + CHEQUEAR ANGULO instead or Raycast
        if (Physics.Raycast(sourcePos, transform.forward, out hit, attackProximity, player))
        {
            playerHP = hit.transform.GetComponentInParent<HealthController>();
            hitPoint = hit.point;

            StartCoroutine(StopToPunch());
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

    public float AttackNow()
    {
        HandleAttack();
        return occupiedTimeAfterAttack;
    }
}
