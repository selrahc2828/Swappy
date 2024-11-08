using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MouvementState
{
    protected StateMachine stateMachine;

    public Falling(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void TickLogic()
    {
        base.TickLogic();

        if (grounded)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.walkingState);
        }
    }
    public override void TickPhysics()
    {
        base.TickPhysics();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
