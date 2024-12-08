using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Magnet : ComportementState
{
    public float magnetRange;
    public float magnetForce;
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
        magnetForce = _sm.comportementManager.magnetForce;
        
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
        Collider[] objectsInRange = Physics.OverlapSphere(_sm.transform.position, _sm.comportementManager.magnetRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                // if (_sm.isPlayer && objectInRange.CompareTag("Player"))
                // {
                //     return; //pour le moment si on la sur nous on est pas affecté
                // }
                
                if (objectInRange.GetComponent<Rigidbody>() != null)
                {

                    Vector3 dir = _sm.transform.position - objectInRange.transform.position; // obj vers magnet
                    if (_sm.comportementManager.magnetGradiantForce)
                    {
                        // -magnetForce 
                        objectInRange.GetComponent<Rigidbody>().AddExplosionForce(-_sm.comportementManager.magnetForce, _sm.transform.position, _sm.comportementManager.magnetRange);
                    }
                    else
                    {
                        objectInRange.GetComponent<Rigidbody>().AddForce( dir * _sm.comportementManager.magnetForce, ForceMode.Impulse);
                    }
                }  
            }
        }
    }
}
