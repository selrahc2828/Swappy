using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_No_Comportement : ComportementState
{
    public C_No_Comportement(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateValue = 0;
        leftValue = 0;
        rightValue = 0;
        base.Enter();
        _sm.rend.material = _sm.rien;
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
