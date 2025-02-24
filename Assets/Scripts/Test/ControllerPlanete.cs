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

    public float maxSpeed;
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
    }

    private void Update()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;

        //calculate movement direction
        moveDirection = (GameManager.Instance.orientation.transform.forward * moveInputVector.y + GameManager.Instance.orientation.transform.right * moveInputVector.x).normalized;
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

        // Convertit la v�locit� globale en v�locit� locale
        Vector3 localVelocity = transform.InverseTransformDirection(movement);

        // V�rifie si l'objet a une v�locit� significative sur X ou Z en local
        bool hasVelocityOnXZ = Mathf.Abs(localVelocity.x) > 0.01f || Mathf.Abs(localVelocity.z) > 0.01f;

        if (!hasVelocityOnXZ)
        {
            localVelocity.x = 0f;
            localVelocity.z = 0f;
        }

        // Reconvertit la v�locit� locale modifi�e en espace global
        movement = transform.TransformDirection(localVelocity);
        
        movement *= targetVelocity * maxSpeed;

        Vector3 localVerticalVelocity = Vector3.Project(rb.velocity, transform.up);

        rb.velocity = localVerticalVelocity + movement;
    }

    void GroundCheck()
    {
        Vector3 groundDirection = planete.transform.position - transform.position;
        //ground check
        grounded = Physics.Raycast(transform.position, groundDirection, playerHeight * 0.5f + 0.3f, whatIsGround);
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
        if (context.performed && grounded)
        {
            isSprinting = false;
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            rb.AddForce(transform.up * 5f, ForceMode.VelocityChange);
        }
    }
}
