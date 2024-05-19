using System.Collections;
using UnityEngine;

public class CameraWalkEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FirstPersonController FPSController;
    [SerializeField] private AnimationCurve bobbingCurve;

    [Header("Walking animation parameters")]
    [SerializeField] private float walkingBobbingSpeed = 3f;
    [SerializeField] private float walkBobbingAmount = 0.5f;

    [Header("Sprinting animation parameters")]
    [SerializeField] private float sprintingBobbingSpeed = 5f;
    [SerializeField] private float sprintBobbingAmount = 1f;

    [Header("Stop animation parameters")]
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private float soundVolume = 0.2f;

    private AudioSource audioSource;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private float timer = 0f;
    private float bobbingAmount;

    private bool isStepping = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.localPosition;
        StartCoroutine(Bobbing());
    }

    void Update()
    {
        switch (FPSController.movementState)
        {
            case FirstPersonController.MovementState.WALKING:
                timer += Time.deltaTime * walkingBobbingSpeed;
                bobbingAmount = walkBobbingAmount;
                break;
            case FirstPersonController.MovementState.SPRINTING:
                timer += Time.deltaTime * sprintingBobbingSpeed;
                bobbingAmount = sprintBobbingAmount;
                break;
            case FirstPersonController.MovementState.IDLE:
                timer = 0f;
                break;
            default:
                break;
        }

        float bobbingOffset = bobbingCurve.Evaluate(timer) * bobbingAmount;
        targetPosition = new Vector3(0f, bobbingOffset, 0f);

        if (FPSController.movementState == FirstPersonController.MovementState.IDLE)
        {
            targetPosition = initialPosition;
        }
    }

    private IEnumerator Bobbing()
    {
        while (true)
        {
            if (FPSController.movementState != FirstPersonController.MovementState.IDLE)
            {
                float bobbingOffset = bobbingCurve.Evaluate(timer) * bobbingAmount;
                targetPosition = new Vector3(0f, bobbingOffset, 0f);

                if (bobbingCurve.Evaluate(timer) <= 0.01f && !isStepping)
                {
                    isStepping = true;
                    audioSource.PlayOneShot(stepSound, soundVolume);
                    StartCoroutine(ResetStepState());
                }
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smoothTime);

            yield return null;
        }
    }

    private IEnumerator ResetStepState()
    {
        yield return new WaitForSeconds(stepSound.length);
        isStepping = false;
    }
}
