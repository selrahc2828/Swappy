using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateSunPlayerAngle : MonoBehaviour
{
    public Vector3 lightDirection;
    public GameObject player;
    public Vector3 playerUpDirection;
    public float NormalizedAngleBetweenLightAndPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;       
    }

    // Update is called once per frame
    void Update()
    {
        lightDirection = transform.forward;
        playerUpDirection = player.transform.up;
        float angleBetweenLightAndPlayer = Vector3.Angle(lightDirection, playerUpDirection);
        NormalizedAngleBetweenLightAndPlayer = 1f - 2f * (angleBetweenLightAndPlayer / 180f);
    }
}
