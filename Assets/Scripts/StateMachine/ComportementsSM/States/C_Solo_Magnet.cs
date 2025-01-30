using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Solo_Magnet : ComportementState
{
    private float magnetRange;
    private float trueMagnetRange;
    private float magnetForce;
    private bool magnetGradiantForce;
    private GameObject SonDeCon;
    
    public C_Solo_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.aimantStart,_sm.gameObject);
        SonDeCon = _sm.GetComponentInChildren<FMODUnity.StudioEventEmitter>().gameObject;
        
        isKinematic = false;
        stateValue = 27;
        leftValue = 27;
        rightValue = 0;
        base.Enter();

        magnetRange = _sm.comportementManager.magnetRange;
        if (_sm.isPlayer)
        {
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;
        }
        else
        {
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;
        }
        magnetForce = _sm.comportementManager.magnetForce;
        magnetGradiantForce = _sm.comportementManager.magnetGradiantForce;
        
        // _sm.rend.material = _sm.magnet;
        ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.noComportementColor);
    }

    public override void TickLogic()
    {
        base.TickLogic();

        Attract();//même comportement sur player et sur objet
    }

    public override void TickPhysics()
    {
        base.TickPhysics();

    }

    public override void Exit()
    {
        
        base.Exit();
        _sm.comportementManager.DestroyObj(SonDeCon);

    }

    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sm.transform.position, trueMagnetRange);   
    }

    public void Attract()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueMagnetRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (!objectInRange.gameObject.CompareTag("Player") && objectInRange.gameObject != _sm.gameObject) // applique pas sur player et lui même
                {
                    if (objectInRange.GetComponent<Rigidbody>() != null)
                    {
                        ApplyForce(magnetGradiantForce, objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, magnetForce);
                        
                    }
                }
            }
        }
    }
    
    public void ApplyForce(bool isGradient, Rigidbody rbObj,GameObject objToApply, float force)
    {
        
        if (isGradient)
        {
            objToApply.GetComponent<Rigidbody>().AddExplosionForce(-force, _sm.transform.position, trueMagnetRange);
        }
        else
        {

            Vector3 dir = (_sm.transform.position - objToApply.transform.position).normalized; // obj vers magnet
            rbObj.AddForce(dir * force, ForceMode.Force);
        }
    }
}
