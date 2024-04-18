using System.Collections;
using TMPro;
using UnityEngine;

public class UITypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [SerializeField] private float delayBeforeStart = 0f;
    [SerializeField] private float timeBtwChars = 0.1f;
    [SerializeField] private string leadingChar = "";
    [SerializeField] private bool leadingCharBeforeDelay = false;

    [Space(10)]
    [SerializeField] private bool startOnEnable = true;

    private string writer;

    private void Awake()
    {
        if (text != null)
        {
            writer = text.text;
        }
    }

    private void Start()
    {
        if (text != null)
        {
            text.text = "";
        }
    }

    private void OnEnable()
    {
        if (startOnEnable) StartTypewriter();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void StartTypewriter()
    {
        StopAllCoroutines();

        if (text != null)
        {
            text.text = "";

            StartCoroutine(TypeWriterEffect());
        }
    }

    private IEnumerator TypeWriterEffect()
    {
        text.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in writer)
        {
            if (text.text.Length > 0)
            {
                text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
            }
            text.text += c;
            text.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
        }
    }
}
