using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Impulse : ComportementState
{
    public float repulserTime = 5f;
    public float repulserTimer;
    public float repulserRange;
    public float trueRepulserRange;
    public float repulserForce;
    public bool destroyOnUse = false;
    public bool impulseGradiantForce = false;
    [Tooltip("Si Rigidbody sur lui")]
    public bool applyOnMe = false;
    public GameObject feedback;
    
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
        
        repulserTime = _sm.comportementManager.repulserTime;
        repulserTimer = _sm.comportementManager.repulserTimer;
        repulserRange = _sm.comportementManager.repulserRange;

        // trueRepulserRange = repulserRange;
        if (_sm.isPlayer)
        {
            trueRepulserRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + repulserRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            trueRepulserRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + repulserRange;
        }
        // pb si obj n'a pas de collider direct (ax Player)
        repulserForce = _sm.comportementManager.repulserForce;
        destroyOnUse = _sm.comportementManager.destroyOnUse;
        impulseGradiantForce = _sm.comportementManager.impulseGradiantForce;
        applyOnMe= _sm.comportementManager.applyOnMe;
        feedback = _sm.comportementManager.impulseFeedback;
        
        // Debug.Log("Solo impulse enter");
        //_sm.rend.material = _sm.impulse;
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.noComportementColor);
    }

    public override void TickLogic()
    {
        base.TickLogic();
        repulserTimer += Time.deltaTime;
        if (repulserTimer >= repulserTime)
        {
            SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.repulseBoom, _sm.gameObject);
            Repulse();
            repulserTimer = 0;
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
                if (objectInRange.gameObject.tag == "Player")
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
                        objectAffected.GetComponent<GrabObject>().Drop(true);
                    }
                }
                else if (objectInRange.GetComponent<Rigidbody>() != null)
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

        if (impulseGradiantForce)
        {
            rbObj.AddExplosionForce(force, _sm.transform.position, trueRepulserRange,1f,ForceMode.Impulse);
        }
        else
        {
            Vector3 direction = (objToApply.transform.position - _sm.transform.position).normalized;
            rbObj.AddForce( direction * force, ForceMode.Impulse);
        }
    }
}
