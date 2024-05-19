using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Volume globalVolume;

    [Header("Parameters")]
    [SerializeField] private float vignetteMaxIntensity = 0.5f;

    [SerializeField] private float vignetteIntensityChangeRate = 0.05f;
    [SerializeField] private float aborrationIntensityChangeRate = 0.05f;

    private Vignette vignette;
    private ChromaticAberration aberration;

    private void OnEnable()
    {
        gameManager.onNewDeadTree += UpdateVignetteIntensity;
    }

    private void OnDisable()
    {
        gameManager.onNewDeadTree -= UpdateVignetteIntensity;
    }

    void Start()
    {
        if (globalVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }

        if (globalVolume.profile.TryGet(out ChromaticAberration a))
        {
            aberration = a;
        }
    }

    void UpdateVignetteIntensity()
    {
        StartCoroutine(AborrationSlowDecrease());

        if (vignette.intensity.value >= vignetteMaxIntensity) return;
        StartCoroutine(VignetteSlowDecrease());
    }

    private IEnumerator VignetteSlowDecrease()
    {
        while(vignette.intensity.value < vignetteIntensityChangeRate)
        {
            vignette.intensity.value += 0.001f;
            yield return null;
        }

        vignetteIntensityChangeRate += 0.05f;
    }

    private IEnumerator AborrationSlowDecrease()
    {
        while (aberration.intensity.value < aborrationIntensityChangeRate)
        {
            aberration.intensity.value += 0.001f;
            yield return null;
        }

        aborrationIntensityChangeRate += 0.05f;
    }
}
