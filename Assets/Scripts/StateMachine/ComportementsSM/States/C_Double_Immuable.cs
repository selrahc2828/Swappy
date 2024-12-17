using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Immuable : ComportementState
{
    public C_Double_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 18;
        leftValue = 9;
        rightValue = 9;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.immuableColor, _sm.comportementManager.immuableColor);

        if (!_sm.isPlayer)
        {
            _sm.gameObject.tag = "Untagged";
        }
        _sm.rb.isKinematic = true;

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
        if (!_sm.isPlayer)
        {
            _sm.gameObject.tag = "Movable";
        }
        _sm.rb.isKinematic = false;
    }
}
