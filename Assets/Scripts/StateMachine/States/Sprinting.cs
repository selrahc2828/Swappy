using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sprinting : MouvementState
{
    protected StateMachine stateMachine;

    public Sprinting(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        controls.Player.StartCrouch.performed += StartCrouch;
        controls.Player.StopSprint.performed += StopSprint;
        controls.Player.Jump.performed += Jump;


        moveSpeed = sprintSpeed;
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
    }

    private void StartCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                _sm.ChangeState(PlayerMouvementStateMachine.crouchingState);
            }
        }
    }
    private void StopSprint(InputAction.CallbackContext context)
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
        if (context.performed && readyToJump && grounded)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.sprintingState);
            
        }
    }

}
