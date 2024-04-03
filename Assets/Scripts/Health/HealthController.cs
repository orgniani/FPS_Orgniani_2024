using UnityEngine;

public delegate void VoidDelegate();

public class HealthController : MonoBehaviour
{
    [SerializeField] private float health =100;
    [SerializeField] private float maxHealth = 100;

    [SerializeField] private ParticleSystem bloodSplashPrefab;

    public bool shouldDisappear = true;

    public VoidDelegate onHurt;
    public VoidDelegate onDead;

    public float getHealth() => health;

    public float getMaxHealth() => maxHealth;


    public void ReceiveDamage(float damage, Vector3 hitPoint)
	{
		health -= damage;
        if (onHurt != null) onHurt();

        CreateBloodSplash(hitPoint);

        if (health <= 0)
			Die();
	}

    private void CreateBloodSplash(Vector3 hitPoint)
    {
        if (!bloodSplashPrefab) return;
        Instantiate(bloodSplashPrefab, hitPoint, Quaternion.identity);
    }

    private void Die()
    {
        if(onDead != null) onDead();

        if(shouldDisappear) gameObject.SetActive(false);
    }
}
