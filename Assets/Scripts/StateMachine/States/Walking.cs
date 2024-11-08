using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : MouvementState
{
    protected StateMachine stateMachine;

    public Walking(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        controls.Player.StartCrouch.performed += StartCrouch;
        controls.Player.Jump.performed += Jump;
        controls.Player.StartSprint.performed += StartSprint;


        moveSpeed = walkSpeed;
    }
    public override void TickLogic()
    {
        base.TickLogic();
    }
    public override void TickPhysics()
    {
        base.TickPhysics();

        if (!grounded)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.fallingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }


    private void StartCrouch(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.crouchingState);
            
        }
    }
    private void StartSprint(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.sprintingState);
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && readyToJump && grounded)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.jumpingState);
        }
    }
}
