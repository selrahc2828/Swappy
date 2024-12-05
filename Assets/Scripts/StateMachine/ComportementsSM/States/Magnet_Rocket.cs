using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet_Rocket : ComportementState
{
    public Magnet_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 108;
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
