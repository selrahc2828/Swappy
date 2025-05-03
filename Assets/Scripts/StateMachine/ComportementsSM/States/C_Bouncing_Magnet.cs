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
    private float magnetForceOnPlayerMultiplier;
    private float magnetForceWhenGrabMultiplier;
    
    private float magnetRange;
    private float trueMagnetRange;
    private float saveMagnetRange;//pour changer au moment d'une collision si grab
    private float magnetForce;
    private float magnetForceOnPlayer;
    private float magnetForceWhenGrab;
    private float trueMagnetForce;
    private bool magnetGradiantForce;
    
    private float bounceMagnitude;

    //private GameObject sonMagnet;
    
    [Range(1,3)]
    // private float magnetUpScaleMultiplier;
    // avec mat bounce, rebond direct quand grab et touche surface, collisionEnter/exit s'enchaine trop vite pour range upsacle
    private float delayScale;
    private float timeSinceCollisionStrat;
    private bool collisionStart = false;//pour pas appeler plusieurs en même temps
    
    //grab
    private bool _rescaleRangeOnce;
    
    // gestion de la collision quand grab, pour la range 
    private bool isCollisionTimerActive;
    private float collisionTimer = 0f;


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

        bouncyMaterial = _sm.comportementManager.bounceData.bouncyMaterial;
        magnetRange = _sm.comportementManager.magnetBounceData.magnetBounceRange;

        if (_sm.isPlayer)
        {
            //pb State se fait avant set dans manager
            basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = bouncyMaterial;
            
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;//toujours des pb de range trop grande mais mieux
        }
        else
        {
            _sm.GetComponent<Collider>().material = bouncyMaterial;
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;//toujours des pb de range trop grande mais mieux
        }
        saveMagnetRange = trueMagnetRange;
        
        magnetForce = _sm.comportementManager.magnetBounceData.magnetBounceForce;
        magnetForceOnPlayer = _sm.comportementManager.magnetBounceData.magnetBounceForceOnPlayer;
        magnetForceWhenGrab = _sm.comportementManager.magnetBounceData.magnetBounceForceWhenGrab;
        trueMagnetForce = magnetForce;

        magnetForceMultiplier = _sm.comportementManager.magnetBounceData.magnetForceMultiplier;
        magnetForceOnPlayerMultiplier = _sm.comportementManager.magnetBounceData.magnetForceOnPlayerMultiplier;
        magnetForceWhenGrabMultiplier = _sm.comportementManager.magnetBounceData.magnetForceWhenGrabMultiplier;
        
        // set la prefab qui va appliquer la force
        forceFieldObj = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.magnetGenericPrefab,_sm.transform.position, Quaternion.identity, _sm.transform);//, _sm.transform => parent mais pose des pb
        forceFieldObj.GetComponent<MagnetForceField>().force = trueMagnetForce;
        forceFieldObj.GetComponent<MagnetForceField>().intervalBetweenBurst = _sm.comportementManager.magnetBounceData.intervalBetweenBurst;
        forceFieldObj.GetComponent<MagnetForceField>().burstColor = _sm.comportementManager.magnetBounceData.burstColor;
        forceFieldObj.GetComponent<MagnetForceField>().delayDisplay = _sm.comportementManager.magnetBounceData.delayDisplay;
        
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        forceFieldObj.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        forceFieldObj.GetComponent<GrowToRadius>().atDestroy = false;
    }

    public override void TickLogic()
    {
        base.TickLogic();

        //delay de reduction de range, lors d'une collision en grab
        if (isCollisionTimerActive)
        {
            collisionTimer += Time.deltaTime;

            if (collisionTimer >= _sm.comportementManager.magnetBounceData.delayDisplay)
            {
                // Réduit la range après x seconde après un choc en grab
                trueMagnetRange = saveMagnetRange / 2;
                forceFieldObj.GetComponent<GrowToRadius>().SetTargetScale(trueMagnetRange);
                forceFieldObj.GetComponent<GrowToRadius>().elapsedTime = 0f;

                // Désactive le timer
                isCollisionTimerActive = false;
            }
            else
            {
                
                
            }
        }

        ScaleGrab();
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
    }

    public override void Exit()
    {
        
        base.Exit();
        //_sm.comportementManager.DestroyObj(sonMagnet);
        _sm.GetComponentInChildren<Collider>().material = null;
        
        _sm.comportementManager.DestroyObj(forceFieldObj);
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);

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
            Rigidbody rb = _sm.GetComponent<Rigidbody>();
            float impact = rb.velocity.magnitude;
            //Debug.LogWarning($"impact velo {impact}");

            // forceFieldObj.GetComponent<MagnetForceField>().Bounce();
            // Debug.Log($"Collision Start boolburst {forceFieldObj.GetComponent<MagnetForceField>().boolBurst}");

            //il y a des cas où le apply burst passe pas dans grab
            // SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.bounceHit,_sm.gameObject);
            // SoundManager.Instance.PlaysoundCompAimaint(_sm.gameObject);
            if (isGrabbed)
            {
                //boolBurst jamais true ici
                
                forceFieldObj.GetComponent<MagnetForceField>().burstForce = magnetForceWhenGrab + impact * magnetForceWhenGrabMultiplier;

                // peut pas utiliser boolBurst de MagnetForceField car il est déjà passé false
                // et si je mets Bounce au début ça "décale" le moment où l'impulsion est fait
                if (!isCollisionTimerActive)//si unScale est lancé, on le refait pas
                {
                    // collisionStart = true; // start rescale magnet
                    // timeSinceCollisionEnd = 0f;//reset "scale timer"
                    
                    //aggrandi la range
                    trueMagnetRange = saveMagnetRange;
                    forceFieldObj.GetComponent<GrowToRadius>().SetTargetScale(saveMagnetRange);
                    forceFieldObj.GetComponent<GrowToRadius>().elapsedTime = 0f;
                    
                    //TEST AVEC DELAY PLUS LONG
                    // impression que le impulse se fait mais trop vite
                    
                    isCollisionTimerActive = true;
                    collisionTimer = 0f;
                }
            }
            else
            {
                // Calculer la force du rebond (différence de vélocité)
                if (_sm.isPlayer)
                {
                    forceFieldObj.GetComponent<MagnetForceField>().burstForce = magnetForceOnPlayer + impact * magnetForceOnPlayerMultiplier;
                }
                else
                {
                    forceFieldObj.GetComponent<MagnetForceField>().burstForce = magnetForce + impact * magnetForceMultiplier;
                }
            }
            // ne se fait pas dans MagnetForceField si en cooldown, 
            forceFieldObj.GetComponent<MagnetForceField>().Bounce();
            // Debug.Log($"Collision Start boolburst {forceFieldObj.GetComponent<MagnetForceField>().boolBurst} \n" +
            //           $"timer burst {forceFieldObj.GetComponent<MagnetForceField>()._timerBurst}");
        }
    }

    public override void CollisionEnd(Collision other)
    {
        forceFieldObj.GetComponent<MagnetForceField>().boolBurst = false;
    }


    void ScaleGrab()
    {
        //scale quand on grab ou relache l'objet
        if (isGrabbed & !_rescaleRangeOnce)//grab et pas rescale
        {
            _rescaleRangeOnce = true;
            trueMagnetRange = saveMagnetRange / 2;
            forceFieldObj.GetComponent<GrowToRadius>().SetTargetScale(trueMagnetRange);
            forceFieldObj.GetComponent<GrowToRadius>().elapsedTime = 0f;
        }
        else if(!isGrabbed && _rescaleRangeOnce)//pour remettre la scale de base 
        {
            _rescaleRangeOnce = false;
            trueMagnetRange = saveMagnetRange;
            forceFieldObj.GetComponent<GrowToRadius>().SetTargetScale(trueMagnetRange);
            forceFieldObj.GetComponent<GrowToRadius>().elapsedTime = 0f;
        }
    }
}
