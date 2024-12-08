using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Impulse : ComportementState
{
    public C_Solo_Impulse(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 1;
        leftValue = 1;
        rightValue = 0;
        base.Enter();
        Debug.Log("Solo impulse");
        _sm.rend.material = _sm.impulse;
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
