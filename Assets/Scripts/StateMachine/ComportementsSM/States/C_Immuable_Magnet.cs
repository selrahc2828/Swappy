using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Immuable_Magnet : ComportementState
{
    private GameObject forcefieldObj;
    private Vector3 baseVelocity;
    private Vector3 baseAngularVelocity;
    private float magnetRange;
    private float trueMagnetRange;
    private float magnetForce;
    private float equilibriumDistance;
    private float dampingFactor;

    private List<Rigidbody> magnetedObjects = new List<Rigidbody>() ;

    public C_Immuable_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {

        isKinematic = true;
        stateValue = 36;
        leftValue = 9;
        rightValue = 27;
        base.Enter();
        
        baseVelocity = _sm.rb.velocity;
        baseAngularVelocity = _sm.rb.angularVelocity;
        _sm.rb.isKinematic = true;
        
        magnetRange = _sm.comportementManager.magnetData.magnetRange;
        equilibriumDistance = _sm.comportementManager.magnetData.equilibriumDistance;
        dampingFactor = _sm.comportementManager.magnetData.dampingFactor;
        if (_sm.isPlayer)
        {
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;
        }
        else
        {
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;
            ColorShaderOutline(_sm.comportementManager.immuableColor, _sm.comportementManager.magnetColor);
        }
        magnetForce = _sm.comportementManager.magnetData.magnetForce;

        forcefieldObj = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Magnet, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        forcefieldObj.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        forcefieldObj.GetComponent<GrowToRadius>().atDestroy = false;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        Attract();
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
    }

    public override void Exit()
    {
        base.Exit();
        _sm.rb.isKinematic = false;
        _sm.rb.velocity = baseVelocity;
        _sm.rb.angularVelocity = baseAngularVelocity;

        _sm.comportementManager.DestroyObj(forcefieldObj);

    }

    public void Attract()
    {
        List<Rigidbody> newMagnetedObjects = new List<Rigidbody>();
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueMagnetRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.gameObject != _sm.gameObject) // applique pas sur player et lui même
                {
                    if (objectInRange.CompareTag("Player"))
                    {
                        ApplyForce(objectInRange.GetComponentInParent<Rigidbody>(), objectInRange.gameObject, magnetForce);
                    }

                    if (objectInRange.GetComponent<Rigidbody>() != null)
                    {
                        ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, magnetForce);

                        if (!magnetedObjects.Contains(objectInRange.GetComponent<Rigidbody>()))
                        {
                            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject,objectInRange.GetComponent<Rigidbody>().mass);
                            
                        }
                        newMagnetedObjects.Add(objectInRange.GetComponent<Rigidbody>());
                    }

                    #region AoeChecks
                    BreakableObject breakableScript = null;
                    try
                    {
                        breakableScript = objectInRange.GetComponent<AoeCondition>().CheckMagnetImmuableAoeCondition();
                    }
                    catch { }
                    finally
                    {
                        if (breakableScript != null)
                        {
                            foreach (Rigidbody rb in breakableScript.ShatterObject())
                            {
                                ApplyForce(rb, rb.gameObject, magnetForce);
                            }
                        }
                    }
                    #endregion
                }
            }
        }
        magnetedObjects = newMagnetedObjects;
    }
    
    public void ApplyForce( Rigidbody rb,GameObject objToApply, float force)
    {

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
