using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFeedback : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShowShotDirection()
    {
        //lineRenderer.SetPosition();
    }
}