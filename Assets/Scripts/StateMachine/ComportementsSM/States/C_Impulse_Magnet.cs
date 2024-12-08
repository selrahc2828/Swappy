using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Impulse_Magnet : ComportementState
{
    public C_Impulse_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 28;
        leftValue = 1;
        rightValue = 27;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.magnetColor);

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
