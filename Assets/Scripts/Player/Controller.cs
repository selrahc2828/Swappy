﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class Controller : MonoBehaviour
{
    public static Controls controls;
    public Camera mainCamera;
    public ComponentStealer stealPasteSript;
    public GrabObject carryingScript;
    
    //private CameraController cameraScript;

    public TextMeshProUGUI speedText;

    [Header("Controller Properties")]
    public Rigidbody rb;
    public Transform root;//pas utile actuellement voir pour les slope / angle à monter
    public Transform orientation;
    public Transform playerObjRenderer;
    public float renderRotationSpeed = 20f;

    public bool timeIsStop;

    public float moveForce;
    public float maxSpeed;
    private float rotationSpeed;
    private Vector2 moveInputVector;
    [Tooltip("Frottement sur le rigidbody, ça le ralenti")]
    public float groundDrag;
    public float gravityForceY = 5f;
    
    private Vector2 moveLookVector;
    private Vector3 moveDir;
    private Vector3 moveVector;

    //public float groundDrag;//pour le player qui glisse
    //public float actualInertiaScale = 500f;
    [Header("Jump")]
    public float jumpForce;
    [Tooltip("Réduction de la force appliquée dans les air (0 à1 )")]
    [Range(0,1)]
    public float airModifier;
    //cooldown ? 

    [Header("GroundCheck")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask floorMask;
    private bool isGrounded;

    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;
        // assigne action du controleur au methode
        controls.Player.StopTime.performed += StopTime;
        controls.Player.CopySteal.performed += CopyStealComp;
        controls.Player.PasteSteal.performed += PasteComp;
        controls.Player.PasteMe.performed += PasteAtMe;
        controls.Player.Jump.performed += Jump;
        controls.Player.GrabDrop.performed += GrabAndDrop;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

private void OnDisable()
    {
        controls.Player.StopTime.performed -= StopTime;
        controls.Player.CopySteal.performed -= CopyStealComp;
        controls.Player.PasteSteal.performed -= PasteComp;
        controls.Player.Jump.performed -= Jump;
        controls.Player.GrabDrop.performed -= GrabAndDrop;

    }

    void Update()
    {

        speedText.text = rb.velocity.magnitude.ToString();

        if (timeIsStop)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

    }

    private void FixedUpdate()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;//input
        handleMovement();
        handleGravity();

        if (Grounded())
        {
            rb.drag = groundDrag;//applique un "frottement" par defaut au sol
            rb.useGravity = false;
        }
        else
        {
            rb.drag = 0f;
            rb.useGravity = true;
        }
    }

    private void StopTime(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // if bool == true set false et vice versa
            timeIsStop = !timeIsStop;
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

    private void PasteAtMe(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("PasteComp  _mvtData.type : " + _mvtData.type);
            //isStealing = false;
            stealPasteSript.PasteAtMe();
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("jump");
            if (Grounded())//marche po
            {
                //reset
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                //isGrounded = false;
            }
        }
    }
    void GrabAndDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (carryingScript)
            {
                if (carryingScript.isCarrying)
                {
                    carryingScript.Drop();
                }
                else
                {
                    carryingScript.Carrying();

                }

            }
        }
    }

    void handleMovement()
    {

        //moveDir = root.TransformDirection(new Vector3(moveInputVector.x, 0f, moveInputVector.y));
        moveDir = transform.forward * moveInputVector.y + transform.right * moveInputVector.x;//orientation
        Quaternion targetRotation;
        if (moveDir != Vector3.zero && !GameManager.Instance.camControllerScript.isFPS)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
            playerObjRenderer.rotation = Quaternion.Slerp(playerObjRenderer.rotation, targetRotation, Time.deltaTime * renderRotationSpeed);
        }



        if (Grounded())
        {
            rb.AddForce(moveDir.normalized * moveForce, ForceMode.Force);// * 10f
        }
        else
        {
            //in air
            rb.AddForce(moveDir.normalized * moveForce  * airModifier, ForceMode.Force); //* 10f
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);



        //faire un slope / pente 
    }

    void handleGravity()
    {
        //Debug.Log(rb.velocity.y);

        if (!Grounded())
        {
            rb.velocity -= Vector3.down * (Physics.gravity.y * (gravityForceY) * Time.deltaTime);
        }
    }

    public bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, floorMask);
    }

    void OnDrawGizmos()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

            Gizmos.DrawRay(transform.position, moveDir.normalized * 15);
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, rb.velocity.normalized * 15);

        }
    }
}