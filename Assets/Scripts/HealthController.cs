using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float health =100;

	public void ReceiveDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
			Die();
	}

    private void Die()
    {
        Destroy(gameObject);
    }
}
