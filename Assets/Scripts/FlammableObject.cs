using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlammableObject : MonoBehaviour, IFlammable
{
    [Header("References")]
    [SerializeField] private ParticleSystem fireParticleSystemPrefab;
    [SerializeField] private HealthController HP;

    [Header("Fire Parameters")]
    [SerializeField] private float fireYOffset;
    [SerializeField] private float fireDamage = 1f;
    [SerializeField] private float recieveDamageCooldown = 1f;

    private bool isOnFire = false;
    private bool shouldLoseHealth = true;

    private ParticleSystem instantiatedFire;
    private FireDeath fireDeath;


    private void Update()
    {
        if (!isOnFire) return;
        if (!shouldLoseHealth) return;
        StartCoroutine(LoseHealthWhileOnFire());
    }

    public void LitOnFire()
    {
        HandleGetLitOnFire();
    }

    [ContextMenu("FIRE!!")]
    private void HandleGetLitOnFire()
    {
        Vector3 firePosition = new Vector3(transform.position.x, transform.position.y + fireYOffset, transform.position.z);
        instantiatedFire = Instantiate(fireParticleSystemPrefab, firePosition, Quaternion.identity);

        fireDeath = instantiatedFire.GetComponent<FireDeath>();
        fireDeath.onDeath += StopBeingOnFire;

        isOnFire = true;
    }

    private IEnumerator LoseHealthWhileOnFire()
    {
        shouldLoseHealth = false;

        HP.ReceiveDamage(fireDamage, transform.position);

        yield return new WaitForSeconds(recieveDamageCooldown);

        shouldLoseHealth = true;
    }

    private void StopBeingOnFire()
    {
        Debug.Log("STOP");
        fireDeath.onDeath -= StopBeingOnFire;
        isOnFire = false;
    }
}
