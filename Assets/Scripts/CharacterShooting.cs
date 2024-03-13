using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private LayerMask enemies;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float gunDamage = 10;

    public void Shoot()
    {
	    RaycastHit hit;

	    Vector3 sourcePos = transform.position + offset;

	    if (Physics.Raycast(sourcePos, transform.forward, out hit, Mathf.Infinity, enemies))
	    {
		    HealthController HP = hit.transform.GetComponentInParent<HealthController>();
			HP.ReceiveDamage(gunDamage);

		    Debug.DrawRay(sourcePos, transform.forward * hit.distance, Color.yellow, 2);
	    }
	    else
	    {
		    Debug.DrawRay(sourcePos, transform.forward * 1000, Color.white, 2);
	    }
	}
}
