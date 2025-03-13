using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Solo_Rocket : ComportementState
{
    private float rocketForce = 20;
    private float rocketForceOnPlayer = 20;
    private float rocketForceWhenGrab= 20;
    private float onOffCooldown;
    private float timer;
    private float maxSpeed;
    private bool rocketOn;
    
    
    //private bool isSoundPlay;

    public C_Solo_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 81;
        leftValue = 81;
        rightValue = 0;
        base.Enter();

        timer = 0f;
        rocketOn = false;
        maxSpeed = _sm.comportementManager.rocketData.rocketMaxSpeed;
        rocketForce = _sm.comportementManager.rocketData.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketData.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketData.rocketForceWhenGrab;
        onOffCooldown = _sm.comportementManager.rocketData.rocketOnOffCouldown;
        
        // _sm.rend.material = _sm.rocket;
        ColorShaderOutline(_sm.comportementManager.rocketColor, _sm.comportementManager.noComportementColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

    }

    public override void TickLogic()
    {
        base.TickLogic();
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
        timer += Time.fixedDeltaTime;
        if (timer > onOffCooldown)
        {
            
            rocketOn = !rocketOn;
            timer = 0f;
        }
        
        if (_sm.rb.velocity.magnitude > maxSpeed)
        {
            _sm.rb.velocity = _sm.rb.velocity.normalized * maxSpeed;
        }
        
        if (rocketOn)
        {
            //FMODEventManager.instance.PlayOneShotAttached(FMODEventManager.instance.FMODEvents.RocketStart,_sm.gameObject);
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForceOnPlayer, ForceMode.Force);
            }
            else if(isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(_sm.transform.up * rocketForceWhenGrab, ForceMode.Force);
            }
            else
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForce, ForceMode.Force);
            }
        }
        else
        {
            //isSoundPlay = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
    }
}
