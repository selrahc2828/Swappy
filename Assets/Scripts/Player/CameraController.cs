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
    public Transform cameraTPSPos;


    [Header("Cinemachine Brain")]
    public CinemachineBrain Brain;
    [Tooltip("Vitesse de transition entre camera")]
    public float blendCamSpeed;

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
    [Tooltip("Groupe Cam cinemachine")]
    public Camera TPSCam_CM;
    public GameObject TPSCam_CMObj;
    public float rotationPlayerSpeed;
    public float xAxisSensitivity = 300f;
    public float yAxisSensitivity = 2f;
    public CinemachineFreeLook cinemachineFreeLook;


    [Header("Variations")]
    public bool isFPS = true;

    [Header("Events")]
    public UnityEvent switchCam;//pour grab, sinon reset en décalage

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;
        //controls.Player.SwitchCam.performed += SwitchCam;

        mainCamera = Brain.OutputCamera;
        Brain.m_DefaultBlend.m_Time = blendCamSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        FPSCamObj.SetActive(isFPS);
        TPSCam_CMObj.SetActive(!isFPS);

        if (isFPS)
        {
            mainCamera = FPSCam;
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
        else
        {
            // changer sensi

            mainCamera = TPSCam_CM;
            if (cinemachineFreeLook != null)
            {
                cinemachineFreeLook.m_XAxis.m_MaxSpeed = xAxisSensitivity;
                cinemachineFreeLook.m_YAxis.m_MaxSpeed = yAxisSensitivity;
            }

            // faire rotate player pas orientation (sinon input bon mais avatar tourne pas)
            Vector3 direction = (player.position - mainCamera.transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction); // Look at the player
            orientation.rotation = Quaternion.Slerp(orientation.rotation, lookRotation, Time.deltaTime * rotationPlayerSpeed);

        }
        // ------- à mettre en fixed update ? 
    }

    public void SwitchCam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Switch");

            isFPS = !isFPS;
            switchCam.Invoke();
            //GameManager.Instance.grabScript.ResetCarryPos();
        }
    }
}
