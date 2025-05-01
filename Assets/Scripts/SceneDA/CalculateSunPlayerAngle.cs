using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateSunPlayerAngle : MonoBehaviour
{
    public GameObject sun;
    public Vector3 lightDirection;
    public GameObject player;
    public Vector3 playerUpDirection;
    public float NormalizedAngleBetweenLightAndPlayer;
    public float valueToUseInShader;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;       
    }

    // Update is called once per frame
    void Update()
    {
        lightDirection = sun.transform.forward;
        playerUpDirection = player.transform.up;
        float angleBetweenLightAndPlayer = Vector3.Angle(lightDirection, playerUpDirection);
        NormalizedAngleBetweenLightAndPlayer = 1f - 2f * (angleBetweenLightAndPlayer / 180f);
        valueToUseInShader = (NormalizedAngleBetweenLightAndPlayer + 1) /2;
    }
}
