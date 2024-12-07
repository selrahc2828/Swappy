using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Rocket : ComportementState
{
    public C_Solo_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 81;
        leftValue = 81;
        rightValue = 0;
        base.Enter();
        _sm.rend.material = _sm.rocket;
    }

    public override void TickLogic()
    {
        
    }

    public override void TickPhysics()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
