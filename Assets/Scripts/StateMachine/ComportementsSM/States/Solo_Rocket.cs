using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solo_Rocket : ComportementState
{
    public Solo_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 81;
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
