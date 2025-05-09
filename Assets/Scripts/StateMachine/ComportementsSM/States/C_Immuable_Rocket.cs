using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Immuable_Rocket : ComportementState
{
    private float chargeTimeMax;
    private float chargeTime;
    private float rocketReleaseForce;

    public C_Immuable_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        isKinematic = true;
        stateValue = 90;
        leftValue = 9;
        rightValue = 81;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.immuableColor, _sm.comportementManager.rocketColor);
        _sm.rb.isKinematic = true;

        rocketReleaseForce = _sm.comportementManager.immuableRocketData.rocketReleaseForce;
        chargeTimeMax = _sm.comportementManager.immuableRocketData.chargeTimeMax;
        chargeTime = 0;
        
        feedBack_GO_Right = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Immuable, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

    }

    public override void TickLogic()
    {
        base.TickLogic();
        if(chargeTime < chargeTimeMax)
        {
            chargeTime += Time.deltaTime;
        }
        else
        {
            chargeTime = chargeTimeMax;
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
        _sm.comportementManager.DestroyObj(feedBack_GO_Right);

        _sm.rb.isKinematic = false;
        float effectiveReleaseForce = rocketReleaseForce * (chargeTime / chargeTimeMax);
        
        _sm.rb.AddForce(_sm.transform.up * effectiveReleaseForce, ForceMode.Impulse);
    }
}
