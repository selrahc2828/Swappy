using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Magnet_Rocket : ComportementState
{
    private float magnetRocketFlyTime = 4f;
    private float rocketMagnetForce = 20f;
    private float rocketMagnetForceOnPlayer = 20f;
    private float rocketMagnetForceWhenGrab = 20f;
    private float magnetTrailSpeedLerp = 1f;
    private float magnetTrailTimeBeforeMove = 3f;
    private float _timer = 0f;
    private bool _rocketOn = true;

    private GameObject prefabForceField;
    private GameObject magnetFieldObject;
    
    private Vector3 magnetPos;
    
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

        magnetRocketFlyTime = _sm.comportementManager.magnetRocketFlyTime;
        rocketMagnetForce = _sm.comportementManager.rocketMagnetForce;
        rocketMagnetForceOnPlayer = _sm.comportementManager.rocketMagnetForceOnPlayer;
        rocketMagnetForceWhenGrab = _sm.comportementManager.rocketMagnetForceWhenGrab;
        
        magnetTrailSpeedLerp = _sm.comportementManager.magnetTrailLerp;
        magnetTrailTimeBeforeMove = _sm.comportementManager.magnetTrailTimeBeforeMove;
        prefabForceField = _sm.comportementManager.prefabMagnetRocketForcefield;
        
        // spawn de la zone de magnet
        magnetPos = _sm.transform.position;
        magnetPos.y -= _sm.GetComponent<Collider>().bounds.extents.magnitude; 
        
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
        
        
        _timer += Time.fixedDeltaTime;
        if (_timer >= magnetRocketFlyTime)
        {
            _rocketOn = !_rocketOn;
            _timer = 0f;
            // _sm.comportementManager.DestroyObj(magnetFieldObject);
            //
            
            // gestion de la zone qui applique la force
            if (_rocketOn)
            {
                SpawnForceField();
            }
            else
            {
                // on met atDetachAndDestroy sur true
                if (magnetFieldObject != null)
                {
                    RocketMagnetEffect effect = magnetFieldObject.GetComponent<RocketMagnetEffect>();
                    if (effect != null)
                    {
                        effect.atDetachAndDestroy = true; // Passe le booléen à true, il sort du parent et sera détruit quand les 2 extrémités seront proche
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
        magnetFieldObject = _sm.comportementManager.InstantiateFeedback(prefabForceField,magnetPos, Quaternion.identity);//, _sm.transform => parent mais pose des pb
        RocketMagnetEffect effect = magnetFieldObject.GetComponent<RocketMagnetEffect>();
        effect.rocketObject = _sm.gameObject.transform;
        effect.delay = magnetTrailSpeedLerp;//delay
        effect.timeBeforeMove = magnetTrailTimeBeforeMove;
        effect.effectForce = rocketMagnetForce;
        effect.effectForceOnPlayer = rocketMagnetForceOnPlayer;
        effect.effectForceWhenGrab = rocketMagnetForceWhenGrab;
    }
}
