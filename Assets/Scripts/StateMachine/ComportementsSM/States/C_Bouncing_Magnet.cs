using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Bouncing_Magnet : ComportementState
{
    //même que C_Solo_Bouncing
    public PhysicMaterial bouncyMaterial;
    public PhysicMaterial basePlayerMaterial;
    public PhysicMaterial basePlayerSlideMaterial;
    
    public float magnetRange;
    public float trueMagnetRange;
    public float magnetForce;
    public float trueMagnetForce;
    public bool magnetGradiantForce;
    
    public float magnetForceMultiplier;
    public float bounceMagnitude;
    public C_Bouncing_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 30;
        leftValue = 3;
        rightValue = 27;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.magnetColor);

        bouncyMaterial = _sm.comportementManager.bouncyMaterial;
        magnetRange = _sm.comportementManager.magnetRange;

        if (_sm.isPlayer)
        {
            //pb State se fait avent set dans manager
            basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            basePlayerSlideMaterial = _sm.comportementManager.playerSlidingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = bouncyMaterial;
            _sm.comportementManager.playerSlidingCollider.material = bouncyMaterial;
            
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;//toujours des pb de range trop grande mais mieux

        }
        else
        {
            _sm.GetComponent<Collider>().material = bouncyMaterial;
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;//toujours des pb de range trop grande mais mieux

        }
        
        magnetForceMultiplier = _sm.comportementManager.magnetForceVelocityMultiplier;
        magnetForce = _sm.comportementManager.magnetForce;
        trueMagnetForce = magnetForce;
        magnetGradiantForce = _sm.comportementManager.magnetGradiantForce;
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
        _sm.GetComponent<Collider>().material = null;
    }
    
    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sm.transform.position, trueMagnetRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sm.transform.position, _sm.GetComponent<Collider>().bounds.extents.magnitude); 
    }
    
    public override void CollisionStart(Collision other)
    {
        if (!other.gameObject.GetComponent<StateMachine>())
        {
            if (isGrabbed)
            {
                //Debug.LogWarning($"Grab Collider {other.gameObject.name}");
            }
            else
            {
                bounceMagnitude = _sm.rb.velocity.magnitude;
                trueMagnetForce = magnetForce + bounceMagnitude * magnetForceMultiplier;
                Attract(true);                      
            }
        }


        // truemagnet Force = force * magnetForceMultiplier
        // voir comment remmetre force de base
    }

    public override void CollisionEnd(Collision other)
    {
        trueMagnetForce = magnetForce;
        // Debug.LogWarning($"collision END magnet force {trueMagnetForce}");

    }
    
    public void Attract(bool isCollide = false)
    {
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueMagnetRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (!objectInRange.gameObject.CompareTag("Player") && objectInRange.gameObject != _sm.gameObject)// applique pas sur player et lui même
                {
                    if (objectInRange.GetComponent<Rigidbody>() != null)
                    {
                        if (isCollide) // collision
                        {
                            ApplyForce(false,objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, trueMagnetForce);
                        }
                        else
                        {
                            ApplyForce(magnetGradiantForce,objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, trueMagnetForce);
                        }
                    }
                }
            }
        }
    }
    
    public void ApplyForce(bool isGradient, Rigidbody rbObj,GameObject objToApply, float force)
    {
        // Debug.LogWarning($"APPLYFORCE magnet force: {force}");

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
