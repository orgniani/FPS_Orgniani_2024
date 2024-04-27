using UnityEngine;

public class AttackSwapController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GunController gunController;
    [SerializeField] private FireExtinguisherController fireExtinguisherController;

    [Header("Weapons GameObjects")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject fireExtinguisher;

    public void SwapToFireExtinguisher()
    {
        if (fireExtinguisher.activeSelf) return;

        gun.SetActive(false);
        gunController.enabled = false;


        fireExtinguisher.SetActive(true);
        fireExtinguisherController.enabled = true;

    }

    public void SwapToGun()
    {
        if (gun.activeSelf) return;

        fireExtinguisher.SetActive(false);
        fireExtinguisherController.enabled = false;

        gun.SetActive(true);
        gunController.enabled = true;
    }

}
