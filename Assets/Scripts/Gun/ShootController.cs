using UnityEngine;


public class ShootController : MonoBehaviour
{
    [SerializeField] private LayerMask enemies;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float gunDamage = 10;

    [SerializeField] private ShotFeedback shotPrefab;
    [SerializeField] private Transform gunTip;

    [SerializeField] private ParticleSystem gunSmoke;

    [SerializeField] private float gunRange = 10f;

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

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 sourcePos = transform.position + offset;

        if (Physics.Raycast(sourcePos, cameraForward, out hit, Mathf.Infinity, enemies))
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
        if (Cursor.lockState != CursorLockMode.Locked) return;

        gunSmoke.Play();

        ShotFeedback shotFeedback = Instantiate(shotPrefab, gunTip.position, Quaternion.identity);


        Vector3 endPosition = gunTip.position + gunTip.forward * gunRange;

        if (isPointingAtEnemy)
	    {
			targetHP.ReceiveDamage(gunDamage, hitPoint);
            shotFeedback.ShowShotDirection(hitPoint);
        }

        else
        {
            shotFeedback.ShowShotDirection(endPosition);
        }

    }
}
