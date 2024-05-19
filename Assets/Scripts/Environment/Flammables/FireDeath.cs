using System;
using UnityEngine;

public class FireDeath : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask fireFoamLayer;
    [SerializeField] private ParticleSystem fireInstance;

    private ParticleSystem.MainModule main;

    private AudioSource audioSource;

    public event Action onDeath = delegate { };

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        main = fireInstance.main;
    }

    private void OnParticleCollision(GameObject other)
    {
        if ((fireFoamLayer.value & (1 << other.layer)) != 0)
        {
            HandleFireDeath();
        }
    }

    public void HandleFireDeath()
    {
        main.loop = false;
    }

    private void OnDestroy()
    {
        onDeath?.Invoke();
    }
}
