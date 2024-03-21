using UnityEngine;


public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private LayerMask enemies;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float gunDamage = 10;

    [SerializeField] private ShotFeedback shotPrefab;
    [SerializeField] private Transform gunTip;

    [SerializeField] private ParticleSystem gunSmoke;

    public bool isPointingAtEnemy = false;

    private HealthController targetHP;
    private Vector3 hitPoint;

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
            hitPoint = hit.point;
            isPointingAtEnemy = true;
        }
        else
        {
            isPointingAtEnemy = false;
        }
    }

    public void Shoot()
    {
        gunSmoke.Play();

        ShotFeedback shotFeedback = Instantiate(shotPrefab, gunTip.position, gunTip.rotation);
        shotFeedback.ShowShotDirection();

        if (isPointingAtEnemy)
	    {
			targetHP.ReceiveDamage(gunDamage, hitPoint);
	    }

	}
}
