using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    [Header("Projection")]
    public float floatLerp; // Utilisé pour SmoothDamp
    public Transform focusTarget;

    private void Start()
    {
        cameraPosition = GameObject.FindGameObjectWithTag("CameraPosition").transform;
        ChangeFocusTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, focusTarget.position) > 2f) // si projection, pos cam change pos
        {
            transform.position = Vector3.Lerp(transform.position, focusTarget.position, floatLerp);
        }
        else
        {
            transform.position = focusTarget.position;
            transform.rotation = GameManager.Instance.player.transform.rotation;
        }
    }
    
    public void ChangeFocusTarget(Transform target = null)
    {
        if (target)
        {
            focusTarget = target;
        }
        else
        {
            focusTarget = cameraPosition;
        }
    }
}
