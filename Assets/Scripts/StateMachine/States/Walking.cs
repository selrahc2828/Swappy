using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MouvementState
{
    protected StateMachine stateMachine;

    public Walking(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        moveSpeed = walkSpeed;
    }
    public override void TickLogic()
    {
        throw new System.NotImplementedException();
    }
    public override void TickPhisics()
    {
        throw new System.NotImplementedException();
    }
    public override void Exit()
    {
        throw new System.NotImplementedException();
    }


}
