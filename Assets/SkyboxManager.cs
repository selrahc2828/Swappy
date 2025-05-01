using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public float LightAngle;
    public CalculateSunPlayerAngle CalculateSunPlayerAngle;
    public Material SkyboxMat;
    public GameObject Sun;
    public float NumberOfMinuteForOneDay;

    private void Update()
    {
        Sun.transform.Rotate((360 / (NumberOfMinuteForOneDay * 60)) * Time.deltaTime, 0, 0);
        LightAngle = CalculateSunPlayerAngle.valueToUseInShader;
        SkyboxMat.SetFloat("_CubemapTransition", Mathf.Abs(LightAngle));
    }
}
