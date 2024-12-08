using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Immuable_Magnet : ComportementState
{
    public C_Immuable_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 36;
        leftValue = 9;
        rightValue = 27;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.immuableColor, _sm.comportementManager.magnetColor);

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
