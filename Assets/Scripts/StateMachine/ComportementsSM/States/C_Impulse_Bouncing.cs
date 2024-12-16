using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Impulse_Bouncing : ComportementState
{
    public float impulseBounceForce;
    public float trueImpulseBounceForce;
    public float saveImpulseForce;//pour changer au moment d'une collision si grab
    public float impulseBounceRange;
    public float trueImpulseBounceRange;
    public float impulseBounceTimer;
    public float impulseBounceCooldown;
    public GameObject impulseBounceFeedback;
    
    public float impulseForceMultiplier;//coeff de conservation de la velocité lors de la collision
    
    private bool canBounce = true; // verif si on peut faire l'impulse

    
    public float velocityMagnitude;
    
    public C_Impulse_Bouncing(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 4;
        leftValue = 1;
        rightValue = 3;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.bouncingColor);

        impulseBounceTimer = _sm.comportementManager.impulseBounceTimer;
        impulseBounceCooldown = impulseBounceTimer;
        impulseBounceRange = _sm.comportementManager.impulseBounceRange;

        // trueRepulserRange = repulserRange;
        if (_sm.isPlayer)
        {
            trueImpulseBounceRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + impulseBounceRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            trueImpulseBounceRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + impulseBounceRange;
        }
        
        impulseBounceForce = _sm.comportementManager.impulseBounceForce;
        saveImpulseForce = impulseBounceForce;
        trueImpulseBounceForce = impulseBounceForce;
        impulseForceMultiplier = _sm.comportementManager.impulseForceMultiplier;
        
        impulseBounceFeedback = _sm.comportementManager.impulseFeedback;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        if (!canBounce)
        {
            impulseBounceCooldown -= Time.deltaTime;
            if (impulseBounceCooldown < 0)
            {
                canBounce = true;
                impulseBounceCooldown = impulseBounceTimer;
            }
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

    public override void CollisionStart(Collision other)
    {
        base.CollisionStart(other);
        if (other!= null && canBounce)
        {
            trueImpulseBounceForce = impulseBounceForce + _sm.rb.velocity.magnitude * impulseForceMultiplier;
            Debug.LogWarning($"dans enter: {trueImpulseBounceForce}");
            Repulse();
            canBounce = false;
        }
    }

    public override void CollisionEnd(Collision other)
    {
        base.CollisionEnd(other);
        trueImpulseBounceForce = saveImpulseForce;
        Debug.LogWarning($"dans exit: {trueImpulseBounceForce}");

    }

    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sm.transform.position, trueImpulseBounceRange);  

    }
    
    public void Repulse()
    {
        if (impulseBounceFeedback)
        {
            GameObject shockWave = _sm.comportementManager.InstantiateFeedback(impulseBounceFeedback, _sm.transform.position, Quaternion.identity);
            shockWave.GetComponent<GrowToRadius>().targetRadius = trueImpulseBounceRange;
        }
        Debug.LogWarning($"dans repulse: {trueImpulseBounceForce}");
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueImpulseBounceRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.GetComponentInParent<Rigidbody>() == null)
                {
                    continue;
                }

                Vector3 direction;
                if (objectInRange.gameObject.CompareTag("Player"))
                {
                    //collider et rigid body pas au même endroit pour lui
                    GameObject objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>().gameObject;
                    
                    direction = (objectAffected.GetComponent<Rigidbody>().transform.position - _sm.transform.position).normalized;
                    objectAffected.GetComponent<Rigidbody>().AddForce( direction * trueImpulseBounceForce, ForceMode.Impulse);

                    // pb pour appliquer la force à cause du drag sur le rigidbody
                    //ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected,repulserForce);
                    
                    // // player relache l'objet repulse
                    // if (isGrabbed) //juste isGrabbed ? objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject
                    // {
                    //     objectAffected.GetComponent<GrabObject>().Drop(true);
                    // }
                }
                else
                {
                    direction = (objectInRange.GetComponent<Rigidbody>().transform.position - _sm.transform.position).normalized;

                    objectInRange.GetComponent<Rigidbody>().AddForce( direction * trueImpulseBounceForce, ForceMode.Impulse);
                }  
            }
        }
    }
}
