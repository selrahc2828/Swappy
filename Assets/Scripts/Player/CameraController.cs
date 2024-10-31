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

    public Camera mainCamera;
    public Transform orientation;
    public Transform camPosition;
    public Transform playerRender; // model

    [Header("Parameters")]
    [Tooltip("Vers le bas")]
    public float xRotMin;
    [Tooltip("Vers le haut")]
    public float xRotMax;
    private float _xRot;
    private float _yRot;
    public float sensitivityX = 5f;
    public float sensitivityY = 5f;
    [Range(0,179)]
    public float fov = 40f;

    void Start()
    {
        controls = GameManager.controls;
    }

    //appel quand une valeur de l'inspecteur est changé
    private void OnValidate()
    {
        mainCamera.fieldOfView = fov;
    }

    void Update()
    {
        mainCamera.transform.position = camPosition.position;


        _moveMouseVector = controls.Player.Look.ReadValue<Vector2>();

        // roation player sur Y
        float mouseX = _moveMouseVector.x * sensitivityX * Time.deltaTime;
        _yRot += mouseX;

        // rotation camera sur x (bas/haut)
        float mouseY = _moveMouseVector.y * sensitivityY * Time.deltaTime;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, xRotMin, xRotMax);

        mainCamera.transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);// y à 0f si cam dans player

        // si utilise player, il arrive que les 2 ne soit pas synchro = mouvement ne se fait plus par rapport à la rotation du player/camera
        orientation.rotation = Quaternion.Euler(0f, _yRot, 0f);
        playerRender.rotation = Quaternion.Euler(0f, _yRot, 0f);
        // interractor placé dans la camera

    }


}
