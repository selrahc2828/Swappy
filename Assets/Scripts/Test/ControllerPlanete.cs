using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using Input = UnityEngine.Input;

[RequireComponent(typeof(Rigidbody))]
public class ControllerPlanete : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody rb;
    private Vector2 moveInputVector;
    private Vector3 moveDirection;
    private Controls controls;
    private float playerHeight;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float aerialMultiplier;
    [SerializeField] private float stoppingRatio;
    [SerializeField] private float sideSpeedReductionRatio;
    [SerializeField] private float jumpForce;
    
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isStopping;
    [SerializeField] private bool grounded;
    [SerializeField] private bool touchingInclinedSurface;

    private LayerMask whatIsGround;
    private GravityPlanete gravityComponent;
    [SerializeField] private Vector3 touchingInclinedSurfaceDirection;
    
    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        controls = GameManager.controls;
        
        controls.Player.Movement.performed += MovementAttack;
        controls.Player.Movement.canceled += MovementAttack;
        controls.Player.Jump.performed += Jump;
        controls.Player.StartSprint.performed += StartSprint;
        controls.Player.StopSprint.performed += StopSprint;

        moveSpeed = gameManager.walkSpeed;
        jumpForce = gameManager.jumpForce;
        playerHeight = gameManager.playerHeight;
        whatIsGround = gameManager.whatIsGround;
    }

    private void OnDisable()
    {
        controls.Player.Movement.performed -= MovementAttack;
        controls.Player.Movement.canceled -= MovementAttack;
        controls.Player.Jump.performed -= Jump;
        controls.Player.StartSprint.performed -= StartSprint;
        controls.Player.StopSprint.performed -= StopSprint;
    }

    // Start is called before the first frame update
    void Start()
    {
        isStopping = true;
        rb = GetComponent<Rigidbody>();
        gravityComponent = GetComponent<GravityPlanete>();
    }

    private void Update()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;

        // Calculate movement direction using GravityPlanete
        if (gravityComponent != null)
        {
            moveDirection = Vector3.ProjectOnPlane(
                GameManager.Instance.orientation.transform.forward * moveInputVector.y +
                GameManager.Instance.orientation.transform.right * moveInputVector.x,
                transform.up
            ).normalized;
        }
        
        //est ce que je touche un mur et l'input de mouvement est vers le mur
        if (touchingInclinedSurface && Vector3.Dot(moveDirection, touchingInclinedSurfaceDirection) < 0)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, touchingInclinedSurfaceDirection);
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        
        Vector3 localHorizontalVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.up);
        
        float orientationValue = 1;
        float aerialMultiplierValue = grounded ? 1 : aerialMultiplier;
        float sprintMultiplierValue = isSprinting ? sprintMultiplier : 1;
        
        if (grounded)
        {
            if (rb.velocity.magnitude > 1f)
            {
                orientationValue = ((Vector3.Dot(moveDirection.normalized, rb.velocity.normalized) -1) /-2) + 1;
            }
            if (moveInputVector.y != 0)
            {
                Vector3 velocityWithoutSides = Vector3.ProjectOnPlane(rb.velocity, transform.right);
                
                rb.velocity = Vector3.Lerp(rb.velocity, velocityWithoutSides, sideSpeedReductionRatio);
            }
            if (isStopping)
            {
                rb.velocity = rb.velocity * stoppingRatio;
            }
        }
        if (localHorizontalVelocity.magnitude > maxSpeed * sprintMultiplierValue && Vector3.Dot(moveDirection, localHorizontalVelocity) > 0)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, localHorizontalVelocity);
        }
        rb.AddForce(moveDirection * (moveSpeed * orientationValue * aerialMultiplierValue * sprintMultiplierValue), ForceMode.Acceleration);
    }

    void GroundCheck()
    {
        Vector3 groundDirection = -transform.up;
        grounded = false;
        touchingInclinedSurface = false;

        if (Physics.Raycast(transform.position, groundDirection, out RaycastHit hit, playerHeight * 0.5f + 0.2f, whatIsGround))
        {
            float slopeAngle = Vector3.Angle(hit.normal, -groundDirection);
            if (slopeAngle < 75f) // Seulement consid�r� comme sol si l'angle est faible
            {
                grounded = true;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.GetContact(0).thisCollider.CompareTag("AntiStick"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                float slopeAngle = Vector3.Angle(contact.normal, -transform.up);
                
                if (slopeAngle > 90f && slopeAngle < 125f) // Surface inclin�e d�tect�e
                {
                    touchingInclinedSurface = true;
                    touchingInclinedSurfaceDirection = contact.normal;
                }
            }
        }
    }

    private void MovementAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isStopping = false;
        }
        if(context.canceled)
        {
            isStopping = true;
        }
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            isSprinting = true;
        }
    }
    private void StopSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = false;
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
    }
}
