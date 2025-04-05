using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    
    float xRotation;
    float yRotation;

    private Ray _ray;
    public LayerMask hitLayer;
    private Transform orientation;

    void Start()
    {
        orientation = GameManager.Instance.orientation.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //get mouse input
        if (!GameManager.Instance.isPaused)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * GameManager.Instance.parameters.sensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * GameManager.Instance.parameters.sensitivity;
        
        
            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
        }
        
    }
}
