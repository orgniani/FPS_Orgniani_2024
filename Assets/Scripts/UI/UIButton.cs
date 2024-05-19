using UnityEngine;

public class UIButton : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverButton;
    [SerializeField] private AudioClip clickButton;

    public void HoverSound()
    {
        if(hoverButton)
            audioSource.PlayOneShot(hoverButton);
    }

    public void ClickSound()
    {
        if(clickButton)
            audioSource.PlayOneShot(clickButton);
    }
}
