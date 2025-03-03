using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Magnet_Rocket : ComportementState
{
    public float magnetRocketFlyTime = 4f;
    public float rocketMagnetForce = 20f;
    public float rocketMagnetForceOnPlayer = 20f;
    public float rocketMagnetForceWhenGrab = 20f;
    public float magnetTrailForce = 20f;
    public float magnetTrailSpeedLerp = 1f;
    public float magnetTrailTimeBeforeMove = 3f;
    private float _timer = 0f;
    private bool _rocketOn = true;

    public GameObject prefabForceField;
    private GameObject magnetFieldObject;
    
    public Vector3 magnetPos;

    private bool isSoundOn;
    
    public C_Magnet_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 108;
        leftValue = 27;
        rightValue = 81;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.rocketColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

        magnetRocketFlyTime = _sm.comportementManager.magnetRocketData.magnetRocketFlyTime;
        rocketMagnetForce = _sm.comportementManager.magnetRocketData.rocketMagnetForce;
        rocketMagnetForceOnPlayer = _sm.comportementManager.magnetRocketData.rocketMagnetForceOnPlayer;
        rocketMagnetForceWhenGrab = _sm.comportementManager.magnetRocketData.rocketMagnetForceWhenGrab;
        magnetTrailForce = _sm.comportementManager.magnetRocketData.magnetTrailForce; 
            
        magnetTrailSpeedLerp = _sm.comportementManager.magnetRocketData.magnetTrailLerp;
        magnetTrailTimeBeforeMove = _sm.comportementManager.magnetRocketData.magnetTrailTimeBeforeMove;
        prefabForceField = _sm.comportementManager.magnetRocketData.prefabMagnetRocketForcefield;
        
        // spawn de la zone de magnet
        magnetPos = _sm.transform.position;
        if (_sm.isPlayer)
        {
            magnetPos.y = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude;
        }
        else
        {
            magnetPos.y -= _sm.GetComponent<Collider>().bounds.extents.magnitude;
        }
        
        SpawnForceField();

    }

    public override void TickLogic()
    {
        base.TickLogic();
        /*
         * toutes les X seconde fait spawn prefab "forcefield" OU on le detache (la rocket se stop mais sa trainée perdure
         * vu qu'on en a plusieurs généré par 1 seul comportement, les addForce sont géré dans RocketMagnetEffect
         * dans RocketMagnetEffect, on récupère les force (normal, onPlayer et whenGrab)
         * on doit géré 
         */
        
        
        _timer += Time.deltaTime;
        if (_timer >= magnetRocketFlyTime)
        {
            _rocketOn = !_rocketOn;
            _timer = 0f;
            
            // gestion de la zone qui applique la force
            if (_rocketOn)
            {
                if (!isSoundOn)
                {
                    SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.propelerStart,_sm.gameObject);
                    isSoundOn = true;
                }
                
                SpawnForceField();//feebback et apply de force, mis dedans pour être conservé quand se sépare de la rocket
            }
            else
            {
                isSoundOn = false;
                // on met atDetachAndDestroy à true
                if (magnetFieldObject != null)
                {
                    RocketMagnetEffect effect = magnetFieldObject.GetComponent<RocketMagnetEffect>();
                    if (effect != null)
                    {
                        effect.atDetachAndDestroy = true; 
                        // Passe le booléen à true, il sort du parent et sera détruit quand les 2 extrémités seront proche
                    }
                }
            }
        }
        
        if (_rocketOn)
        {
            ApplyForce();
        }
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
        
    }

    public override void Exit()
    {
        base.Exit();
        if (magnetFieldObject)
        {
            RocketMagnetEffect effect = magnetFieldObject?.GetComponent<RocketMagnetEffect>();
            if (effect != null)
            {
                effect.atDetachAndDestroy = true; 
                // Passe le booléen à true, il sort du parent et sera détruit quand les 2 extrémités seront proche
            }
        }
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);

      
    }

    public void ApplyForce()
    {
        if (_sm.isPlayer)
        {
            _sm.rb.AddForce(Vector3.up * rocketMagnetForceOnPlayer, ForceMode.Force);
        }
        else if(isGrabbed)
        {
            _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(Vector3.up * rocketMagnetForceWhenGrab, ForceMode.Force);
        }
        else
        {
            _sm.rb.AddForce(Vector3.up * rocketMagnetForce, ForceMode.Force);
        }
    }

    public void SpawnForceField()
    {
        if (magnetFieldObject)//si on en avait déjà instancié un, on dit de le retirer du parent et de le détruire
        {
            magnetFieldObject.GetComponent<RocketMagnetEffect>().atDetachAndDestroy = true;
            magnetFieldObject = null;
        }
        
        magnetFieldObject = _sm.comportementManager.InstantiateFeedback(prefabForceField,magnetPos, Quaternion.identity);//, _sm.transform => parent mais pose des pb
        RocketMagnetEffect effect = magnetFieldObject.GetComponent<RocketMagnetEffect>();
        effect.rocketObject = _sm.gameObject.transform;
        effect.delay = magnetTrailSpeedLerp;//delay
        effect.isPlayer = _sm.isPlayer;
        effect.timeBeforeMove = magnetTrailTimeBeforeMove;
        effect.effectForce = magnetTrailForce;
        effect.effectForceOnPlayer = rocketMagnetForceOnPlayer;
        effect.effectForceWhenGrab = rocketMagnetForceWhenGrab;
    }
}
