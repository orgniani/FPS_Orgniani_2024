using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttackSwapController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GunController gunController;
    [SerializeField] private FireExtinguisherController fireExtinguisherController;

    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float animationDuration = 2;
    [SerializeField] private float posYPosition = 2;

    [Header("Weapons GameObjects")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject fireExtinguisher;

    private Vector3 gunInitialPosition;
    private Vector3 extinguisherInitialPosition;

    private bool canSwitch = true;

    private void Awake()
    {
        gunInitialPosition = gun.transform.localPosition;
        extinguisherInitialPosition = fireExtinguisher.transform.localPosition;
    }

    public void SwapToFireExtinguisher()
    {
        if (!canSwitch) return;

        if (fireExtinguisher.activeSelf) return;

        canSwitch = false;

        fireExtinguisher.transform.localPosition -= Vector3.up * posYPosition;

        if(gun.activeSelf)
        {
            gunController.enabled = false;
            StartCoroutine(AnimateExitSwap(gun));
        }

        StartCoroutine(AnimateSelectSwap(fireExtinguisher, extinguisherInitialPosition));

        //fireExtinguisherController.enabled = true;

    }

    public void SwapToGun()
    {
        if (!canSwitch) return;

        if (gun.activeSelf) return;

        canSwitch = false;

        gun.transform.localPosition -= Vector3.up * posYPosition;

        if(fireExtinguisher.activeSelf)
        {
            fireExtinguisherController.enabled = false;
            StartCoroutine(AnimateExitSwap(fireExtinguisher));
        }

        StartCoroutine(AnimateSelectSwap(gun, gunInitialPosition));

        //gunController.enabled = true;
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
    }

    private IEnumerator AnimateExitSwap(GameObject weapon)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = weapon.transform.localPosition;
        Vector3 targetPosition = initialPosition - Vector3.up * posYPosition;

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

        if(weapon == gun)
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