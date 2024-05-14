using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private HandController handController;

    [SerializeField] private AnimationCurve curveColor;

    [SerializeField] private Material sharedMaterial;

    [SerializeField] private Color originalColor;
    [SerializeField] private Color pulseColor;

    [SerializeField] private GameObject instructionsCanvas;

    private void Awake()
    {
        sharedMaterial = GetComponent<Renderer>().material;
        originalColor = sharedMaterial.color;
    }

    private void Update()
    {
        StartCoroutine(Pulse());

        if (handController.IsDraggingEnemy && handController.IsAtTheDoor)
        {
            instructionsCanvas.SetActive(true);
        }

        else
        {
            instructionsCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("Player is at the door!");
            if (handController.IsDraggingEnemy)
            {
                Debug.Log("Player is holding a goblin!");
                handController.IsAtTheDoor = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("Player left the door!");
            handController.IsAtTheDoor = false;
        }
    }

    private IEnumerator Pulse()
    {
        float tt = 0;

        if (!handController.IsDraggingEnemy)
        {
            sharedMaterial.color = originalColor;
            yield break;
        }

        while (handController.IsDraggingEnemy)
        {
            tt += Time.deltaTime;
            float valorDe0a1 = tt / 1f;

            float colorVal = curveColor.Evaluate(valorDe0a1);

            sharedMaterial.color = Color.Lerp(originalColor, pulseColor, colorVal);

            yield return null;
        }
    }
}
