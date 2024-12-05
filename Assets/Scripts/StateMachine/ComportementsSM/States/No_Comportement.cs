using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class No_Comportement : ComportementState
{
    public No_Comportement(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateValue = 0;
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
