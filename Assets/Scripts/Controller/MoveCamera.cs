using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void Start()
    {
        cameraPosition = GameObject.FindGameObjectWithTag("CameraPosition").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
        transform.rotation = GameManager.Instance.player.transform.rotation;
    }
}