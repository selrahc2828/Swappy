using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class C_Magnet_Rocket : ComportementState
{
    public GameObject prefabMagnetTrail;
    public float rocketMagnetForce = 20f;
    public float rocketMagnetForceOnPlayer = 20f;
    public float rocketMagnetForceWhenGrab = 20f;
    public float magnetTrailDuration = 1f;
    private float _timer = 0f;
    private bool _rocketOn = true;

    public GameObject prefabForceField;
    private GameObject magnetFieldObject;
    
    public TrailRenderer magnetTrail;
    
    public Vector3 magnetPos;
    
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

        rocketMagnetForce = _sm.comportementManager.rocketMagnetForce;
        rocketMagnetForceOnPlayer = _sm.comportementManager.rocketMagnetForceOnPlayer;
        rocketMagnetForceWhenGrab = _sm.comportementManager.rocketMagnetForceWhenGrab;
        
        magnetTrailDuration = _sm.comportementManager.magnetTrailDuration;
        
        // prefabMagnetTrail = _sm.comportementManager.prefabMagnetTrail;
        // magnetTrail = _sm.comportementManager?.InstantiateFeedback(prefabMagnetTrail,_sm.transform.position, Quaternion.identity, _sm.transform).GetComponent<TrailRenderer>();
        // if (magnetTrail != null)
        // {
        //     magnetTrail.time = _sm.comportementManager.magnetTrailDuration;//temps de vie du trail (on l'utilise aussi pour les forcefield)
        // }

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
         * toutes les X seconde fait spawn prefab "forcefield"
         * orientation présédente pos - pos actuelle
         * => forcefield apply force sur son transform.up (en théorie il sera orienté vers direction de la rocket
         */
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
        _timer += Time.fixedDeltaTime;
        if (_timer >= magnetTrailDuration)
        {
            _rocketOn = !_rocketOn;
            _timer = 0f;
            Debug.Log($"destroy magnetFieldObject");
            _sm.comportementManager.DestroyObj(magnetFieldObject);
            
            if (_rocketOn)
            {
                Debug.Log($"spawn magnetFieldObject");
                SpawnForceField();
            }
        }
        
        if (_rocketOn)
        {
            ApplyForce();
        }
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
        magnetFieldObject = _sm.comportementManager.InstantiateFeedback(prefabForceField,magnetPos, Quaternion.identity, _sm.transform);
        RocketMagnetEffect effect = magnetFieldObject.GetComponent<RocketMagnetEffect>();
        effect.rocketObject = _sm.gameObject.transform;
        effect.delay = _sm.comportementManager.magnetTrailDuration;//life time
    }
}
