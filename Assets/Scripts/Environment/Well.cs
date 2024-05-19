using System.Collections;
using UnityEngine;

public class Well : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private HandController handController;
    [SerializeField] private ParticleSystem waterParticleSystemPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip waterSound;

    private AudioSource audioSource;

    private bool isPlayerClose = false;
    private bool isSplashing = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        handController.onClick += HandleSplash;
    }

    private void OnDisable()
    {
        handController.onClick -= HandleSplash;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            isPlayerClose = false;
        }
    }

    private void HandleSplash()
    {
        if (isSplashing || !isPlayerClose) return;

        isSplashing = true;

        waterParticleSystemPrefab.Play();
        audioSource.PlayOneShot(waterSound);

        StartCoroutine(ResetSplashState());
    }

    private IEnumerator ResetSplashState()
    {
        yield return new WaitForSeconds(waterSound.length);

        isSplashing = false;
    }
}
