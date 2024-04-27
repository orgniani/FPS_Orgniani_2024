using UnityEngine;

public class AfterDeathExplosion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private HealthController HP;

    [Header("Parameters")]
    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionDamage = 10;

    private void OnEnable()
    {
        HP.onDead += HandleExplosion;
    }

    private void OnDisable()
    {
        HP.onDead -= HandleExplosion;
    }

    private void HandleExplosion()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            HealthController HP = hit.GetComponent<HealthController>();

            if (rb != null)
            {
                rb.isKinematic = false;

                // Apply explosion force to each Rigidbody
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            if (HP != null) HP.ReceiveDamage(explosionDamage, hit.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
