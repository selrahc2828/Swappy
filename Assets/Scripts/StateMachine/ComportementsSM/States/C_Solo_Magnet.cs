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
        stateValue = 27;
        leftValue = 27;
        rightValue = 0;
        base.Enter();
        _sm.rend.material = _sm.magnet;
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
