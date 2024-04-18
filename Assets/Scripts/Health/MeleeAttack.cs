using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;

    [SerializeField] private float attackProximity = 5f;

    [SerializeField] private LayerMask player;

    private bool shouldAttack = true;

    private NavMeshAgent agent;

    private HealthController playerHP;
    private Vector3 hitPoint;

    public event Action onPunch = delegate { };

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        enemy.onAttack += HandleAttack;
    }

    private void OnDisable()
    {
        enemy.onAttack -= HandleAttack;
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

        yield return new WaitForSeconds(0.5f);

        onPunch.Invoke();
        playerHP.ReceiveDamage(damage, hitPoint);

        yield return new WaitForSeconds(0.5f);

        agent.isStopped = false;
        enabled = true;
    }
}
