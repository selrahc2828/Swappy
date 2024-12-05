using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse_Magnet : ComportementState
{
    public Impulse_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 28;
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
