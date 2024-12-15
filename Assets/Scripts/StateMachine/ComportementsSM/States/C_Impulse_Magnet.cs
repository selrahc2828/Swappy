using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Impulse_Magnet : ComportementState
{
    public float zoneImpulseRange;
    public float trueZoneImpulseRange;

    public float zoneImpulseForce;
    
    public GameObject feedback;
    public GameObject zone;
    
    public C_Impulse_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 28;
        leftValue = 1;
        rightValue = 27;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.magnetColor);
        
        zoneImpulseRange = _sm.comportementManager.zoneImpulseRange;

        if (_sm.isPlayer)
        {
            trueZoneImpulseRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + zoneImpulseRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            trueZoneImpulseRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + zoneImpulseRange;
        }
        
        zoneImpulseForce = _sm.comportementManager.zoneImpulseForce;
        feedback = _sm.comportementManager.feedbackImpulseMagnet;
        
        //zone qui repousse constamment
        ConstantRepulse();

    }

    public override void TickLogic()
    {
        base.TickLogic();

        if (zone != null)
        {
            zone.GetComponent<ImpulseMagnetZone>().followTransform = _sm.transform;
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
        _sm.comportementManager.DestroyObj(zone);
        
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
            Debug.LogWarning($"create");
            zone = _sm.comportementManager.InstantiateFeedback(feedback, _sm.transform.position, Quaternion.identity);//, _sm.transform //parent
            zone.transform.localScale = new Vector3(trueZoneImpulseRange * 2, trueZoneImpulseRange * 2, trueZoneImpulseRange * 2); //* 2 pour appliquer le diametre pas le rayon
            zone.GetComponent<ImpulseMagnetZone>().force = zoneImpulseForce;
            zone.GetComponent<ImpulseMagnetZone>().range = trueZoneImpulseRange;
        }
        //
        // Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueRepulserRange);
        // if (objectsInRange.Length > 0)
        // {
        //     foreach (Collider objectInRange in objectsInRange)
        //     {
        //         if (objectInRange.gameObject.tag == "Player")
        //         {
        //             if (!objectInRange.gameObject.GetComponentInParent<Rigidbody>())
        //             {
        //                 return;
        //             }
        //             //collider et rigid body pas au même endroit pour lui
        //             GameObject objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>().gameObject;
        //             
        //             // pb pour appliquer la force à cause du drag sur le rigidbody
        //             ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected,repulserForce);
        //             
        //             // player relache l'objet repulse
        //             if (isGrabbed) //juste isGrabbed ? objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject
        //             {
        //                 objectAffected.GetComponent<GrabObject>().Drop(true);
        //             }
        //         }
        //         else if (objectInRange.GetComponent<Rigidbody>() != null)
        //         {
        //             ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, repulserForce);
        //         }  
        //     }
        // }
        
    }
    
}
