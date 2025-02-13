using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Impulse_Magnet : ComportementState
{
    private float zoneImpulseRange;
    private float trueZoneImpulseRange;

    private float zoneImpulseForce;
    
    private GameObject feedback;
    private GameObject zone;

    private GameObject prefabSonMagnet;
    
    public C_Impulse_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.aimantStart,_sm.gameObject);
        prefabSonMagnet = _sm.GetComponentInChildren<FMODUnity.StudioEventEmitter>().gameObject;
        SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.repulseBoom,_sm.gameObject);
        stateValue = 28;
        leftValue = 1;
        rightValue = 27;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.magnetColor);
        
        zoneImpulseRange = _sm.comportementManager.impulseMagnetData.zoneImpulseRange;

        if (_sm.isPlayer)
        {
            trueZoneImpulseRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + zoneImpulseRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            trueZoneImpulseRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + zoneImpulseRange;
        }
        
        zoneImpulseForce = _sm.comportementManager.impulseMagnetData.zoneImpulseForce;
        feedback = _sm.comportementManager.impulseMagnetData.prefabImpulseMagnet;
        
        //zone qui repousse constamment
        ConstantRepulse();

    }

    public override void TickLogic()
    {
        base.TickLogic();

        if (zone != null)
        {
            zone.GetComponent<ImpulseMagnetZone>().followTransform = _sm.transform;
            zone.GetComponent<ImpulseMagnetZone>().isGrabbed = isGrabbed;

        }


        // comme impusle ou faire collider stay
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
    }

    public override void Exit()
    {
        base.Exit();
        _sm.comportementManager.DestroyObj(prefabSonMagnet);
        if (zone != null)
        {
            _sm.comportementManager.DestroyObj(zone);
        }
    }

    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sm.transform.position, trueZoneImpulseRange);  
    }  
    
    public void ConstantRepulse()
    {
        if (feedback)
        {
            zone = _sm.comportementManager.InstantiateFeedback(feedback, _sm.transform.position, Quaternion.identity);//, _sm.transform //parent
            zone.transform.localScale = new Vector3(trueZoneImpulseRange * 2, trueZoneImpulseRange * 2, trueZoneImpulseRange * 2); //* 2 pour appliquer le diametre pas le rayon
            zone.GetComponent<ImpulseMagnetZone>().force = zoneImpulseForce;
            zone.GetComponent<ImpulseMagnetZone>().range = trueZoneImpulseRange;
        }
    }
    
}
