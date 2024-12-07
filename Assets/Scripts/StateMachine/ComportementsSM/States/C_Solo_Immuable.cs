using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Immuable : ComportementState
{
    public C_Solo_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 9;
        leftValue = 9;
        rightValue = 0;
        base.Enter();
        Debug.Log("Solo Immuable");
        _sm.rend.material = _sm.immuable;
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
