using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public float LightAngle;
    public CalculateSunPlayerAngle CalculateSunPlayerAngle;
    public Material SkyboxMat;
    public Material WaterSkyEffect;
    public GameObject Sun;
    public float NumberOfMinuteForOneDay;

    public Material Grass1;
    public Material Grass2;
    public Material Grass3;

    private void Update()
    {
        Sun.transform.Rotate((360 / (NumberOfMinuteForOneDay * 60)) * Time.deltaTime, 0, 0);
        LightAngle = CalculateSunPlayerAngle.valueToUseInShader;
        SkyboxMat.SetFloat("_CubemapTransition", Mathf.Abs(LightAngle));
        WaterSkyEffect.SetFloat("_OpacityTransition", Mathf.Abs(LightAngle));
        
        Grass1.SetFloat("_EmissiveMult", Mathf.Abs(LightAngle));
        Grass2.SetFloat("_EmissiveMult", Mathf.Abs(LightAngle));
        Grass3.SetFloat("_EmissiveMult", Mathf.Abs(LightAngle));
    }
}
