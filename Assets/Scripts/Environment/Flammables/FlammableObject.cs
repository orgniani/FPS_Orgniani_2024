using System;
using System.Collections;
using UnityEngine;

public class FlammableObject : MonoBehaviour, IFlammable
{
    [Header("References")]
    [SerializeField] private ParticleSystem fireParticleSystemPrefab;
    [SerializeField] private HealthController HP;
    [SerializeField] private ParticleSystem smokeParticleSystemPrefab;

    [Header("Fire Parameters")]
    [SerializeField] private float fireYOffset;
    [SerializeField] private float fireDamage = 1f;
    [SerializeField] private float recieveDamageCooldown = 1f;
    [SerializeField] private float smokeYOffset = 2f;

    private bool shouldLoseHealth = true;

    private ParticleSystem instantiatedFire;
    private FireDeath fireDeath;

    public static event Action<FlammableObject> onSpawn;
    public static event Action<FlammableObject> onFire;
    public static event Action<FlammableObject> onExtinguished;
    public static event Action<FlammableObject> onDeath;

    public bool IsOnFire { get; private set; }

    private void Start()
    {
        onSpawn?.Invoke(this);
    }

    private void OnEnable()
    {
        IsOnFire = false;

        HP.onDead += HandleDeath;
    }

    private void OnDisable()
    {
        HP.onDead -= HandleDeath;
    }

    private void Update()
    {
        if (!IsOnFire) return;
        if (!shouldLoseHealth) return;
        StartCoroutine(LoseHealthWhileOnFire());
    }

    public void HandleGetLitOnFire()
    {
        Vector3 firePosition = new Vector3(transform.position.x, transform.position.y + fireYOffset, transform.position.z);
        instantiatedFire = Instantiate(fireParticleSystemPrefab, firePosition, Quaternion.identity);

        fireDeath = instantiatedFire.GetComponent<FireDeath>();
        fireDeath.onDeath += StopBeingOnFire;

        IsOnFire = true;

        onFire?.Invoke(this);
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
        fireDeath.onDeath -= StopBeingOnFire;
        IsOnFire = false;

        onExtinguished?.Invoke(this);
    }

    private void HandleDeath()
    {
        fireDeath.HandleFireDeath();

        Vector3 smokePosition = new Vector3(transform.position.x, transform.position.y - smokeYOffset, transform.position.z);
        Instantiate(smokeParticleSystemPrefab, smokePosition, Quaternion.identity);

        onDeath?.Invoke(this);
    }
}
