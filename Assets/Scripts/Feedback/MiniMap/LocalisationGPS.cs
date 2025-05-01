using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationGPS : MonoBehaviour
{
    public GameObject miniMapPlayer;
    public GameObject miniMapPlanet;
    public GameObject miniMapMainPlanetCore;
    public GameObject mainPlanetCore;
    
    [Range(0.0001f, 1f)]
    public float scaleRatio = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 relativeLocalPos = mainPlanetCore.transform.InverseTransformPoint(GameManager.Instance.player.transform.position);
        Vector3 scaledLocalPos = relativeLocalPos * scaleRatio;
        miniMapPlayer.transform.position = miniMapMainPlanetCore.transform.TransformPoint(scaledLocalPos);
        if(Input.GetKeyDown(KeyCode.M))
        {
            miniMapPlanet.SetActive(!miniMapPlanet.activeInHierarchy);
            miniMapPlayer.SetActive(!miniMapPlayer.activeInHierarchy);
        }
    }   
}
