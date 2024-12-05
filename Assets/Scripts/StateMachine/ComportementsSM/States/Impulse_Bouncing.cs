using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse_Bouncing : ComportementState
{
    public Impulse_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 4;
        leftValue = 1;
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
