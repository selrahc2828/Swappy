using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Impulse_Immuable : ComportementState
{
    private float repulserTime = 5f;
    private float repulserTimer;
    private float repulserRange;
    private float trueRepulserRange;
    private float repulserForce;
    private GameObject feedback;

    private Vector3 _baseVelocity;
    private Vector3 _baseAngularVelocity;
    public C_Impulse_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = true;
        stateValue = 10;
        leftValue = 1;
        rightValue = 9;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.impulseColor, _sm.comportementManager.immuableColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Immuable, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        
        repulserTime = _sm.comportementManager.impulseData.impulseTime;
        repulserTimer = 0;
        repulserRange = _sm.comportementManager.impulseData.impulseRange;

        if (_sm.isPlayer)
        {
            trueRepulserRange = _sm.comportementManager.playerBouncingCollider.bounds.extents.magnitude + repulserRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        }
        else
        {
            trueRepulserRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + repulserRange;
        }

        repulserForce = _sm.comportementManager.impulseData.impulseForce;
        feedback = _sm.comportementManager.impulseData.impulseFeedback;

        _baseVelocity = _sm.rb.velocity;
        _baseAngularVelocity = _sm.rb.angularVelocity;
        _sm.rb.isKinematic = true;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        repulserTimer += Time.deltaTime;
        if (repulserTimer >= repulserTime)
        {
            Repulse();
            repulserTimer = 0;
        }
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
    }

    public override void Exit()
    {
        base.Exit();
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);

        _sm.rb.isKinematic = false;
        _sm.rb.velocity = _baseVelocity;
        _sm.rb.angularVelocity = _baseAngularVelocity;
    }

    public void Repulse()
    {
        if (feedback)
        {
            GameObject shockWave = _sm.comportementManager.InstantiateFeedback(feedback, _sm.transform.position, Quaternion.identity);
            shockWave.GetComponent<GrowToRadius>().targetRadius = trueRepulserRange;
        }
        SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.repulseBoom,_sm.gameObject);
        SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.immuableHit,_sm.gameObject);
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueRepulserRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.gameObject.tag == "Player")
                {
                    if (!objectInRange.gameObject.GetComponentInParent<Rigidbody>())
                    {
                        return;
                    }
                    //collider et rigid body pas au m�me endroit pour lui
                    GameObject objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>().gameObject;

                    // pb pour appliquer la force � cause du drag sur le rigidbody
                    ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected, repulserForce);

                    // player relache l'objet repulse
                    if (isGrabbed) //juste isGrabbed ? objectAffected.GetComponent<GrabObject>().carriedObject == _sm.gameObject
                    {
                        objectAffected.GetComponent<GrabObject>().Drop(true);
                    }
                }
                else if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, repulserForce);
                }
            }
        }
    }

    public void ApplyForce(Rigidbody rbObj, GameObject objToApply, float force)
    {
        Vector3 direction = (objToApply.transform.position - _sm.transform.position).normalized;
        rbObj.AddForce(direction * force, ForceMode.Impulse);
    }
}
