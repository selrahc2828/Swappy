using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Double_Bouncing : ComportementState
{
    public Double_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 6;
        leftValue = 3;
        rightValue = 3;
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
