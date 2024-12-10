using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Immuable : ComportementState
{
    public Vector3 baseVelocity;
    public Vector3 baseAngularVelocity;
    
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

        baseVelocity = _sm.rb.velocity;
        baseAngularVelocity = _sm.rb.angularVelocity;
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
        _sm.rb.velocity = baseVelocity;
        _sm.rb.angularVelocity = baseAngularVelocity;
    }
}
