using System;
using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;

    [SerializeField] private float attackProximity = 5f;

    [SerializeField] private LayerMask player;

    private bool shouldAttack = true;

    public event Action onPunch = delegate { };

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
            HealthController playerHP = hit.transform.GetComponentInParent<HealthController>();

            Debug.Log("Attack!");
            onPunch.Invoke();

            playerHP.ReceiveDamage(damage, hit.point);
        }


        yield return new WaitForSeconds(attackCooldown);

        shouldAttack = true;
    }
}
