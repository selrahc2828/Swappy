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
        
        doubleBouncyMaterial = _sm.comportementManager.doubleBounceData.doubleBouncyMaterial;
        
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.bouncingColor);
        if (_sm.isPlayer)
        {
            basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = doubleBouncyMaterial;
        }
        else
        {
            _sm.objectCollider.material = doubleBouncyMaterial;
        }

        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);
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
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);

        if (_sm.isPlayer)
        {
            _sm.comportementManager.playerBouncingCollider.material = basePlayerMaterial;
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
        }

    }

    public override void CollisionStart(Collision other)
    {
        
    }
}
