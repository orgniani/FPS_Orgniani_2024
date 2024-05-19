using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem fireFoam;

    [SerializeField] private AudioSource extinguishSound;

    private bool spray = false;

    public void Spray(bool isSpraying)
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (!enabled) return;

        spray = isSpraying;

        if (spray)
        {
            fireFoam.Play();
            extinguishSound.Play();
        }

        else
        {
            fireFoam.Stop();
            extinguishSound.Stop();
        }
    }
}
