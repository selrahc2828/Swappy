using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Impulse : ComportementState
{
    public float repulserTime = 5f;
    public float repulserTimer;
    public float repulserRange;
    public float repulserForce;
    public bool destroyOnUse = false;
    public bool impulseGradiantForce = false;
    [Tooltip("Si Rigidbody sur lui")]
    public bool applyOnMe = false;
    public GameObject feedback;
    
    public C_Solo_Impulse(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 1;
        leftValue = 1;
        rightValue = 0;
        base.Enter();
        
        repulserTime = _sm.comportementManager.repulserTime;
        repulserTimer = _sm.comportementManager.repulserTimer;
        repulserRange = _sm.comportementManager.repulserRange;
        repulserForce = _sm.comportementManager.repulserForce;
        destroyOnUse = _sm.comportementManager.destroyOnUse;
        impulseGradiantForce = _sm.comportementManager.impulseGradiantForce;
        applyOnMe= _sm.comportementManager.applyOnMe;
        feedback = _sm.comportementManager.feedback;
        
        // Debug.LogWarning("NAME PORTEUR SM: " + _sm.gameObject.name);
        Debug.Log("Solo impulse");
        _sm.rend.material = _sm.impulse;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        repulserTimer += Time.deltaTime;
        if (repulserTimer >= repulserTime)
        {
            Expulse();
            repulserTimer = 0;
        }
    }

    public override void TickPhysics()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public void Expulse()
    {
        if (feedback)
        {
            GameObject shockWave = _sm.comportementManager.InstantiateFeedback(feedback, _sm.transform.position, Quaternion.identity);
            shockWave.GetComponent<GrowToRadius>().targetRadius = repulserRange;
        }

        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, repulserRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.gameObject.tag == "Player")
                {
                    GameObject objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>().gameObject;
                    // Debug.Log("objectAffected repulse : " + objectAffected.name);
                    if (impulseGradiantForce)
                    {
                        objectAffected.GetComponent<Rigidbody>().AddExplosionForce(repulserForce, _sm.transform.position, repulserRange);
                    }
                    else
                    {
                        objectAffected.GetComponent<Rigidbody>().AddForce((objectInRange.transform.position - _sm.transform.position) * repulserForce, ForceMode.Impulse);
                    }

                    // player relache l'objet repulse
                    if (objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject)
                    {
                        objectAffected.GetComponent<GrabObject>().Drop(true);
                    }
                }
                if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    if (!applyOnMe && objectInRange.gameObject == _sm.gameObject)
                    {
                        // si rigid body sur objet, on applique pas la force sur lui pour le lancer par exemple
                        return;
                    }

                    if (impulseGradiantForce)
                    {
                        objectInRange.GetComponent<Rigidbody>().AddExplosionForce(repulserForce, _sm.transform.position, repulserRange);

                    }
                    else
                    {
                        objectInRange.GetComponent<Rigidbody>().AddForce((objectInRange.transform.position - _sm.transform.position) * repulserForce, ForceMode.Impulse);
                    }
                }  
            }
        }

        if (destroyOnUse)
        {
            _sm.comportementManager.DestroyObj(_sm.gameObject);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sm.transform.position, repulserRange);
    }
}
