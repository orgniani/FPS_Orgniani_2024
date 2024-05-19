using System.Collections;
using UnityEngine;

public class AttackSwapController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GunController gunController;
    [SerializeField] private FireExtinguisherController fireExtinguisherController;
    [SerializeField] private HandController handController;

    [Header("Swap animation parameters")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float animationDuration = 2;
    [SerializeField] private float posYPosition = 2;

    [Header("Weapon objects")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject fireExtinguisher;

    [Header("Audio")]
    [SerializeField] private AudioClip swapSound;
    private AudioSource audioSource;
    
    private Vector3 gunInitialPosition;
    private Vector3 extinguisherInitialPosition;

    private bool canSwitch = true;

    public bool AquiredExtinguisher { get; set; }
    public bool AquiredGun { get; set; }

    private void Awake()
    {
        gunInitialPosition = gun.transform.localPosition;
        extinguisherInitialPosition = fireExtinguisher.transform.localPosition;

        audioSource = GetComponent<AudioSource>();
    }

    public void SwapToFireExtinguisher()
    {
        if (!AquiredExtinguisher) return;

        if (!canSwitch) return;

        if (fireExtinguisher.activeSelf) return;

        canSwitch = false;

        fireExtinguisher.transform.localPosition -= Vector3.up * posYPosition;

        if (gun.activeSelf)
        {
            gunController.enabled = false;
            StartCoroutine(AnimateExitSwap(gun));
        }

        else
        {
            handController.CanDrag = false;
            audioSource.PlayOneShot(swapSound);
        }

        StartCoroutine(AnimateSelectSwap(fireExtinguisher, extinguisherInitialPosition));
    }

    public void SwapToGun()
    {
        if (!AquiredGun) return;

        if (!canSwitch) return;

        if (gun.activeSelf) return;

        canSwitch = false;

        gun.transform.localPosition -= Vector3.up * posYPosition;

        if (fireExtinguisher.activeSelf)
        {
            fireExtinguisherController.enabled = false;
            StartCoroutine(AnimateExitSwap(fireExtinguisher));
        }

        else
        {
            handController.CanDrag = false;
            audioSource.PlayOneShot(swapSound);
        }

        StartCoroutine(AnimateSelectSwap(gun, gunInitialPosition));
    }

    public void SwapToHands()
    {
        if (!canSwitch) return;

        if (!gun.activeSelf && !fireExtinguisher.activeSelf) return;

        canSwitch = false;

        if (gun.activeSelf)
        {
            gunController.enabled = false;
            StartCoroutine(AnimateExitSwap(gun));
        }

        else
        {
            fireExtinguisherController.enabled = false;
            StartCoroutine(AnimateExitSwap(fireExtinguisher));
        }

        handController.CanDrag = true;
    }

    private IEnumerator AnimateExitSwap(GameObject weapon)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = weapon.transform.localPosition;
        Vector3 targetPosition = initialPosition - Vector3.up * posYPosition;

        audioSource.PlayOneShot(swapSound);

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = animationCurve.Evaluate(t);

            weapon.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, curveValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weapon.SetActive(false);

        if (!gun.activeSelf && !fireExtinguisher.activeSelf) canSwitch = true;
    }

    private IEnumerator AnimateSelectSwap(GameObject weapon, Vector3 weaponInitialPosition)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = weapon.transform.localPosition;
        Vector3 targetPosition = weaponInitialPosition;

        weapon.SetActive(true);

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = animationCurve.Evaluate(t);

            weapon.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, curveValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (weapon == gun)
        {
            gunController.enabled = true;
        }

        else
        {
            fireExtinguisherController.enabled = true;
        }

        canSwitch = true;
    }

}