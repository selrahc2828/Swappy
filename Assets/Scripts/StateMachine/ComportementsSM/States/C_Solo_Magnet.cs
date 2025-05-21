
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

public class C_Solo_Magnet : ComportementState
{
    private float magnetRange;
    private float trueMagnetRange;
    private float magnetForce;

    private List<Rigidbody> magnetedObjects = new List<Rigidbody>();
    
    private float equilibriumDistance;
    private float dampingFactor;

    
    public C_Solo_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        isKinematic = false;
        stateValue = 27;
        leftValue = 27;
        rightValue = 0;
        base.Enter();

        magnetRange = _sm.comportementManager.magnetData.magnetRange;
        if (_sm.isPlayer)
        {
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;
        }
        else
        {
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;
        }
        magnetForce = _sm.comportementManager.magnetData.magnetForce;
        equilibriumDistance = _sm.comportementManager.magnetData.equilibriumDistance;
        dampingFactor = _sm.comportementManager.magnetData.dampingFactor;
        
        // _sm.rend.material = _sm.magnet;
        ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.noComportementColor);

        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Magnet, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Left.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        feedBack_GO_Left.GetComponent<GrowToRadius>().atDestroy = false;
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
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);

    }

    public override void DisplayGizmos()
    {
        base.DisplayGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sm.transform.position, trueMagnetRange);   
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_sm.transform.position, equilibriumDistance);
    }

    public void Attract()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueMagnetRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.gameObject != _sm.gameObject) // applique pas sur player et lui même
                {
                    if (objectInRange.GetComponent<Rigidbody>() != null)
                    {
                        ApplyForce( objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, magnetForce);

                    }
                }
            }
        }
    }
    
    public void ApplyForce( Rigidbody rb,GameObject objToApply, float force)
    {

        if (!magnetedObjects.Contains(rb))
        {
            magnetedObjects.Add(rb);
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject,rb.mass);
        }


        if (rb == null) return;


        Vector3 toObject = objToApply.transform.position - _sm.transform.position;
        float currentDistance = toObject.magnitude;

        // Si l'objet est exactement à la distance souhaitée, aucune force
        if (Mathf.Approximately(currentDistance, equilibriumDistance)) return;

        // Calcul du point sur la sphère (direction * rayon)
        Vector3 targetPoint = _sm.transform.position + toObject.normalized * equilibriumDistance;

        // Direction vers ce point d'équilibre
        Vector3 forceDir = (targetPoint - objToApply.transform.position).normalized;

        rb.AddForce(forceDir * force, ForceMode.Force);
        
        // --- Damping : freine la vitesse radiale (vers/depuis le centre) ---
        Vector3 radialVelocity = Vector3.Project(rb.velocity, forceDir);
        rb.velocity -= radialVelocity * (dampingFactor * Time.deltaTime);
    }
}
