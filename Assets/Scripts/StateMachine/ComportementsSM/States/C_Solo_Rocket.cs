using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Rocket : ComportementState
{
    public float rocketForce = 20;

    public C_Solo_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 81;
        leftValue = 81;
        rightValue = 0;
        base.Enter();
        
        rocketForce = _sm.comportementManager.rocketForce;
        
        // _sm.rend.material = _sm.rocket;
        ColorShaderOutline(_sm.comportementManager.rocketColor, _sm.comportementManager.noComportementColor);

    }

    public override void TickLogic()
    {
        base.TickLogic();
    }

    public override void TickPhysics()
    {
        if (_sm.rb)
        {
            _sm.rb.AddForce(Vector3.up * rocketForce, ForceMode.Force);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
