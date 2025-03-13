using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Impulse : ComportementState
{
    private float impulseTime = 5f;
    private float impulseTimer;
    private float impulseRange;
    private float trueImpulseRange;
    private float impulseForce;
    private bool destroyOnUse = false;
    [Tooltip("Si Rigidbody sur lui")]
    private GameObject feedback;
    
    public C_Double_Impulse(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 2;
        leftValue = 1;
        rightValue = 1;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.impulseColor);
        
        impulseTime = _sm.comportementManager.impulseData.doubleImpulseTime;
        impulseTimer = 0f;
        impulseRange = 2.5f * _sm.comportementManager.impulseData.impulseRange;

        if (_sm.isPlayer)
        {
            trueImpulseRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + impulseRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            trueImpulseRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + impulseRange;
        }
        
        // pb si obj n'a pas de collider direct (ax Player)
        impulseForce = 2.5f * _sm.comportementManager.impulseData.impulseForce;
        destroyOnUse = _sm.comportementManager.impulseData.destroyOnUse;
        feedback = _sm.comportementManager.impulseData.impulseFeedback;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        
        impulseTimer += Time.deltaTime;
        if (impulseTimer >= impulseTime)
        {
            Impulse();
            impulseTimer = 0;
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
        Gizmos.DrawWireSphere(_sm.transform.position, trueImpulseRange);
    }  
    
    public void Impulse()
    {
        if (feedback)
        {
            GameObject shockWave = _sm.comportementManager.InstantiateFeedback(feedback, _sm.transform.position, Quaternion.identity);
            shockWave.GetComponent<GrowToRadius>().targetRadius = trueImpulseRange;
        }
        
        //SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.repulseBoom,_sm.gameObject);
        //SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.repulseBoom,_sm.gameObject);
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueImpulseRange);
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
                    ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected,impulseForce);
                    
                    // player relache l'objet repulse
                    if (isGrabbed) //juste isGrabbed ? objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject
                    {
                        objectAffected.GetComponent<GrabObject>().Drop(true);
                    }
                }
                else if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, impulseForce);
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
        Vector3 direction = (objToApply.transform.position - _sm.transform.position).normalized;
        rbObj.AddForce(direction * force, ForceMode.Impulse);
    }
}
