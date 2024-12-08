using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Magnet : ComportementState
{
    public C_Solo_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        baseStateValue = 27;
        stateValue = 27;
        leftValue = 27;
        rightValue = 0;
        base.Enter();
        _sm.rend.material = _sm.magnet;
        ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.noComportementColor);

    }

    public override void TickLogic()
    {
        if (baseStateValue != stateValue)
        {
            CalculateNewtState(stateValue);
        }

    }

    public override void TickPhysics()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
