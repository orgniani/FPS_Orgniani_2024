using System;
using System.Collections;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] private HealthController playerHP;
    [SerializeField] private GunController playerGun;
    [SerializeField] private AttackSwapController attackSwapController;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private ItemType itemType;

    [SerializeField] private float restoredHP = 10f;

    public static event Action onPickUp;

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            switch(itemType)
            {
                case ItemType.AMMO:
                    if (playerGun.AmmoAmount >= playerGun.MaxAmmoAmount && !attackSwapController.AquiredGun) return;

                    playerGun.ReplenishAmmo();
                    gameObject.SetActive(false);
                    break;

                case ItemType.LIFE:

                    if (playerHP.Health >= playerHP.MaxHealth) return;

                    playerHP.RestoreHP(restoredHP);
                    gameObject.SetActive(false);
                    break;

                case ItemType.EXTINGUISHER:

                    attackSwapController.AquiredExtinguisher = true;
                    onPickUp?.Invoke();

                    gameObject.SetActive(false);
                    break;

                case ItemType.GUN:

                    attackSwapController.AquiredGun = true;
                    onPickUp?.Invoke();

                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
