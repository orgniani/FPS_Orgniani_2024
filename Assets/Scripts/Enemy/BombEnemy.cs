using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BombEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private HealthController HP;

    [SerializeField] private Transform target;
    [SerializeField] private LayerMask playerLayer;

    [Header("Parameters")]
    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionDamage = 10;
    [SerializeField] private float countdownDuration = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explotionSound;
    [SerializeField] private AudioSource countingDownSound;

    private NavMeshAgent agent;
    private bool isCountingDown = false;

    public event Action onCloseToPlayer = delegate { };

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        HP.onDead += HandleExplosion;
    }

    private void OnDisable()
    {
        HP.onDead -= HandleExplosion;
    }

    private void Update()
    {
        agent.SetDestination(target.position);
        CheckIfPlayerClose();
    }

    private void HandleExplosion()
    {
        audioSource.PlayOneShot(explotionSound);

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            HealthController targetHP = hit.GetComponent<HealthController>();
            FlammableObject flammableObject = hit.GetComponent<FlammableObject>();

            if (rb != null)
            {
                rb.isKinematic = false;

                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            if(flammableObject != null)
            {
                flammableObject.HandleGetLitOnFire();
            }

            if (targetHP != null && hit.CompareTag("Player")) targetHP.ReceiveDamage(explosionDamage, hit.transform.position);
        }
    }

    private void CheckIfPlayerClose()
    {
        bool playerIsCloseEnough = Physics.CheckSphere(transform.position, explosionRadius, playerLayer);

        if (playerIsCloseEnough && !isCountingDown)
        {
            countingDownSound.Play();
            onCloseToPlayer?.Invoke();

            StartCoroutine(InitiateCountdown());
        }
    }

    private IEnumerator InitiateCountdown()
    {
        isCountingDown = true;
        agent.isStopped = true;

        float timer = countdownDuration;
        while (timer > 0f)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        countingDownSound.Stop();
        HP.ReceiveDamage(HP.Health, Vector3.zero);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}