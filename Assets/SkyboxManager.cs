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
    public float rotationSpeed;

    private void Update()
    {
        Sun.transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        LightAngle = CalculateSunPlayerAngle.NormalizedAngleBetweenLightAndPlayer;
        SkyboxMat.SetFloat("_CubemapTransition", Mathf.Abs(LightAngle));
    }
}
