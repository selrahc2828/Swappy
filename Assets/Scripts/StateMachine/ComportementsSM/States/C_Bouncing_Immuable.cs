using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Bouncing_Immuable : ComportementState
{
    public C_Bouncing_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 12;
        leftValue = 3;
        rightValue = 9;
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
