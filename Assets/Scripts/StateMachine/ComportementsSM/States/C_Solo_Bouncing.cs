using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class C_Solo_Bouncing : ComportementState
{
    private PhysicMaterial _bouncyMaterial;
    private PhysicMaterial _basePlayerMaterial;
    private PhysicMaterial _basePlayerSlideMaterial;

    public C_Solo_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 3;
        leftValue = 3;
        rightValue = 0;
        base.Enter();

        _bouncyMaterial = _sm.comportementManager.bounceData.bouncyMaterial;
        // _sm.rend.material = _sm.bounce;
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.noComportementColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        
        if (_sm.isPlayer)
        {
            _basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _basePlayerSlideMaterial = _sm.comportementManager.playerSlidingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = _bouncyMaterial;
            _sm.comportementManager.playerSlidingCollider.material = _bouncyMaterial;
        }
        else
        {
            _sm.GetComponent<Collider>().material = _bouncyMaterial;
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
        
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
        
        if (_sm.isPlayer)
        {
            _sm.comportementManager.playerBouncingCollider.material = _basePlayerMaterial;
            _sm.comportementManager.playerSlidingCollider.material = _basePlayerSlideMaterial;
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
        }

    }

    public override void CollisionStart(Collision other)
    {
        FMODEventManager.instance.PlayOneShot(FMODEventManager.instance.FMODEvents.BounceHit,other.GetContact(0).point);
    }
}
