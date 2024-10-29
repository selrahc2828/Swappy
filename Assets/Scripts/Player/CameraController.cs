using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    private Controls controls;
    private Vector2 _moveMouseVector;
    public Transform player;
    public Camera mainCamera;
    public Transform orientation;
    public Transform cameraFPSPos;

    [Header("FPS")]
    public Camera FPSCam;
    public GameObject FPSCamObj;
    public MeshRenderer eyes;
    private float _xRot;
    [Tooltip("Vers le bas")]
    public float xRotMin;
    [Tooltip("Vers le haut")]
    public float xRotMax;
    private float _yRot;
    public float sensitivity = 100f;

    [Header("TPS")]
    public float rotationPlayerSpeed;
    public float xAxisSensitivity = 300f;
    public float yAxisSensitivity = 2f;

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = cameraFPSPos.position;

        _moveMouseVector = controls.Player.Look.ReadValue<Vector2>();
        //Debug.Log(moveMouseVector);

        // roation player sur Y
        float mouseX = _moveMouseVector.x * sensitivity * Time.deltaTime;
        _yRot += mouseX;

        // rotation camera sur x (bas/haut)
        float mouseY = _moveMouseVector.y * sensitivity * Time.deltaTime;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, xRotMin, xRotMax);

        mainCamera.transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);// y à 0f si cam dans player

        player.rotation = Quaternion.Euler(0f, _yRot, 0f);

    }


}
