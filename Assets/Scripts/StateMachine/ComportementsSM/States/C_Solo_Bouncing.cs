using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;


public class C_Solo_Bouncing : ComportementState
{
    private PhysicMaterial _bouncyMaterial;
    private PhysicMaterial _basePlayerMaterial;
    private PhysicMaterial _basePlayerSlideMaterial;
    
    private EventInstance _bounceSoundInstane;

    public C_Solo_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _bounceSoundInstane = FMODEventManager.instance.CreateEventInstance(FMODEventManager.instance.FMODEvents.Bounce);
        FMODEventManager.instance.PlayEventInstance(_bounceSoundInstane);
        isKinematic = false;
        stateValue = 3;
        if (_sm.updateRight)  // Si on veut initialiser pour la main droite
        {
            leftValue = 0;
            rightValue = 3;
        }
        else  // Par défaut, initialisation pour la main gauche
        {
            leftValue = 3;
            rightValue = 0;
        }
        // leftValue = 3;
        // rightValue = 0;
        base.Enter();

        _bouncyMaterial = _sm.comportementManager.bounceData.bouncyMaterial;
        // _sm.rend.material = _sm.bounce;
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.noComportementColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        
        if (_sm.isPlayer)
        {
            _basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = _bouncyMaterial;
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
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
        }
        
        FMODEventManager.instance.ReleaseEventInstance(_bounceSoundInstane);

    }

    public override void CollisionStart(Collision other)
    {
        FMODEventManager.instance.SetNamedParamEventInstance(_bounceSoundInstane, "Stinger", 1f);
    }
}
