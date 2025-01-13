using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Impulse_Rocket : ComportementState
{
   
    
    public C_Impulse_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 82;
        leftValue = 1;
        rightValue = 81;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.rocketColor);
        
        

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
