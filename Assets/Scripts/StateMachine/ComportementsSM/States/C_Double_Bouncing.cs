using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Bouncing : ComportementState
{
    private PhysicMaterial doubleBouncyMaterial;
    private PhysicMaterial basePlayerMaterial;
    private PhysicMaterial basePlayerSlideMaterial;
    public C_Double_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 6;
        leftValue = 3;
        rightValue = 3;
        base.Enter();
        
        doubleBouncyMaterial = _sm.comportementManager.doubleBouncyMaterial;
        
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.bouncingColor);
        if (_sm.isPlayer)
        {
            basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            basePlayerSlideMaterial = _sm.comportementManager.playerSlidingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = doubleBouncyMaterial;
            _sm.comportementManager.playerSlidingCollider.material = doubleBouncyMaterial;
        }
        else
        {
            _sm.objectCollider.material = doubleBouncyMaterial;
        }

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
        if (_sm.isPlayer)
        {
            _sm.comportementManager.playerBouncingCollider.material = basePlayerMaterial;
            _sm.comportementManager.playerSlidingCollider.material = basePlayerSlideMaterial;
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
        }

    }
}
