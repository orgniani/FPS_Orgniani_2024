using System;
using UnityEngine;

public class FireDeath : MonoBehaviour
{
    [SerializeField] private LayerMask fireFoamLayer;

    [SerializeField] private ParticleSystem fireInstance;
    private ParticleSystem.MainModule main;

    public event Action onDeath = delegate { };

    private void OnEnable()
    {
        main = fireInstance.main;
    }

    private void OnParticleCollision(GameObject other)
    {
        if ((fireFoamLayer.value & (1 << other.layer)) != 0)
        {
            //Debug.Log("Collision detected with: " + other.name);
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
