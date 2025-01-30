using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class Falling : MouvementState
{
    protected StateMachine stateMachine;
    private FMOD.Studio.EventInstance fallSound;

    public Falling(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter(); 
        fallSound =SoundManager.Instance.CreateSoundFalling();
        SoundManager.Instance.PlaySound(fallSound);
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
        SoundManager.Instance.StopSound(fallSound);
        base.Exit();
    }
}
