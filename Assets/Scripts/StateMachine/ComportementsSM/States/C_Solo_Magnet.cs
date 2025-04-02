using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;

public class C_Solo_Magnet : ComportementState
{
    private float magnetRange;
    private float trueMagnetRange;
    private float magnetForce;
    private bool magnetGradiantForce;
    
    private EventInstance _magnetSoundInstance;

    
    public C_Solo_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _magnetSoundInstance = FMODEventManager.instance.CreateEventInstance(FMODEventManager.instance.FMODEvents.Magnet);
        
        isKinematic = false;
        stateValue = 27;
        if (_sm.updateRight)  // Si on veut initialiser pour la main droite
        {
            leftValue = 0;
            rightValue = 27;
        }
        else  // Par défaut, initialisation pour la main gauche
        {
            leftValue = 27;
            rightValue = 0;
        }
        // leftValue = 27;
        // rightValue = 0;
        base.Enter();

        magnetRange = _sm.comportementManager.magnetData.magnetRange;
        if (_sm.isPlayer)
        {
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;
        }
        else
        {
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;
        }
        magnetForce = _sm.comportementManager.magnetData.magnetForce;
        magnetGradiantForce = _sm.comportementManager.magnetData.magnetGradiantForce;
        
        // _sm.rend.material = _sm.magnet;
        ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.noComportementColor);

        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Magnet, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Left.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        feedBack_GO_Left.GetComponent<GrowToRadius>().atDestroy = false;
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
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
        FMODEventManager.instance.ReleaseEventInstance(_magnetSoundInstance);
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
