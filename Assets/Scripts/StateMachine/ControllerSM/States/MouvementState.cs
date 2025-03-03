using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouvementState : State
{
    protected Controls controls;
    protected Vector2 moveInputVector;

    [Header("Movement")]
    protected float moveSpeed = 7;
    public float walkSpeed;
    public float sprintSpeed;
    private float groundDrag;

    [Header("Jumping")]
    public float jumpTimer;
    public float airMultiplier;
    protected bool readyToJump = true;
    protected bool jumped = false;

    [Header("Ground Check")]
    private float playerHeight;
    private LayerMask whatIsGround;
    protected bool grounded;

    [Header("Slope Handeling")]
    private float maxSlopeAngle;
    private RaycastHit slopeHit;
    protected bool exitingSlope;

    private Vector3 gravity;
    private Vector3 slopeGravity;

    protected Vector3 moveDirection;
    private float _footStepsSoundValue;
    private float _footstepsMaxValue;
    private float _footstepsSlowValue = 350f;
    private float _footstepsBaseValue = 200f;

    protected PlayerMouvementStateMachine _sm;

    public MouvementState(StateMachine stateMachine) : base(stateMachine)
    {
        _sm = ((PlayerMouvementStateMachine)_stateMachine);
    }

    public override void Enter()
    {
        controls = GameManager.controls;

        walkSpeed = _sm.gameManager.walkSpeed;
        sprintSpeed = _sm.gameManager.sprintSpeed;
        groundDrag = _sm.gameManager.groundDrag;
        airMultiplier = _sm.gameManager.airMultiplier;
        playerHeight = _sm.gameManager.playerHeight;
        whatIsGround = _sm.gameManager.whatIsGround;
        maxSlopeAngle = _sm.gameManager.maxSlopeAngle;

        _sm.rb.freezeRotation = true;

        gravity = Physics.gravity;
        
        controls.Player.Projection.performed += Projection;
    }
    public override void TickLogic()
    {
        //Debug.Log(_sm.currentState.ToString());
        
        JumpCooldown();
        SpeedControl();
    }
    public override void TickPhysics()
    {
        //ground check
        grounded = Physics.Raycast(_sm.rb.transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (_sm.currentState != PlayerMouvementStateMachine.projectingState)
        {
            moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;
        }
        
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
        controls.Player.Projection.performed -= Projection;
    }

    public override void CollisionStart(Collision other)
    {
    }

    public override void CollisionDuring(Collision other)
    {
    }

    public override void CollisionEnd(Collision other)
    {
    }

    public override void DisplayGizmos()
    {
    }
    
    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = _sm.gameManager.orientation.forward * moveInputVector.y + _sm.gameManager.orientation.right * moveInputVector.x;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            if (GameManager.Instance.slowMotion)
            {
                _footstepsMaxValue = _footstepsSlowValue;
            }
            else
            {
                _footstepsMaxValue = _footstepsBaseValue;
            }
            _footStepsSoundValue += 1 * _sm.rb.velocity.magnitude;
            if (_footStepsSoundValue>_footstepsMaxValue )
            {
                SoundManager.Instance.PlaySoundFootstep();
                _footStepsSoundValue = 0;
            }
            _sm.rb.AddForce(getSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);
            slopeGravity = Vector3.Project(gravity, slopeHit.normal);
            _sm.rb.AddForce(slopeGravity * 5, ForceMode.Acceleration);
            if (_sm.rb.velocity.y > 0)
            {
                _sm.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //on Flat Ground
        else if (grounded)
        {
            if (GameManager.Instance.slowMotion)
            {
                _footstepsMaxValue = _footstepsSlowValue;
            }
            else
            {
                _footstepsMaxValue = _footstepsBaseValue;
            }
            _footStepsSoundValue += 1 * _sm.rb.velocity.magnitude;
            if (_footStepsSoundValue > _footstepsMaxValue)
            {
                SoundManager.Instance.PlaySoundFootstep();
                _footStepsSoundValue = 0;
            }
            _sm.rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        //in air
        else
        {
            _sm.rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
            //_sm.rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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
    private void JumpCooldown()
    {
        if(jumped)
        {
            jumpTimer -= Time.deltaTime;
            if(jumpTimer < 0)
            {
                ResetJump();
                jumped = false;
            }
        }
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }
    void Projection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_sm.gameManager.canProjectWhenCarryingObject && _sm.gameManager.grabScript.isCarrying)// empeche projection sur objet grab
            {
                return;
            }
            if (_sm.currentState == PlayerMouvementStateMachine.projectingState)//sort de projection si on l'est déjà
            {
                _sm.ChangeState(PlayerMouvementStateMachine.walkingState);
            }
            else
            {
                _sm.ChangeState(PlayerMouvementStateMachine.projectingState);
            }
        }
    }
}
