using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private bool matchYaxis;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 pos = cam.transform.position;
        if (matchYaxis)
            pos.y = transform.position.y;

        transform.LookAt(pos, Vector3.up);
    }
}
