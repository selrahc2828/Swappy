using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Bouncing_Magnet : ComportementState
{
    //même que C_Solo_Bouncing
    private PhysicMaterial bouncyMaterial;
    private PhysicMaterial basePlayerMaterial;
    private PhysicMaterial basePlayerSlideMaterial;

    private GameObject forceFieldObj;
    private float magnetForceMultiplier;

    
    
    public Vector3 lastVelocity; 
    
    private float magnetRange;
    private float trueMagnetRange;
    private float saveMagnetRange;//pour changer au moment d'une collision si grab
    private float magnetForce;
    private float trueMagnetForce;
    private bool magnetGradiantForce;
    
    private float bounceMagnitude;

    [Range(1,3)]
    private float magnetUpScaleMultiplier;
    // avec mat bounce, rebond direct quand grab et touche surface, collisionEnter/exit s'enchaine trop vite pour range upsacle
    private float delayScale;
    private float timeSinceCollisionStrat;
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
            //pb State se fait avant set dans manager
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
        
        // set la prefab qui va appliquer la force
        forceFieldObj = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.magnetGenericPrefab,_sm.transform.position, Quaternion.identity, _sm.transform);//, _sm.transform => parent mais pose des pb
        forceFieldObj.GetComponent<MagnetForceField>().force = trueMagnetForce;
        forceFieldObj.GetComponent<MagnetForceField>().intervalBetweenBurst = _sm.comportementManager.intervalBetweenBurst;
        forceFieldObj.GetComponent<MagnetForceField>().burstColor = _sm.comportementManager.burstColor;
        
        forceFieldObj.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        forceFieldObj.GetComponent<GrowToRadius>().atDestroy = false;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        //Attract();//même comportement sur player et sur objet
        // lastVelocity = _sm.GetComponent<Rigidbody>().velocity;

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
        
        _sm.comportementManager.DestroyObj(forceFieldObj);
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
                    // trueMagnetRange *= magnetUpScaleMultiplier;
                    // collisionStart = true; // start rescale magnet
                    // timeSinceCollisionEnd = 0f;//reset "scale timer"
                }
            }
            else
            {
                
                Rigidbody rb = _sm.GetComponent<Rigidbody>();
                float impact = rb.velocity.magnitude;

                // Calculer la force du rebond (différence de vélocité)

                forceFieldObj.GetComponent<MagnetForceField>().burstForce = magnetForce + impact * magnetForceMultiplier;

                forceFieldObj.GetComponent<MagnetForceField>().Bounce();

                // Attract(true);
                // SetParamPrefabMagnet();
                // if (!collisionStart)
                // {
                //     forceFieldObj.GetComponent<MagnetForceField>().Bounce();
                // }

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
            //trueMagnetForce = magnetForce;//reset de la force de base, si pas en main
        }
        // Debug.LogWarning($"collision END magnet force {trueMagnetForce}");
    }


    void SetParamPrefabMagnet()
    {
        forceFieldObj.GetComponent<MagnetForceField>().force = trueMagnetForce;
    }
}
