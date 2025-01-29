using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_No_Comportement : ComportementState
{
    public C_No_Comportement(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 0;
        leftValue = 0;
        rightValue = 0;
        base.Enter();
        // _sm.rend.material = _sm.rien;
        ColorShaderOutline(_sm.comportementManager.noComportementColor, _sm.comportementManager.noComportementColor);
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

    public void CollisionStart()
    {
        SoundManager.Instance.PlaySoundCollision(_sm.gameObject);
    }
}
