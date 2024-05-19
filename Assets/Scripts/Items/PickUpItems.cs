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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickUpItemSound;

    [Header("Arrow animation parameters")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private float posYPosition = 1f;

    private Vector3 initialPosition;

    public static event Action onPickUp;

    private void Start()
    {
        if(arrow == null)
        {
            enabled = false;
            return;
        }

        initialPosition = arrow.transform.position;
        StartCoroutine(Float());
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

                    gameObject.SetActive(false);
                    break;

                case ItemType.GUN:

                    attackSwapController.AquiredGun = true;
                    attackSwapController.SwapToGun();

                    onPickUp?.Invoke();

                    gameObject.SetActive(false);
                    break;
            }
        }
    }


    private IEnumerator Float()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                float curveValue = animationCurve.Evaluate(t);

                Vector3 targetPosition = initialPosition + Vector3.up * posYPosition * curveValue;

                arrow.transform.position = Vector3.Lerp(arrow.transform.position, targetPosition, Time.deltaTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            posYPosition = -posYPosition;
        }

    }
}
