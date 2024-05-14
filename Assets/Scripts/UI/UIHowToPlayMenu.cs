using System.Collections;
using UnityEngine;
using TMPro;

public class UIHowToPlayMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    private string[] instructions = new string[]
    {
        "1. Combat the Goblins:\n\nThe forest (and you) are under attack by goblins! Use your tranquilizer gun to make them fall asleep.",
        "2. Secure the Goblins:\n\nOnce a goblin is asleep, disarm and drag it to the nearest cabin to trap it inside!",
        "3. Act Quickly:\n\nThe tranquilizer effect is short-lived, so move fast! Trap the goblin before it wakes up again.",
        "4. Extinguish Fires:\n\nUse the fire extinguisher to put out any fires before the forest is destroyed.",
        "5. Win!\n\nTrap all the goblins before you or the forest are destroyed to win!"
    };

    private int currentInstructionIndex = 0;

    private void OnEnable()
    {
        currentInstructionIndex = 0;
        UpdateInstructionText();
    }

    public void OnNextPage()
    {
        currentInstructionIndex = Mathf.Min(currentInstructionIndex + 1, instructions.Length - 1);
        UpdateInstructionText();
    }


    public void OnBackPage()
    {
        currentInstructionIndex = Mathf.Max(currentInstructionIndex - 1, 0);
        UpdateInstructionText();
    }

    private void UpdateInstructionText()
    {
        StopFadeCoroutine();
        instructionText.text = instructions[currentInstructionIndex];
        fadeCoroutine = StartCoroutine(FadeInstructionText());
    }

    private IEnumerator FadeInstructionText()
    {
        float elapsedTime = 0f;
        Color startColor = instructionText.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float valueFrom0To1 = elapsedTime / fadeDuration;
            float t = animationCurve.Evaluate(valueFrom0To1);
            instructionText.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        //instructionText.color = targetColor;
    }

    private void StopFadeCoroutine()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
    }


}
