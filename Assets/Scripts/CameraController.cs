using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Controls controls;
    private Vector2 _moveMouseVector;
    public Transform player;
    public Transform playerObjRenderer;
    public Camera mainCamera;
    public Transform orientation;
    public Transform cameraFPSPos;
    public Transform cameraTPSPos;

    public float sensitivity = 100f;

    [Header("FPS")]
    public Camera FPSCam;
    public MeshRenderer eyes;
    private float _xRot;
    [Tooltip("Vers le bas")]
    public float xRotMin;
    [Tooltip("Vers le haut")]
    public float xRotMax;
    private float _yRot;

    [Header("TPS")]
    [Tooltip("Groupe Cam cinemachine")]
    public Camera TPSCam_CM;
    public float rotationPlayerSpeed;

    [Header("Variations")]
    public bool isFPS = true;

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;
        controls.Player.SwitchCam.performed += SwitchCam;
        mainCamera = FPSCam;
    }

    // Update is called once per frame
    void Update()
    {
        FPSCam.enabled = isFPS;
        TPSCam_CM.enabled = !isFPS;

        if (isFPS)
        {
            mainCamera = FPSCam;
            mainCamera.transform.position = cameraFPSPos.position;
            //eyes.enabled = false;


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

            mainCamera.transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);// y � 0f si cam dans player

            orientation.localRotation = Quaternion.Euler(0f, _yRot, 0f); // pour player dans le controller

        }
        else
        {
            mainCamera = TPSCam_CM;
            //mainCamera.transform.position = cameraTPSPos.position;
            //eyes.enabled = false;

            //player(script sur player) - cam

            // faire rotate player pas orientation (sinon input bon mais avatar tourne pas)
            Vector3 direction = (player.position - mainCamera.transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction); // Look at the player
            orientation.rotation = Quaternion.Slerp(orientation.rotation, lookRotation, Time.deltaTime * rotationPlayerSpeed);
            //playerObjRenderer.rotation = Quaternion.Slerp(orientation.rotation, lookRotation, Time.deltaTime * rotationPlayerSpeed);
            //player.Rotate(Vector3.up * _yRot);
        }
        

        // ------- � mettre en fixed update ? 


        //mainCamera.transform.eulerAngles = new Vector3(-mouseX, xRot, 0f); // -_yRot, xRot, _zTilt + _slideTilt
        //orientation.localRotation = Quaternion.Euler(0f, _yRot, 0f); // pour player dans le controller
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
