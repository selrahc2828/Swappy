using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immuable_Magnet : ComportementState
{
    public Immuable_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 36;
        leftValue = 9;
        rightValue = 27;
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
