using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Immuable_Rocket : ComportementState
{
    public float chargeTimeMax;
    public float chargeTime;
    public float rocketReleaseForce;

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

        rocketReleaseForce = _sm.comportementManager.rocketReleaseForce;
        chargeTimeMax = _sm.comportementManager.chargeTimeMax;
        chargeTime = 0;
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

        _sm.rb.isKinematic = false;
        float effectiveReleaseForce = rocketReleaseForce * (chargeTime / chargeTimeMax);
        _sm.rb.AddForce(Vector3.up * effectiveReleaseForce, ForceMode.Impulse);
    }
}
