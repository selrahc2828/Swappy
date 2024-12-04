using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MouvementState
{
    protected StateMachine stateMachine;
    public float jumpForce;
    public float jumpCooldown;

    public Jumping(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        jumpForce = _sm.gameManager.jumpForce;
        jumpCooldown = _sm.gameManager.jumpCooldown;

        readyToJump = false;
        exitingSlope = true;

        //reset y velocity
        _sm.rb.velocity = new Vector3(_sm.rb.velocity.x, 0f, _sm.rb.velocity.z);
        _sm.rb.AddForce(_sm.transform.up * jumpForce, ForceMode.Impulse);
        Debug.Log(jumpForce);

        jumped = true;
        jumpTimer = jumpCooldown;
    }
    public override void TickLogic()
    {
        base.TickLogic();
    }
    public override void TickPhysics()
    {
        base.TickPhysics();
        if(_sm.rb.velocity.y < 0f)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.fallingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        readyToJump = true;
    }
}
