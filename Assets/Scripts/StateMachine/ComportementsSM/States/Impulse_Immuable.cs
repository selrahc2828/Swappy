using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse_Immuable : ComportementState
{
    public Impulse_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 10;
        base.Enter();
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
