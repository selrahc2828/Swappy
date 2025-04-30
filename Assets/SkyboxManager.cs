using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public float LightAngle;
    public CalculateSunPlayerAngle CalculateSunPlayerAngle;
    public Material SkyboxMat;

    private void Start()
    {
        
    }

    private void Update()
    {
        LightAngle = CalculateSunPlayerAngle.NormalizedAngleBetweenLightAndPlayer;
        SkyboxMat.SetFloat("_CubemapTransition", Mathf.Abs(LightAngle));
    }
}
