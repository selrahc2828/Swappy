using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;
using UnityEngine;

public class C_Magnet_Rocket : ComportementState
{
    public GameObject prefabMagnetTrail;
    public float rocketMagnetForce = 20f;
    public GameObject prefabForceField;
    public float distanceBetweenPoint = 2f;
    
    public TrailRenderer magnetTrail;
    
    public Vector3 lastPos;
    
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
        prefabMagnetTrail = _sm.comportementManager.prefabMagnetTrail;
        
        magnetTrail = _sm.comportementManager?.InstantiateFeedback(prefabMagnetTrail,_sm.transform.position, Quaternion.identity, _sm.transform).GetComponent<TrailRenderer>();
        if (magnetTrail != null)
        {
            magnetTrail.time = _sm.comportementManager.magnetTrailDuration;//temps de vie du trail (on l'utilise aussi pour les forcefield)
        }

        distanceBetweenPoint = _sm.comportementManager.magnetRocketDistanceBetweenPoint;
        prefabForceField = _sm.comportementManager.prefabMagnetRocketForcefield;
        lastPos = _sm.transform.position;
        
        Debug.LogWarning($"lastPos of {_sm.name} : {lastPos}");
    }

    public override void TickLogic()
    {
        base.TickLogic();
        /*
         * toutes les X seconde fait spawn prefab "forcefield"
         * orientation présédente pos - pos actuelle
         * => forcefield apply force sur son transform.up (en théorie il sera orienté vers direction de la rocket
         */
        Debug.LogWarning($"lastPos.position : {lastPos}\n" +
                         $"transform : {_sm.transform.position}");
        if (Vector3.Distance(lastPos, _sm.transform.position) > distanceBetweenPoint)
        {
            //lastPos = _sm.transform;
            GameObject forcefiled = _sm.comportementManager?.InstantiateFeedback(prefabForceField,_sm.transform.position, Quaternion.identity);
            //on scale le chanmp par rapport à la largeur et longueur de l'objet qui le fait spawn, pass de sa hauteur
            forcefiled.transform.localScale = new Vector3(_sm.GetComponent<Collider>().bounds.extents.x, 1, _sm.GetComponent<Collider>().bounds.extents.z);
            
            _sm.comportementManager.DestroyObj(forcefiled, _sm.comportementManager.magnetTrailDuration);

            lastPos = _sm.transform.position;
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
}
