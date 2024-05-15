using System;
using System.Collections;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController playerHP;
    [SerializeField] private GunController playerGun;
    [SerializeField] private AttackSwapController attackSwapController;

    [Header("Types")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private ItemType itemType;

    [Header("Meds Parameters")]
    [SerializeField] private float restoredHP = 10f;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickUpItemSound;

    [Header("Glow animation parameters")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float animationDuration = 2f;

    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] private float maxGlowIntensity = 0.3f;

    [SerializeField] private Material sharedMaterial;

    public static event Action onPickUp;

    private void Start()
    {
        if (sharedMaterial == null) return;

        StartCoroutine(Glow());
        sharedMaterial.DisableKeyword("_EMISSION");
    }

    private void OnDisable()
    {
        if (sharedMaterial == null) return;

        sharedMaterial.DisableKeyword("_EMISSION");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            switch(itemType)
            {
                case ItemType.AMMO:
                    if (playerGun.AmmoAmount >= playerGun.MaxAmmoAmount || !attackSwapController.AquiredGun) return;

                    audioSource.PlayOneShot(pickUpItemSound);

                    playerGun.ReplenishAmmo();
                    gameObject.SetActive(false);
                    break;

                case ItemType.LIFE:

                    if (playerHP.Health >= playerHP.MaxHealth) return;

                    audioSource.PlayOneShot(pickUpItemSound);

                    playerHP.RestoreHP(restoredHP);
                    gameObject.SetActive(false);
                    break;

                case ItemType.EXTINGUISHER:

                    attackSwapController.AquiredExtinguisher = true;
                    attackSwapController.SwapToFireExtinguisher();

                    onPickUp?.Invoke();

                    StopGlow();
                    gameObject.SetActive(false);
                    break;

                case ItemType.GUN:

                    attackSwapController.AquiredGun = true;
                    attackSwapController.SwapToGun();

                    onPickUp?.Invoke();

                    StopGlow();
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void StopGlow()
    {
        StopCoroutine(Glow());
        sharedMaterial.DisableKeyword("_EMISSION");
    }

    private IEnumerator Glow()
    {
        while (true)
        {
            yield return AnimateIntensity(0f, maxGlowIntensity);

            yield return AnimateIntensity(maxGlowIntensity, 0f);
        }
    }

    private IEnumerator AnimateIntensity(float startIntensity, float endIntensity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = Mathf.Lerp(startIntensity, endIntensity, animationCurve.Evaluate(t));

            Color finalColor = glowColor * curveValue;
            sharedMaterial.SetColor("_EmissionColor", finalColor);

            sharedMaterial.EnableKeyword("_EMISSION");

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sharedMaterial.DisableKeyword("_EMISSION");
    }
}
