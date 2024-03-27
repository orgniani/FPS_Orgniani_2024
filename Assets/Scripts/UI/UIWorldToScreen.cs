using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldToScreen : MonoBehaviour
{
    public Transform enemy;
    public float angle;

    RectTransform thisRT;
    Camera cam;

    private void Start()
    {
        thisRT = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        //Vector3 enemyCamDir = enemy.transform.position - cam.transform.position;
        //angle = Vector3.Angle(cam.transform.forward, enemyCamDir);

        thisRT.position = cam.WorldToScreenPoint(enemy.position);
    }
}
