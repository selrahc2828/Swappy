using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Magnet_Rocket : ComportementState
{
    private float magnetRange;
    private float trueMagnetRange;
    private float magnetForce;
    private float equilibriumDistance;
    private float dampingFactor;
    
    private float rocketForce;
    private float rocketForceOnPlayer;
    private float rocketForceWhenGrab;
    private float onCooldown;
    private float onFirstCooldown;
    private float offCooldown;
    private float timer;
    private float maxSpeed;
    private bool rocketOn;
    private bool startingSoonSignalSended;

    private List<Rigidbody> magnetedObjects = new List<Rigidbody>();
    
    
    public C_Magnet_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 108;
        leftValue = 27;
        rightValue = 81;
        base.Enter();
        
        rocketOn = false;
        startingSoonSignalSended = false;
        maxSpeed = _sm.comportementManager.magnetRocketData.rocketMaxSpeed;
        rocketForce = _sm.comportementManager.magnetRocketData.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.magnetRocketData.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.magnetRocketData.rocketForceWhenGrab;
        onCooldown = _sm.comportementManager.magnetRocketData.rocketOnCooldown;
        onFirstCooldown = _sm.comportementManager.magnetRocketData.rocketFirstOnCooldown;
        offCooldown = _sm.comportementManager.magnetRocketData.rocketOffCooldown;
        
        magnetRange = _sm.comportementManager.magnetRocketData.magnetRange;
        if (_sm.isPlayer)
        {
            trueMagnetRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + magnetRange;
        }
        else
        {
            trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;
            ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.rocketColor);
        }
        magnetForce = _sm.comportementManager.magnetRocketData.magnetForce;
        equilibriumDistance = _sm.comportementManager.magnetRocketData.equilibriumDistance;
        dampingFactor = _sm.comportementManager.magnetRocketData.dampingFactor;
        
        timer = 0f;
        timer += onCooldown - onFirstCooldown;

        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Magnet, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Left.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        feedBack_GO_Left.GetComponent<GrowToRadius>().atDestroy = false;
        feedBack_GO_Right = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

    }

    public override void TickLogic()
    {
        base.TickLogic();
        Attract();
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
        timer += Time.fixedDeltaTime;
        if (timer > onCooldown && !rocketOn)
        {
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
            rocketOn = true;
            timer = 0f;
        }
        if(timer >  (onCooldown -1) && !rocketOn && startingSoonSignalSended == false)
        {
            startingSoonSignalSended = true;
            GlobalEventManager.Instance.JustBeforeRocketStart(GetGameObject());
        }

        if (timer > offCooldown && rocketOn)
        {
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
            rocketOn = false;
            timer = 0f;
        }

        if (_sm.transform.InverseTransformDirection(_sm.rb.velocity).y > maxSpeed && rocketOn)// compare la velocity local y a la max speed
        {
            return;
        }

        if (rocketOn)
        {
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForceOnPlayer, ForceMode.Force);
            }
            else if (isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>()
                    .AddForce(_sm.transform.up * rocketForceWhenGrab, ForceMode.Acceleration);
            }
            else
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForce, ForceMode.Force);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
        _sm.comportementManager.DestroyObj(feedBack_GO_Right);
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
                    #region AoeChecks
                    BreakableObject breakableScript = null;
                    try
                    {
                        breakableScript = objectInRange.GetComponent<AoeCondition>().CheckMagnetRocketAoeCondition();
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
