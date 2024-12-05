using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Double_Impulse : ComportementState
{
    public Double_Impulse(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 2;
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
