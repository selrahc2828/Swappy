using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MouvementState
{

    private PlayerMouvementStateMachine _sm;

    public Crouching(StateMachine stateMachine) : base(stateMachine)
    {
        _sm = ((PlayerMouvementStateMachine)_stateMachine);
    }

    public override void Enter()
    {
        base.Enter();

        startYScale = _sm.transform.localScale.y;
    }
}
