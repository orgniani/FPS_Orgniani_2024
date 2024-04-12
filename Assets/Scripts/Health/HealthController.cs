using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float health =100;
    [SerializeField] private float maxHealth = 100;

    [SerializeField] private ParticleSystem bloodSplashPrefab;

    public bool shouldDisappear = true;

    public event Action onHurt = delegate { };
    public event Action onDead = delegate { };

    public float getHealth() => health;

    public float getMaxHealth() => maxHealth;


    public void ReceiveDamage(float damage, Vector3 hitPoint)
	{
		health -= damage;
        onHurt.Invoke();

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
        onDead.Invoke();

        if(shouldDisappear) gameObject.SetActive(false);
    }
}
