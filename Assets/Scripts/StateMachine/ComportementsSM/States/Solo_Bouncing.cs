using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solo_Bouncing : ComportementState
{
    public Solo_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 3;
        leftValue = 3;
        rightValue = 0;
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
