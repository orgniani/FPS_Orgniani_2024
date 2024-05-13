using System;
using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour, IHittable
{
    [Header("References")]
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    [Header("Parameters")]
    [SerializeField] private float health =100;
    [SerializeField] private float maxHealth = 100;

    [SerializeField] private bool shouldDisappearAfterDeath = false;

    public event Action onHurt = delegate { };
    public event Action onDead = delegate { };

    public float Health => health;

    public float MaxHealth => maxHealth;


    public void ReceiveDamage(float damage, Vector3 hitPoint)
	{
		health -= damage;
        onHurt?.Invoke();

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
        onDead?.Invoke();

        if(shouldDisappearAfterDeath) gameObject.SetActive(false);
    }
}
