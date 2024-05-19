using System;
using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttack
{
    [Header("Parameters")]
    [SerializeField] private float damage;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackProximity = 5f;

    [SerializeField] private float fieldOfAttackAngle = 90f;
    [SerializeField] private float offset = 0.5f;

    [SerializeField] private LayerMask playerLayer;

    [Header("Audio")]
    [SerializeField] private AudioClip punchSound;

    private AudioSource audioSource;

    private Transform playerTransform;
    private HealthController playerHP;

    private bool shouldAttack = true;
    private Vector3 hitPoint;
    private float occupiedTimeAfterAttack = 1;

    public event Action onPunch = delegate { };

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (angleToPlayer < fieldOfAttackAngle && playerIsInAttackRange)
            Punch();

        yield return new WaitForSeconds(attackCooldown);

        shouldAttack = true;
    }

    private void Punch()
    {
        RaycastHit hit;
        Vector3 sourcePos = transform.position;

        Physics.Raycast(sourcePos, transform.forward, out hit, attackProximity, playerLayer);
        hitPoint = hit.point;

        onPunch?.Invoke();
        if (punchSound) audioSource.PlayOneShot(punchSound);

        playerHP.ReceiveDamage(damage, hitPoint);
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
