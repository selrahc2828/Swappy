using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Bouncing_Rocket : ComportementState
{
    public C_Bouncing_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 84;
        leftValue = 3;
        rightValue = 81;
        base.Enter();
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
}
