using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireFoam;
    [SerializeField] private LayerMask fireMask;

    private bool spray = false;

    public void Spray(bool isSpraying)
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (!enabled) return;

        spray = isSpraying;

        if (spray)
            fireFoam.Play();
        else
            fireFoam.Stop();
    }
}
