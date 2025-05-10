using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public enum FeelCurveType
{
    CameraOnJump,
    CameraOffJump,
    CameraOnGround,
    CameraOnWall
}

public class FeelCurve
{
    public AnimationCurve curve;
    public FeelCurveType curveType;
    public float curveTime;
    public float curveTimer;

    public float tickCurve()
    {
        curveTimer += Time.deltaTime;
        return curve.Evaluate(curveTimer/curveTime);
    }
}

public class CurveManager : MonoBehaviour
{
    public List<FeelCurve> activeCurves = new List<FeelCurve>();

    public FeelCurve StartCurve(AnimationCurve curve, FeelCurveType curveType, float curveTime)
    {
        activeCurves.Add(new FeelCurve());
        FeelCurve startedCurve = activeCurves[activeCurves.Count];

        startedCurve.curve = curve;
        startedCurve.curveTime = curveTime;

        foreach (FeelCurve feelCurve in activeCurves)
        {
            if (feelCurve.curveType == curveType)
            {

            }
        }

        return startedCurve;
    }

    public void Update()
    {

    }

}