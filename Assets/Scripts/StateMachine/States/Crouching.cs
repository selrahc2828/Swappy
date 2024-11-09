using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crouching : MouvementState
{
    protected StateMachine stateMachine;
    public float crouchSpeed;
    public float crouchYScale;
    protected float startYScale;

    public Crouching(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        controls.Player.StopCrouch.performed += StopCrouch;
        controls.Player.Jump.performed += Jump;
        controls.Player.StartSprint.performed += StartSprint;

        crouchSpeed = _sm.gameManager.crouchSpeed;
        crouchYScale = _sm.gameManager.crouchYScale;

        startYScale = _sm.transform.localScale.y;

        _sm.transform.localScale = new Vector3(_sm.transform.localScale.x, crouchYScale, _sm.transform.localScale.z);
        _sm.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        moveSpeed = crouchSpeed;

    }
    public override void TickLogic()
    {
        base.TickLogic();
    }
    public override void TickPhysics()
    {
        base.TickPhysics();
    }
    public override void Exit()
    {
        base.Exit();
        _sm.transform.localScale = new Vector3(_sm.transform.localScale.x, startYScale, _sm.transform.localScale.z);
        Debug.Log(startYScale);
        controls.Player.StartCrouch.performed -= StopCrouch;
        controls.Player.Jump.performed -= Jump;
        controls.Player.StartSprint.performed -= StartSprint;
    }

    private void StopCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                _sm.ChangeState(PlayerMouvementStateMachine.walkingState);
            }
            else
            {
                _sm.ChangeState(PlayerMouvementStateMachine.fallingState);
            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (readyToJump && grounded)
            {
                _sm.ChangeState(PlayerMouvementStateMachine.jumpingState);
            }
        }
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.sprintingState);
        }
    }
}
