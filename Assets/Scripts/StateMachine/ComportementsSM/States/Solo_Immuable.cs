using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solo_Immuable : ComportementState
{
    public Solo_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 9;
        leftValue = 9;
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
