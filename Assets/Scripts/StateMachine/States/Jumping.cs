using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MouvementState
{
    private PlayerMouvementStateMachine _sm;

    public Jumping(StateMachine stateMachine) : base(stateMachine)
    {
        _sm = ((PlayerMouvementStateMachine)_stateMachine);
    }

    public override void Enter()
    {
        base.Enter();

        readyToJump = true;
    }
}
