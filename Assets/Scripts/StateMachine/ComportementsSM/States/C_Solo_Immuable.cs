using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Immuable : ComportementState
{
    private Vector3 _baseVelocity;
    private Vector3 _baseAngularVelocity;
    
    public C_Solo_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = true;
        stateValue = 9;
        leftValue = 9;
        rightValue = 0;
        base.Enter();
        
        ColorShaderOutline(_sm.comportementManager.immuableColor, _sm.comportementManager.noComportementColor);

        _baseVelocity = _sm.rb.velocity;
        _baseAngularVelocity = _sm.rb.angularVelocity;
        _sm.rb.isKinematic = true;
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
        _sm.rb.isKinematic = false;
        _sm.rb.velocity = _baseVelocity;
        _sm.rb.angularVelocity = _baseAngularVelocity;
    }

    public override void CollisionStart(Collision other)
    {
        //SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.immuableHit, _sm.gameObject);
    }
}
