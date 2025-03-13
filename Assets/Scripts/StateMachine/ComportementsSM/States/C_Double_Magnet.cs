using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Magnet : ComportementState
{
    private GameObject forceFieldObj;
    private float magnetRange;
    private float trueMagnetRange;
    private float magnetForce;
    private float magnetForceOnPlayer;
    private float magnetForceWhenGrab;

    private Color color;

    //private GameObject sonMagnet;

    public C_Double_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.aimantStart,_sm.gameObject);
        // sonMagnet = _sm.GetComponentInChildren<FMODUnity.StudioEventEmitter>().gameObject;

        stateValue = 54;
        leftValue = 27;
        rightValue = 27;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.magnetColor);

        magnetRange = _sm.comportementManager.doubleMagnetData.doubleMagnetRange;
        if (_sm.isPlayer)
        {
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;
        }
        else
        {
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;
        }
        
        magnetForce = _sm.comportementManager.doubleMagnetData.doubleMagnetForce;
        magnetForceOnPlayer = _sm.comportementManager.doubleMagnetData.doubleMagnetForceOnPlayer;
        magnetForceWhenGrab = _sm.comportementManager.doubleMagnetData.doubleMagnetForceWhenGrab;
        
        // set la prefab qui va appliquer la force
        forceFieldObj = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.doubleMagnetData.prefabDoubleMagnetForcefield,_sm.transform.position, Quaternion.identity, _sm.transform);//, _sm.transform => parent mais pose des pb
        forceFieldObj.GetComponent<MagnetForceField>().force = magnetForce;
        forceFieldObj.GetComponent<MagnetForceField>().affectedPlayer = true;
        
        forceFieldObj.GetComponent<MagnetForceField>().magnetFeedbackMaterial.material.SetColor("_Color0", _sm.comportementManager.doubleMagnetData.justePourDiffSimpleMagnet);
        
        forceFieldObj.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        forceFieldObj.GetComponent<GrowToRadius>().atDestroy = false;
    }

    public override void TickLogic()
    {
        base.TickLogic();

        if (_sm.isPlayer)
        {
            forceFieldObj.GetComponent<MagnetForceField>().force = magnetForceOnPlayer;
            forceFieldObj.GetComponent<MagnetForceField>().affectedPlayer = false;

        }
        else if (isGrabbed)
        {
            forceFieldObj.GetComponent<MagnetForceField>().force = magnetForceWhenGrab;
            forceFieldObj.GetComponent<MagnetForceField>().affectedPlayer = false;

        }
        else
        {
            forceFieldObj.GetComponent<MagnetForceField>().force = magnetForce;
            forceFieldObj.GetComponent<MagnetForceField>().affectedPlayer = true;

        }
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
    }

    public override void Exit()
    {
        base.Exit();
        _sm.comportementManager.DestroyObj(forceFieldObj);
        //_sm.comportementManager.DestroyObj(sonMagnet);

    }
}
