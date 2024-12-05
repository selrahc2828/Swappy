using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Double_Rocket : ComportementState
{
    public Double_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 162;
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
