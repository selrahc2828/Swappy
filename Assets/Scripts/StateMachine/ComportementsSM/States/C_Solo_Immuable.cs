using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class C_Solo_Immuable : ComportementState
{
    private Vector3 _baseVelocity;
    private Vector3 _baseAngularVelocity;

    private EventInstance _immuableSoundInstance;
    
    public C_Solo_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _immuableSoundInstance = FMODEventManager.instance.CreateEventInstance(FMODEventManager.instance.FMODEvents.Immuable);
        FMODEventManager.instance.PlayEventInstance(_immuableSoundInstance);
        isKinematic = true;
        stateValue = 9;
        if (_sm.updateRight)  // Si on veut initialiser pour la main droite
        {
            leftValue = 0;
            rightValue = 9;
        }
        else  // Par défaut, initialisation pour la main gauche
        {
            leftValue = 9;
            rightValue = 0;
        }
        // leftValue = 9;
        // rightValue = 0;
        base.Enter();
        
        ColorShaderOutline(_sm.comportementManager.immuableColor, _sm.comportementManager.noComportementColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Immuable, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        
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
        
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
        FMODEventManager.instance.ReleaseEventInstance(_immuableSoundInstance);
    }

    public override void CollisionStart(Collision other)
    {
        FMODEventManager.instance.SetNamedParamEventInstance(_immuableSoundInstance,"Stinger", 1f);
    }
}
