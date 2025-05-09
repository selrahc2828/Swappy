using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Impulse_Bouncing : ComportementState
{
    private float impulseBounceForce;
    private float trueImpulseBounceForce;
    private float saveImpulseForce;//pour changer au moment d'une collision si grab
    private float impulseBounceRange;
    private float trueImpulseBounceRange;
    private float impulseBounceTimer;
    private float impulseBounceCooldown;
    private GameObject impulseBounceFeedback;
    private bool applyOnMe = false;

    private float impulseForceMultiplier;//coeff de conservation de la velocité lors de la collision
    
    private bool canBounce = true; // verif si on peut faire l'impulse

    private PhysicMaterial _bouncyMaterial;
    private PhysicMaterial _basePlayerMaterial;
    private PhysicMaterial _basePlayerSlideMaterial;
    
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
        
        impulseBounceTimer = _sm.comportementManager.impulseBounceData.impulseBounceTimer;
        impulseBounceCooldown = impulseBounceTimer;
        impulseBounceRange = _sm.comportementManager.impulseBounceData.impulseBounceRange;

        _bouncyMaterial = _sm.comportementManager.bounceData.bouncyMaterial;
        if (_sm.isPlayer)
        {
            trueImpulseBounceRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + impulseBounceRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
            
            _basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = _bouncyMaterial;
        }
        else
        {
            trueImpulseBounceRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + impulseBounceRange;
            
            _sm.GetComponent<Collider>().material = _bouncyMaterial;

        }
        
        impulseBounceForce = _sm.comportementManager.impulseBounceData.impulseBounceForce;
        saveImpulseForce = impulseBounceForce;
        trueImpulseBounceForce = impulseBounceForce;
        impulseForceMultiplier = _sm.comportementManager.impulseBounceData.impulseForceMultiplier;
        applyOnMe = _sm.comportementManager.impulseData.applyOnMe;
        impulseBounceFeedback = _sm.comportementManager.impulseData.impulseFeedback;
        
        feedBack_GO_Right = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);

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
        _sm.comportementManager.DestroyObj(feedBack_GO_Right);
        
        if (_sm.isPlayer)
        {
            _sm.comportementManager.playerBouncingCollider.material = _basePlayerMaterial;
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
        }

    }

    public override void CollisionStart(Collision other)
    {
        base.CollisionStart(other);
        if (other!= null && canBounce)
        {
            trueImpulseBounceForce = impulseBounceForce + _sm.rb.velocity.magnitude * impulseForceMultiplier;
            // Debug.LogWarning($"dans enter: {trueImpulseBounceForce}");
            Repulse();
            canBounce = false;
        }
        
    }

    public override void CollisionEnd(Collision other)
    {
        base.CollisionEnd(other);
        trueImpulseBounceForce = saveImpulseForce;
        // Debug.LogWarning($"dans exit: {trueImpulseBounceForce}");

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
        // Debug.LogWarning($"dans repulse: {trueImpulseBounceForce}");
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueImpulseBounceRange);
        
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
                    ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected, impulseBounceForce);

                    // player relache l'objet repulse
                    if (isGrabbed) //juste isGrabbed ? objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject
                    {
                        objectAffected.GetComponent<GrabObject>().Release(true);
                    }
                }
                else if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, impulseBounceForce);
                }
            }
        }
    }
            public void ApplyForce(Rigidbody rbObj, GameObject objToApply, float force)
    {
        if (!applyOnMe && objToApply == _sm.gameObject)
        {
            // si rigid body sur objet, on applique pas la force sur lui pour le lancer par exemple
            return;
        }
        else
        {
            Vector3 direction = (objToApply.transform.position - _sm.transform.position).normalized;
            rbObj.AddForce(direction * force, ForceMode.Impulse);
        }
    }    
}
