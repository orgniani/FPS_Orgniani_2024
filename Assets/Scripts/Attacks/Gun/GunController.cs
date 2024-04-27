using UnityEngine;


public class GunController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float gunDamage = 10f;
    [SerializeField] private float gunRange = 10f;

    public bool isPointingAtEnemy = false;

    [Header("References")]
    [SerializeField] private ShotFeedback shotPrefab;

    [SerializeField] private Transform gunTip;
    [SerializeField] private ParticleSystem gunSmoke;


    [SerializeField] private LayerMask enemies;

    private float targetDistance;

    private HealthController targetHP;
    private Vector3 hitPoint;

    private void Update()
    {
        if (!enabled) return;
        UpdateGunSightColor();
    }

    private void UpdateGunSightColor()
    {
        RaycastHit hit;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 sourcePos = transform.position + offset;

        if (Physics.Raycast(sourcePos, cameraForward, out hit, Mathf.Infinity, enemies))
        {
            targetHP = hit.transform.GetComponentInParent<HealthController>();
            hitPoint = hit.point;

            targetDistance = Vector3.Distance(gunTip.position, hitPoint);
            isPointingAtEnemy = targetDistance <= gunRange;
        }
        else
        {
            isPointingAtEnemy = false;
        }
    }

    public void Shoot()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (!enabled) return;

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
