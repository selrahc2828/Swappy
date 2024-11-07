using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouvementState : State
{
    private Controls controls;
    private Vector2 moveInputVector;

    [Header("Movement")]
    protected float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    protected bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    protected float startYScale;

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

    private PlayerMouvementStateMachine _sm;

    public MouvementState(StateMachine stateMachine) : base(stateMachine)
    {
        _sm = ((PlayerMouvementStateMachine)_stateMachine);
    }

    public override void Enter()
    {
        controls = GameManager.controls;

        _sm.rb.freezeRotation = true;

    }
    public override void TickLogic()
    {
        //ground check
        grounded = Physics.Raycast(_sm.rb.transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        SpeedControl();
    }
    public override void TickPhisics()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;
        MovePlayer();
        if (grounded)
        {
            _sm.rb.drag = groundDrag;
        }
        else
        {
            _sm.rb.drag = 0;
        }
    }

    public override void Exit()
    {
    }


    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * moveInputVector.y + orientation.right * moveInputVector.x;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            _sm.rb.AddForce(getSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (_sm.rb.velocity.y > 0)
            {
                _sm.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //on Flat Ground
        else if (grounded)
        {
            _sm.rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //in air
        else
        {
            _sm.rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //turn gravity off while on slope
        _sm.rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slipe
        if (OnSlope() && !exitingSlope)
        {
            if (_sm.rb.velocity.magnitude > moveSpeed)
            {
                _sm.rb.velocity = _sm.rb.velocity.normalized * moveSpeed;
            }
        }
        //limiting speed on ground or in the air
        else
        {
            Vector3 flatVel = new Vector3(_sm.rb.velocity.x, 0f, _sm.rb.velocity.z);

            //limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                _sm.rb.velocity = new Vector3(limitedVel.x, _sm.rb.velocity.y, limitedVel.z);
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(_sm.rb.transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
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
