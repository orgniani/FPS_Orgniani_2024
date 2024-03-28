using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScriptsOnDeath : MonoBehaviour
{
    [SerializeField] private HealthController HP;
    [SerializeField] private FirstPersonController FPS;
    [SerializeField] private CharacterShooting gun;

    [SerializeField] private CharacterController CC;
    private void OnEnable()
    {
        HP.onDead += HandleDeath;
    }

    private void OnDisable()
    {
        HP.onDead -= HandleDeath;
    }

    private void HandleDeath()
    {
        HP.enabled = false;
        FPS.enabled = false;
        gun.enabled = false;

        CC.enabled = false;
    }
}
