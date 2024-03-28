using UnityEngine;

public delegate void VoidDelegate();

public class HealthController : MonoBehaviour
{
    [SerializeField] private float health =100;
    [SerializeField] private float maxHealth = 100;

    [SerializeField] private ParticleSystem bloodSplashPrefab;
    [SerializeField] private ParticleSystem explosionPrefab;

    [SerializeField] private bool shouldExplodeAfterDeath = false;
    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRadius = 5f;

    public bool shouldDisappear = true;

    public VoidDelegate onHurt;
    public VoidDelegate onDead;

    public float getHealth() => health;

    public float getMaxHealth() => maxHealth;


    public void ReceiveDamage(float damage, Vector3 hitPoint)
	{
		if (onHurt != null) onHurt();

		health -= damage;
        CreateBloodSplash(hitPoint);

		if (health <= 0)
			Die();
	}

    private void CreateBloodSplash(Vector3 hitPoint)
    {
        Instantiate(bloodSplashPrefab, hitPoint, Quaternion.identity);
    }

    private void Die()
    {
        if(onDead != null) onDead();

        if(shouldExplodeAfterDeath)
        {
            Explode();
        }

        if(shouldDisappear) gameObject.SetActive(false);
    }

    private void Explode()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // Get all colliders in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;

                // Apply explosion force to each Rigidbody
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }
}
