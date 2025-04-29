using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateSunPlayerAngle : MonoBehaviour
{
    public Vector3 lightDirection;
    public Vector3 playerUpDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        lightDirection = transform.forward;
        playerUpDirection = GameManager.Instance.player.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        float angleBetweenLightAndPlayer = Vector3.Angle(lightDirection, playerUpDirection);
        float NormalizedAngleBetweenLightAndPlayer = 1f - 2f * (angleBetweenLightAndPlayer / 180f);
    }
}
