using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public static Controls controls;
    public Camera mainCamera;
    public ComponentStealer stealPasteSript;
    //private CameraController cameraScript;



    [Header("EN COURS POUR FAIRE LE FPS")]
    [Space]
    [Header("Controller Properties")]
    public Rigidbody rb;
    public Transform root;//point pivot
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask floorMask;
    public float moveSpeed;
    public float rotationSpeed;
    public float jumpForce;
    public Vector2 moveInputVector;
    public Vector3 moveDir;
    public bool isGrounded;



    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;
        // assigne action du controleur au methode
        controls.PlayerGod.StopTime.performed += StopTime;
        controls.PlayerGod.CopySteal.performed += CopyStealComp;
        controls.PlayerGod.PasteSteal.performed += PasteComp;
        controls.PlayerGod.ReloadScene.performed += ReloadScene;
        //controls.PlayerGod.Jump.performed += Jump;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        controls.PlayerGod.StopTime.performed -= StopTime;
        controls.PlayerGod.CopySteal.performed -= CopyStealComp;
        controls.PlayerGod.PasteSteal.performed -= PasteComp;
        controls.PlayerGod.ReloadScene.performed -= ReloadScene;
        //controls.PlayerGod.Jump.performed -= Jump;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {

    }

    private void OnEnable()
    {

    }

    private void ReloadScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StopTime(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // if bool == true set false et vice versa
        }
    }
    private void CopyStealComp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("StealComp  _mvtData.type : " + _mvtData.type);
            //isStealing = true;
            stealPasteSript.CopyStealComp();
        }
    }

    private void PasteComp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("PasteComp  _mvtData.type : " + _mvtData.type);
            //isStealing = false;
            stealPasteSript.PasteComp();
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("jump");
        if (Grounded())//marche po
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //isGrounded = false;
        }
    }

    public bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, floorMask);
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
