using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Bouncing : ComportementState
{
    public C_Double_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 6;
        leftValue = 3;
        rightValue = 3;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.bouncingColor);

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
