using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projecting : MouvementState
{
    protected StateMachine stateMachine;

    public Projecting(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void TickLogic()
    {
        throw new System.NotImplementedException();
    }
    public override void TickPhysics()
    {
        throw new System.NotImplementedException();
    }
    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
