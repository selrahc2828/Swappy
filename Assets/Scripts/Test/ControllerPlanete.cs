using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ControllerPlanete : MonoBehaviour
{
    public GameManager gameManager;
    public Rigidbody rb;
    public Vector2 moveInputVector;
    public Vector3 moveDirection;
    public Controls controls;
    public bool grounded;
    public GameObject planete;
    public float playerHeight;
    private LayerMask whatIsGround;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        controls = GameManager.controls;
        
        controls.Player.Jump.performed += Jump;
        controls.Player.StartSprint.performed += StartSprint;
        controls.Player.StopSprint.performed += StopSprint;
        
        playerHeight = gameManager.playerHeight;
        whatIsGround = gameManager.whatIsGround;
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= Jump;
        controls.Player.StartSprint.performed -= StartSprint;
        controls.Player.StopSprint.performed -= StopSprint;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        throw new NotImplementedException();
    }

    void FixedUpdate()
    {
        Vector3 groundDirection = planete.transform.position - transform.position;
        //ground check
        grounded = Physics.Raycast(transform.position, groundDirection, playerHeight * 0.5f + 0.3f, whatIsGround);
        
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;
        //calculate movement direction
        moveDirection = transform.forward * moveInputVector.y + transform.right * moveInputVector.x;
        
        rb.AddForce(moveDirection * 10f, ForceMode.Acceleration);
    }
    
    private void StartSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }
    private void StopSprint(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            rb.AddForce(transform.up * 10f, ForceMode.VelocityChange);
        }
    }
}
