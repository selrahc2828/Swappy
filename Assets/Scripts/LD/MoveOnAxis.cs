using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnAxis : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float offsetTime;
    [SerializeField] private AnimationCurve offsetCurve;

    private Vector3 offsetPointA;
    private Vector3 offsetPointB;
    private Vector3 basePosition;
    private float offsetTimer;
    private bool isAxisActive;

    private void Start()
    {
        basePosition = transform.localPosition;
    }
    private void Update()
    {
        if (isAxisActive)
        {
            transform.localPosition = basePosition + OffsetAxisTick();
        }
    }

    public void OffsetAxisStart()
    {
        isAxisActive = true;
        offsetTimer = 0;
        offsetPointA = Vector3.zero;
        offsetPointB = offset;
    }

    private Vector3 OffsetAxisTick()
    {
        offsetTimer += Time.deltaTime;
        if (offsetTimer > offsetTime) 
        {
            OffsetAxisEnd();
        }
        Mathf.Max(offsetTimer, offsetTime);
        Vector3 calculatedOffset = Vector3.Lerp(offsetPointA, offsetPointB, offsetCurve.Evaluate(offsetTimer / offsetTime));
        return calculatedOffset;
    }

    public void OffsetAxisEnd()
    {
        basePosition += offset;
        isAxisActive = false;
    }
}
