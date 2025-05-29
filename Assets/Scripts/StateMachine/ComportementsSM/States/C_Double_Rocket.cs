using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Rocket : ComportementState
{
    private float rocketForce = 20;
    private float rocketForceOnPlayer = 20;
    private float rocketForceWhenGrab= 20;
    private float onOffCouldown;
    private float onOffFirstCouldown;
    private float timer;
    
    public C_Double_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 162;
        leftValue = 81;
        rightValue = 81;
        base.Enter();

        if (!_sm.isPlayer)
        {
            ColorShaderOutline(_sm.comportementManager.rocketColor, _sm.comportementManager.rocketColor);
        }
        rocketForce = _sm.comportementManager.doubleRocketData.rocketDoubleForce;
        rocketForceOnPlayer = _sm.comportementManager.doubleRocketData.rocketDoubleForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.doubleRocketData.rocketDoubleForceWhenGrab;
        onOffCouldown = _sm.comportementManager.doubleRocketData.rocketDoubleCouldown;
        onOffFirstCouldown = _sm.comportementManager.doubleRocketData.rocketDoubleFirstCouldown;

        timer = 0f;
        //timer += onOffCouldown - onOffFirstCouldown; // a activer pour early start
    }

    public override void TickLogic()
    {
        base.TickLogic();
        
        timer += Time.deltaTime;

        if (timer > onOffCouldown)
        {
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
            timer = 0f;
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForceOnPlayer, ForceMode.Impulse);
            }
            else if(isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(_sm.transform.up * rocketForceWhenGrab, ForceMode.Impulse);
            }
            else
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForce, ForceMode.Impulse);
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
