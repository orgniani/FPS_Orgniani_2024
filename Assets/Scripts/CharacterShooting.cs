using UnityEngine;


public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private LayerMask enemies;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float gunDamage = 10;

    public bool isPointingAtEnemy = false;

    [SerializeField] private ShotFeedback shotFeedback;

    private HealthController targetHP;

    private void Update()
    {
        UpdateGunSightColor();
    }

    private void UpdateGunSightColor()
    {
        RaycastHit hit;

        Vector3 sourcePos = transform.position + offset;

        if (Physics.Raycast(sourcePos, transform.forward, out hit, Mathf.Infinity, enemies))
        {
            // Check if the raycast hits an enemy
            targetHP = hit.transform.GetComponentInParent<HealthController>();
            isPointingAtEnemy = true;
        }
        else
        {
            isPointingAtEnemy = false;
        }
    }

    public void Shoot()
    {
	    if (isPointingAtEnemy)
	    {
			targetHP.ReceiveDamage(gunDamage);
	    }
	}


}
