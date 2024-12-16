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
    public float saveMagnetRange;//pour changer au moment d'une collision si grab
    public float magnetForce;
    public float trueMagnetForce;
    public bool magnetGradiantForce;
    
    public float magnetForceMultiplier;
    public float bounceMagnitude;

    [Range(1,3)]
    public float magnetUpScaleMultiplier;
    // avec mat bounce, rebond direct quand grab et touche surface, collisionEnter/exit s'enchaine trop vite pour range upsacle
    public float delayScale;
    public float timeSinceCollisionStrat;
    private bool collisionStart = false;//pour pas appeler plusieurs en même temps

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
        magnetRange = _sm.comportementManager.magnetBounceRange;

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
        saveMagnetRange = trueMagnetRange;
        
        magnetForce = _sm.comportementManager.magnetBounceForce;
        trueMagnetForce = magnetForce;

        magnetForceMultiplier = _sm.comportementManager.magnetForceVelocityMultiplier;
        magnetUpScaleMultiplier = _sm.comportementManager.magnetScaleMultiplier;
        
        magnetGradiantForce = _sm.comportementManager.magnetGradiantForce;
        
        delayScale = _sm.comportementManager.delayScale;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        Attract();//même comportement sur player et sur objet

        
        if (collisionStart)
        {
            timeSinceCollisionStrat += Time.deltaTime; // Incrémente le timer

            if (timeSinceCollisionStrat >= delayScale) // Vérifie si le délai est écoulé
            {
                timeSinceCollisionStrat = 0f;
                trueMagnetRange = saveMagnetRange; // reset range
                trueMagnetForce = magnetForce;
                collisionStart = false; // on peut à nouveau faire le upScale avec collision
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
        _sm.GetComponentInChildren<Collider>().material = null;
    }
    
    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sm.transform.position, trueMagnetRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sm.transform.position, _sm.GetComponentInChildren<Collider>().bounds.extents.magnitude); 
    }
    
    public override void CollisionStart(Collision other)
    {
        if (other!= null)
        {
            if (isGrabbed)
            {
                if (!collisionStart)//si unScale est lancé, on le refait pas
                {
                    trueMagnetRange *= magnetUpScaleMultiplier;
                
                    collisionStart = true; // start rescale magnet
                    //timeSinceCollisionEnd = 0f;//reset "scale timer"
                }
            }
            else
            {
                bounceMagnitude = _sm.rb.velocity.magnitude;
                trueMagnetForce = magnetForce + bounceMagnitude * magnetForceMultiplier;
                Attract(true);                      
            }
        }
    }

    public override void CollisionEnd(Collision other)
    {
        if (isGrabbed)
        {
            // trueMagnetRange = saveMagnetRange;//revient à la range de base
            // on fait un delay pour le UpScale, le reset de la range est fait dedans, pour le moment
        }
        else
        {
            trueMagnetForce = magnetForce;//reset de la force de base, si pas en main
        }
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
