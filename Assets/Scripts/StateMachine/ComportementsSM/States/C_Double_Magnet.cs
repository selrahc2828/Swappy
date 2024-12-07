using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Magnet : ComportementState
{
    public C_Double_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 54;
        leftValue = 27;
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
