using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class C_Solo_Impulse : ComportementState
{
    private float repulserTime = 5f;
    private float repulserFirstTime;
    private float repulserTimer;
    private float repulserRange;
    private float trueRepulserRange;
    private float repulserForce;
    private bool destroyOnUse = false;
    [Tooltip("Si Rigidbody sur lui")]
    private bool applyOnMe = false;
    private GameObject feedback;
    private bool explodingSoonSignalSended;
    
    
    public C_Solo_Impulse(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 1;
        leftValue = 1;
        rightValue = 0;
        base.Enter();
        
        repulserTime = _sm.comportementManager.impulseData.impulseTime;
        repulserFirstTime = _sm.comportementManager.impulseData.impulseFirstTime;
        repulserTimer = 0f;
        repulserRange = _sm.comportementManager.impulseData.impulseRange;

        if (_sm.isPlayer)
        {
            trueRepulserRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + repulserRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            trueRepulserRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + repulserRange;
            ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.noComportementColor);
        }
        // pb si obj n'a pas de collider direct (ax Player)
        repulserForce = _sm.comportementManager.impulseData.impulseForce;
        destroyOnUse = _sm.comportementManager.impulseData.destroyOnUse;
        applyOnMe= _sm.comportementManager.impulseData.applyOnMe;
        feedback = _sm.comportementManager.impulseData.impulseFeedback;
        explodingSoonSignalSended = false;

        GlobalEventManager.Instance.Explosion(GetGameObject(), repulserFirstTime, true);
    }

    public override void TickLogic()
    {
        base.TickLogic();
        repulserTimer += Time.deltaTime;
        if (repulserTimer >= repulserTime)
        {
            
            Repulse();
            repulserTimer = 0;
            explodingSoonSignalSended = false;
        }
        if(repulserTimer > (repulserTime-1.5f) && explodingSoonSignalSended == false)
        {
            explodingSoonSignalSended=true;
            GlobalEventManager.Instance.JustBeforeExplosion(GetGameObject());
        }
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sm.transform.position, trueRepulserRange);  
    }    
 
    public void Repulse()
    {
        GlobalEventManager.Instance.Explosion(GetGameObject(), repulserTime, false);
        GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
        if (feedback)
        {
            GameObject shockWave = _sm.comportementManager.InstantiateFeedback(feedback, _sm.transform.position, Quaternion.identity);
            shockWave.GetComponent<GrowToRadius>().targetRadius = trueRepulserRange;
        }
        
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueRepulserRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.gameObject.CompareTag("Player"))
                {
                    if (!objectInRange.gameObject.GetComponentInParent<Rigidbody>())
                    {
                        return;
                    }
                    //collider et rigid body pas au même endroit pour lui
                    GameObject objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>().gameObject;
                    
                    // pb pour appliquer la force à cause du drag sur le rigidbody
                    ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected,repulserForce);
                    
                    // player relache l'objet repulse
                    if (isGrabbed) //juste isGrabbed ? objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject
                    {
                        objectAffected.GetComponent<GrabObject>().Release(true);
                    }
                }

                BreakableObject breakableScript = null;
                try
                {
                    breakableScript = objectInRange.GetComponent<AoeCondition>().CheckImpulseAoeCondition();
                }
                catch {}
                finally
                {
                    if (breakableScript != null)
                    {
                        foreach (Rigidbody rb in breakableScript.ShatterObject())
                        {
                            ApplyForce(rb, rb.gameObject, repulserForce);
                        }
                    }
                }

                if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, repulserForce);
                }  
            }
        }

        if (destroyOnUse)
        {
            _sm.comportementManager.DestroyObj(_sm.gameObject);
        }
    }

    public void ApplyForce(Rigidbody rbObj,GameObject objToApply, float force)
    {
        if (!applyOnMe && objToApply == _sm.gameObject)
        {
            // si rigid body sur objet, on applique pas la force sur lui pour le lancer par exemple
            return;
        }
        Vector3 direction = (objToApply.transform.position - _sm.transform.position).normalized;
        rbObj.AddForce( direction * force, ForceMode.Impulse);
    }
}
