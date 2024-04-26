using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireFoam;
    [SerializeField] private LayerMask fireMask;


    private bool spray = false;

    private void Update()
    {
        if(!spray) return;
        SprayFoam();
    }

    public void Spray(bool isSpraying)
    {
        spray = isSpraying;
    }

    public void SprayFoam()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        fireFoam.Play();
    }
}
