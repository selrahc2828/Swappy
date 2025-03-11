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
    public float antiStickBaseValue;
    public PhysicMaterial antiStickMaterial;

    public float maxSpeed;
    public float jumpForce;
    public bool isSprinting;
    public bool isStopping;

    public AnimationCurve moveSpeedCurveAttackWalk;
    public AnimationCurve moveSpeedCurveAttackRun;
    public AnimationCurve moveSpeedCurveSustainWalk;
    public AnimationCurve moveSpeedCurveSustainRun;
    public AnimationCurve moveSpeedCurveReleaseWalk;
    public AnimationCurve moveSpeedCurveReleaseRun;
    private float mouvementAttackTime;
    public float mouvementAttackDuration;
    private float mouvementSustainTime;
    private float mouvementReleaseTime;
    public float mouvementReleaseDuration;

    private LayerMask whatIsGround;
    public GravityPlanete gravityComponent;
    public bool touchingInclinedSurface;
    public Vector3 touchingInclinedSurfaceDirection;
    public Vector3 touchingInclinedSurfaceDirectionOnPlanet;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        controls = GameManager.controls;
        
        controls.Player.Movement.performed += MouvementAttack;
        controls.Player.Movement.canceled += MouvementAttack;
        controls.Player.Jump.performed += Jump;
        controls.Player.StartSprint.performed += StartSprint;
        controls.Player.StopSprint.performed += StopSprint;
        
        playerHeight = gameManager.playerHeight;
        whatIsGround = gameManager.whatIsGround;
        antiStickBaseValue = gameManager.playerAntiStick.dynamicFriction;
        antiStickMaterial = gameManager.playerAntiStick;
    }

    private void OnDisable()
    {
        controls.Player.Movement.performed -= MouvementAttack;
        controls.Player.Movement.canceled -= MouvementAttack;
        controls.Player.Jump.performed -= Jump;
        controls.Player.StartSprint.performed -= StartSprint;
        controls.Player.StopSprint.performed -= StopSprint;
    }

    // Start is called before the first frame update
    void Start()
    {
        isStopping = true;
        mouvementAttackTime = 0f; 
        mouvementSustainTime = 0f;
        mouvementReleaseTime = 0f;
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
                gravityComponent.transform.up
            ).normalized;
        }

        if (touchingInclinedSurface)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, touchingInclinedSurfaceDirection).normalized;
        }
    }

    void FixedUpdate()
    {
        GroundCheck();

        float targetVelocity = 0;
        Vector3 movement = moveDirection;

        if(isSprinting)
        {
            if(isStopping)
            {
                float curveTime = Mathf.Clamp01((Time.time - mouvementReleaseTime) / (mouvementReleaseDuration));
                targetVelocity = moveSpeedCurveReleaseRun.Evaluate(curveTime);
            }
            else
            {
                float curveTime = Mathf.Clamp01((Time.time - mouvementAttackTime) / (mouvementSustainTime - mouvementAttackTime)); 
                if (curveTime < 1)
                {
                    targetVelocity = moveSpeedCurveAttackRun.Evaluate(curveTime);
                }
                else
                {
                    targetVelocity = moveSpeedCurveSustainRun.Evaluate(curveTime);
                }
            }
        }
        else
        {
            if(isStopping)
            {
                float curveTime = Mathf.Clamp01((Time.time - mouvementReleaseTime) / (mouvementReleaseDuration));
                targetVelocity = moveSpeedCurveReleaseWalk.Evaluate(curveTime);
            }
            else
            {
                float curveTime = Mathf.Clamp01((Time.time - mouvementAttackTime) / (mouvementSustainTime - mouvementAttackTime)); 
                if (curveTime < 1)
                {
                    targetVelocity = moveSpeedCurveAttackWalk.Evaluate(curveTime);
                }
                else
                {
                    targetVelocity = moveSpeedCurveSustainWalk.Evaluate(curveTime);
                }
            }
        }
        
        movement *= targetVelocity * maxSpeed;

        Vector3 localVerticalVelocity = Vector3.Project(rb.velocity, transform.up);
        
        movement = Vector3.ProjectOnPlane(movement, gravityComponent.transform.up);
        if(touchingInclinedSurface)
        {
            movement = Vector3.ProjectOnPlane(moveDirection, touchingInclinedSurfaceDirection);
        }
        rb.velocity = localVerticalVelocity + movement;
    }

    void GroundCheck()
    {
        Vector3 groundDirection = -gravityComponent.transform.up;
        grounded = false;
        touchingInclinedSurface = false;

        if (Physics.Raycast(transform.position, groundDirection, out RaycastHit hit, playerHeight * 0.5f + 0.3f, whatIsGround))
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
                float slopeAngle = Vector3.Angle(contact.normal, -gravityComponent.transform.up);
                //Debug.Log(slopeAngle);
                if (slopeAngle > 90f && slopeAngle < 125f) // Surface inclin�e d�tect�e
                {
                    touchingInclinedSurface = true;
                    touchingInclinedSurfaceDirection = contact.normal;
                }
            }
        }
    }

    private void MouvementAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            mouvementAttackTime = Time.time;
            mouvementSustainTime = mouvementAttackTime + mouvementAttackDuration;
            isStopping = false;
        }
        if(context.canceled)
        {
            mouvementReleaseTime = Time.time;
            isStopping = true;
        }
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
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
