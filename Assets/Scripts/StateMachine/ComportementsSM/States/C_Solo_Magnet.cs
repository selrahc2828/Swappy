using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Magnet : ComportementState
{
    public float magnetRange;
    public float trueMagnetRange;
    public float magnetForce;
    public bool magnetGradiantForce;
    public C_Solo_Magnet(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 27;
        leftValue = 27;
        rightValue = 0;
        base.Enter();

        magnetRange = _sm.comportementManager.magnetRange;
        trueMagnetRange = _sm.GetComponent<Collider>().bounds.extents.magnitude + magnetRange;//toujours des pb de range trop grande car prend pas la scale en compte mais mieux
        magnetForce = _sm.comportementManager.magnetForce;
        magnetGradiantForce = _sm.comportementManager.magnetGradiantForce;
        
        // _sm.rend.material = _sm.magnet;
        ColorShaderOutline(_sm.comportementManager.magnetColor, _sm.comportementManager.noComportementColor);
    }

    public override void TickLogic()
    {
        Attract();
    }

    public override void TickPhysics()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void Attract()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, magnetRange);
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
                    //collider et rigid body pas au même endroit pour lui
                    GameObject objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>().gameObject;
                    
                    // pb pour appliquer la force à cause du drag sur le rigidbody
                    ApplyForce(objectAffected.GetComponent<Rigidbody>(), objectAffected,magnetForce);
                }
                else if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    ApplyForce(objectInRange.GetComponent<Rigidbody>(), objectInRange.gameObject, magnetForce);
                    
                }  
            }
        }
    }
    
    public void ApplyForce(Rigidbody rbObj,GameObject objToApply, float force)
    {
        if (magnetGradiantForce)
        {
            objToApply.GetComponent<Rigidbody>().AddExplosionForce(-magnetForce, _sm.transform.position, trueMagnetRange);
        }
        else
        {
            Vector3 dir = (_sm.transform.position - objToApply.transform.position).normalized; // obj vers magnet
            rbObj.AddForce(dir * force, ForceMode.Impulse);
        }
    }
}
