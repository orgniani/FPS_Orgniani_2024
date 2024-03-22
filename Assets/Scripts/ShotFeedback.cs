using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFeedback : MonoBehaviour
{
    [SerializeField] private float gunRange;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShowShotDirection(Vector3 endPosition)
    {
        // Calculate the distance between start and end position
        float distance = Vector3.Distance(transform.position, endPosition);

        // Ensure the line doesn't exceed the calculated distance
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(transform.position + Vector3.ClampMagnitude(endPosition - transform.position, distance)));

        Destroy(gameObject,0.1f);
    }
}
