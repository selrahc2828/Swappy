using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Immuable_Magnet : ComportementState
{
    private Vector3 baseVelocity;
    private Vector3 baseAngularVelocity;
    private float magnetRange;
    private float trueMagnetRange;
    private float magnetForce;
    private bool magnetGradiantForce;

    //private GameObject sonMagnet;
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
        ColorShaderOutline(_sm.comportementManager.immuableColor, _sm.comportementManager.magnetColor);
        feedBack_GO_Right = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Immuable, _sm.transform.position, _sm.transform.rotation, _sm.transform);

        baseVelocity = _sm.rb.velocity;
        baseAngularVelocity = _sm.rb.angularVelocity;
        _sm.rb.isKinematic = true;
        
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
        magnetGradiantForce = _sm.comportementManager.magnetData.magnetGradiantForce;
        
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Immuable, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Right = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Magnet, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Right.GetComponent<GrowToRadius>().targetRadius = trueMagnetRange;
        feedBack_GO_Right.GetComponent<GrowToRadius>().atDestroy = false;
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
        //_sm.comportementManager.DestroyObj(sonMagnet);
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
        _sm.comportementManager.DestroyObj(feedBack_GO_Right);

    }

    public void OncollisionEnter(Collision collision)
    {
        
    }
    public void Attract()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, trueMagnetRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (!objectInRange.gameObject.CompareTag("Player") && objectInRange.gameObject != _sm.gameObject) // applique pas sur player et lui même
                {
                    if (objectInRange.GetComponent<Rigidbody>() != null)
                    {
                        ApplyForce(magnetGradiantForce, objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, magnetForce);
                        
                    }
                }
            }
        }
    }
    
    public void ApplyForce(bool isGradient, Rigidbody rbObj,GameObject objToApply, float force)
    {
        
        if (isGradient)
        {
            objToApply.GetComponent<Rigidbody>().AddExplosionForce(-force, _sm.transform.position, trueMagnetRange);
        }
        else
        {

            Vector3 dir = (_sm.transform.position - objToApply.transform.position).normalized; // obj vers magnet
            rbObj.AddForce(dir * force, ForceMode.Force);
        }
    }
}
