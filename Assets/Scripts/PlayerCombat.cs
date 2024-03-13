using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
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
		    Debug.Log("Did Hit");
	    }
	    else
	    {
		    Debug.DrawRay(sourcePos, transform.forward * 1000, Color.white, 2);
		    Debug.Log("Did not Hit");
	    }
	}
}
