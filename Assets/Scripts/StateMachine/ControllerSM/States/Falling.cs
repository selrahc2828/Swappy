using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;

public class Falling : MouvementState
{
    protected StateMachine stateMachine;

    public Falling(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void TickLogic()
    {
        base.TickLogic();
    }
    public override void TickPhysics()
    {
        base.TickPhysics();
        
        if (grounded)
        {
            SoundManager.Instance.PlaySoundLand();
            _sm.ChangeState(PlayerMouvementStateMachine.walkingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
