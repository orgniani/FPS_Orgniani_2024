using UnityEngine;

public class AfterDeathExplosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPrefab;

    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionDamage = 10;

    [SerializeField] private HealthController HP;

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

        // Get all colliders in the explosion radius
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
