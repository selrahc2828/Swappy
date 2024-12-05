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

    public float sensitivityXSlowTime = 10f;
    public float sensitivityYSlowTime = 10f;

    private float _sensiX, _sensiY;

    [Range(0,179)]
    public float fov = 40f;

    [Header("Projection")] 
    
    public float floatLerp; // Utilisé pour SmoothDamp
    [HideInInspector] public Transform focusTarget;

    void Start()
    {
        controls = GameManager.controls;
        ChangeFocusTarget();
        // floatLerp = 1;
    }

    //appel quand une valeur de l'inspecteur est chang�
    private void OnValidate()
    {
        mainCamera.fieldOfView = fov;
        
    }

    void Update()
    {
        // mainCamera.transform.position = focusTarget.position;

        if (Vector3.Distance(mainCamera.transform.position, focusTarget.position) > 2f) // si projection, pos cam change pos
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, focusTarget.position, floatLerp);
            
            // Vector3 direction = (mainCamera.transform.position - focusTarget.position).normalized;
            // Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // mainCamera.transform.localRotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, floatLerp);
        
        }
        else
        {
            mainCamera.transform.position = focusTarget.position;
        }
        
        if (GameManager.Instance.slowMotion)
        {
            _sensiX = sensitivityXSlowTime;
            _sensiY = sensitivityYSlowTime;
        }
        else
        {
            _sensiX = sensitivityX;
            _sensiY = sensitivityY;
        }

        _moveMouseVector = controls.Player.Look.ReadValue<Vector2>();

        // roation player sur Y
        float mouseX = _moveMouseVector.x * _sensiX * Time.deltaTime;
        _yRot += mouseX;

        // rotation camera sur x (bas/haut)
        float mouseY = _moveMouseVector.y * _sensiY * Time.deltaTime;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, xRotMin, xRotMax);
        
        mainCamera.transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);// y � 0f si cam dans player

        // si utilise player, il arrive que les 2 ne soit pas synchro = mouvement ne se fait plus par rapport � la rotation du player/camera
        if (!GameManager.Instance.etatIsProjected)
        {
            //si pas en projection, on tourne l'avatar
            // voir si on peut faire ça en même temps que la position cam pour pas avoir 2 if
            orientation.rotation = Quaternion.Euler(0f, _yRot, 0f);
            playerRender.rotation = Quaternion.Euler(0f, _yRot, 0f);
        }
        // interractor plac� dans la camera

    }

    public void ChangeFocusTarget(Transform target = null)
    {
        if (target)
        {
            focusTarget = target;
        }
        else
        {
            focusTarget = camPosition;
        }
    }

}
