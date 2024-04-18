using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveTest : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float duration = 2;
    [SerializeField] private float scaleMaxSize = 2;
    [SerializeField] private float t;

    private void Update()
    {
        t += Time.deltaTime;

        if (t >= duration)
            t = 0;

        float ceroToOneValue = t / duration;
        float scale = animationCurve.Evaluate(ceroToOneValue);


    }
}
