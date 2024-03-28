using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    [SerializeField] private FollowerEnemy enemy;

    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;

    [SerializeField] private LayerMask player;

    private bool shouldAttack = true;

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

        if (Physics.Raycast(sourcePos, transform.forward, out hit, Mathf.Infinity, player))
        {
            HealthController playerHP = hit.transform.GetComponentInParent<HealthController>();
            playerHP.ReceiveDamage(damage, hit.point);
        }


        yield return new WaitForSeconds(attackCooldown);

        shouldAttack = true;
    }
}
