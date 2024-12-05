using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solo_Magnet : ComportementState
{
    public Solo_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 27;
        leftValue = 27;
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
