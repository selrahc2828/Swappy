using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Impulse : ComportementState
{
    public C_Double_Impulse(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 2;
        leftValue = 1;
        rightValue = 1;
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
