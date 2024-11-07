using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Controls controls;
    private Vector2 moveInputVector;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode croutchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handeling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    Vector3 moveDirection;

    Rigidbody rb;

    public MouvementState state;
    public enum MouvementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;

        controls.Player.StartCrouch.performed += StartCrouch;
        controls.Player.StopCrouch.performed += StopCrouch;
        controls.Player.Jump.performed += Jump;
        controls.Player.StartSprint.performed += StartSprint;
        controls.Player.StopSprint.performed += StopSprint;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = walkSpeed;
        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        SpeedControl();
        StateHandler();
    }

    private void FixedUpdate()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;
        MovePlayer();
        if(grounded)
        {
            rb.drag = groundDrag;
        }   
        else
        { 
            rb.drag = 0;
        }
    }
    

    private void StateHandler()
    {
        if (grounded)
        {
            if(state != MouvementState.crouching && state != MouvementState.sprinting)
            {
                state = MouvementState.walking;
            }
        }
        else
        {
            //Mode - air
            state = MouvementState.air;
        }       
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * moveInputVector.y + orientation.right * moveInputVector.x;

        //on slope
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(getSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //on Flat Ground
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //in air
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slipe
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //limiting speed on ground or in the air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            if (readyToJump && grounded)
            {
                readyToJump = false;
                exitingSlope = true;

                //reset y velocity
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(grounded && state != MouvementState.crouching)
            {
                // Mode - Sprinting
                state = MouvementState.sprinting;
                moveSpeed = sprintSpeed;
            }
        }
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (state == MouvementState.sprinting)
            {
                // Mode - walking
                state = MouvementState.walking;
                moveSpeed = walkSpeed;
            }
        }
    }

    private void StartCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);


                // Mode - Crouching
                state = MouvementState.crouching;
                moveSpeed = crouchSpeed;
            }
        }
    }
   
    private void StopCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            if (state == MouvementState.crouching)
            {
                state = MouvementState.walking;
                moveSpeed = walkSpeed;
            }
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            //compare the slope angle to out max angle variable
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 getSlopeMoveDirection() 
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
