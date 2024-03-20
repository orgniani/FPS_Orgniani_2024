using UnityEngine;

public delegate void VoidDelegate();

public class HealthController : MonoBehaviour
{
    [SerializeField] private float health =100;
    [SerializeField] private float maxHealth = 100;

    [SerializeField] private ParticleSystem bloodSplashPrefab;

    public VoidDelegate onHurt;

	public float getHealth() => health;

    public float getMaxHealth() => maxHealth;


    public void ReceiveDamage(float damage)
	{
		if (onHurt != null) onHurt();

		health -= damage;
        CreateBloodSplash();

		if (health <= 0)
			Die();
	}

    private void CreateBloodSplash()
    {
        Instantiate(bloodSplashPrefab,
        transform.position,
        transform.rotation);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
