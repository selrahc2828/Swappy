using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Impulse_Rocket : ComportementState
{
    private float explosionForce;
    private float explosionRange;
    private float explosionTrueRange;
    private float rocketForce = 20;
    private float rocketForceOnPlayer = 20;
    private float rocketForceWhenGrab = 20;
    private float flyTime;
    private float onTimer;
    private float onCooldown;
    private float offCooldown;
    private float impulseTimer;
    private float timeBetweenImpulses;
    private float maxSpeed;
    private bool rocketOn;
    private GameObject feedback;
    private bool isSonOn;
    
    public C_Impulse_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 82;
        leftValue = 1;
        rightValue = 81;
        base.Enter();
        
        maxSpeed = _sm.comportementManager.rocketData.rocketMaxSpeed;
        explosionForce = _sm.comportementManager.impulseRocketData.impulseRocketExplosionForce;
        explosionRange = _sm.comportementManager.impulseRocketData.impulseRocketExplosionRange;
        feedback = _sm.comportementManager.impulseData.impulseFeedback;

        // trueRepulserRange = repulserRange;
        if (_sm.isPlayer)
        {
            explosionTrueRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + explosionRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            explosionTrueRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + explosionRange;
        }
        // pb si obj n'a pas de collider direct (ax Player)
        rocketForce = _sm.comportementManager.rocketData.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketData.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketData.rocketForceWhenGrab;
        onCooldown = _sm.comportementManager.rocketData.rocketOnCooldown;
        offCooldown = _sm.comportementManager.rocketData.rocketOffCooldown;
        timeBetweenImpulses = _sm.comportementManager.impulseRocketData.timeBetweenImpulses;
        onTimer = 0f;
        impulseTimer = 0f;
        rocketOn = false;
        
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.rocketColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

    }

    public override void TickLogic()
    {
        base.TickLogic();
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
        onTimer += Time.fixedDeltaTime;

        if (onTimer > onCooldown && !rocketOn)
        {
            rocketOn = true;
            onTimer = 0f;
        }

        if (onTimer > offCooldown && rocketOn)
        {
            rocketOn = false;
            isSonOn = false;
            onTimer = 0f;
        }


        if (rocketOn)
        {
            if (!isSonOn)
            {
                
                isSonOn = true;
            }
            impulseTimer += Time.fixedDeltaTime;
            if (impulseTimer > timeBetweenImpulses)
            {
                Repulse();
                impulseTimer = 0f;
            }

            if (_sm.rb.velocity.magnitude > maxSpeed && rocketOn)
            {
                _sm.rb.velocity = _sm.rb.velocity.normalized * maxSpeed;
            }
            
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForceOnPlayer , ForceMode.Force);
            }
            else if(isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(_sm.transform.up * rocketForceWhenGrab, ForceMode.Acceleration);
            }
            else
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForce, ForceMode.Force);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
    }
    
    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sm.transform.position, explosionTrueRange);  

    }    
 
    public void Repulse()
    {
        if (feedback)
        {
            GameObject shockWave = _sm.comportementManager.InstantiateFeedback(feedback, _sm.transform.position, Quaternion.identity);
            shockWave.GetComponent<GrowToRadius>().targetRadius = explosionTrueRange;
        }
        
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, explosionTrueRange);
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
                    ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected,explosionForce);
                    
                    // player relache l'objet repulse
                    if (isGrabbed) //juste isGrabbed ? objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject
                    {
                        objectAffected.GetComponent<GrabObject>().Release(true);
                    }
                }
                else if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, explosionForce);
                }  
            }
        }
    }

    public void ApplyForce(Rigidbody rbObj,GameObject objToApply, float force)
    {
        Vector3 direction = (objToApply.transform.position - _sm.transform.position).normalized;
        rbObj.AddForce( direction * force, ForceMode.Impulse);
    }
}
