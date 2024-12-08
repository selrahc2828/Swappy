using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Bouncing : ComportementState
{
    public C_Solo_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 3;
        leftValue = 3;
        rightValue = 0;
        base.Enter();
        _sm.rend.material = _sm.bounce;
    }

    public override void TickLogic()
    {
        base.TickLogic();
    }

    public override void TickPhysics()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
