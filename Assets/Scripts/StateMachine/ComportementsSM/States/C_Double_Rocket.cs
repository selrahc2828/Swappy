using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Rocket : ComportementState
{
    public float rocketForce = 20;
    public float rocketForceOnPlayer = 20;
    public float rocketForceWhenGrab= 20;
    public float onOffCouldown;
    public float timer;
    
    public C_Double_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        stateValue = 162;
        leftValue = 81;
        rightValue = 81;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.rocketColor, _sm.comportementManager.rocketColor);

        timer = 0f;
        rocketForce = _sm.comportementManager.rocketDoubleForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketDoubleForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketDoubleForceWhenGrab;
        onOffCouldown = _sm.comportementManager.rocketDoubleCouldown;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        
        timer += Time.deltaTime;
        if (timer > onOffCouldown)
        {
            timer = 0f;
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(Vector3.up * rocketForceOnPlayer, ForceMode.Impulse);
            }
            else if(isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(Vector3.up * rocketForceWhenGrab, ForceMode.Impulse);
            }
            else
            {
                _sm.rb.AddForce(Vector3.up * rocketForce, ForceMode.Impulse);
            }
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
