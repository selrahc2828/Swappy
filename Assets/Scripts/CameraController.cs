using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Controls controls;
    private Vector2 _moveMouseVector;
    public Transform player;
    public Camera mainCamera;
    public Transform orientation;
    public Transform cameraFPSPos;
    public Transform cameraTPSPos;

    public float sensitivity = 100f;

    [Header("FPS")]
    public MeshRenderer eyes;
    private float _xRot;
    [Tooltip("Vers le bas")]
    public float xRotMin;
    [Tooltip("Vers le haut")]
    public float xRotMax;
    private float _yRot;


    [Header("Variations")]
    public bool isFPS = true;

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;

        controls.Player.SwitchCam.performed += SwitchCam;
    }

    // Update is called once per frame
    void Update()
    {

        if (isFPS)
        {
            mainCamera.transform.position = cameraFPSPos.position;
            //eyes.enabled = false;
        }
        else
        {
            mainCamera.transform.position = cameraTPSPos.position;
            //eyes.enabled = false;
        }
        

        // ------- à mettre en fixed update ? 

        _moveMouseVector = controls.Player.Look.ReadValue<Vector2>();
        //Debug.Log(moveMouseVector);

        // roation player sur Y
        float mouseX = _moveMouseVector.x * sensitivity * Time.deltaTime;
        _yRot += mouseX;
        //player.Rotate(Vector3.up * _yRot);

        // rotation camera sur x (bas/haut)
        float mouseY = _moveMouseVector.y * sensitivity * Time.deltaTime;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, xRotMin, xRotMax);

        mainCamera.transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);// y à 0f si cam dans player
        //mainCamera.transform.eulerAngles = new Vector3(-mouseX, xRot, 0f); // -_yRot, xRot, _zTilt + _slideTilt
        orientation.localRotation = Quaternion.Euler(0f, _yRot, 0f); // pour player dans le controller
    }

    public void SwitchCam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Switch");

            isFPS = !isFPS;
        }
    }
}
