using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class C_Solo_Rocket : ComportementState
{
    private float rocketForce = 20;
    private float rocketForceOnPlayer = 20;
    private float rocketForceWhenGrab= 20;
    private float onCooldown;
    private float onFirstCooldown;
    private float offCooldown;
    private float timer;
    private float maxSpeed;
    private bool rocketOn;
    private bool firstRocket;
    private bool startingSoonSignalSended;


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
        firstRocket = true;
        startingSoonSignalSended = false;
        maxSpeed = _sm.comportementManager.rocketData.rocketMaxSpeed;
        rocketForce = _sm.comportementManager.rocketData.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketData.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketData.rocketForceWhenGrab;
        onCooldown = _sm.comportementManager.rocketData.rocketOnCooldown;
        onFirstCooldown = _sm.comportementManager.rocketData.rocketFirstOnCooldown;
        offCooldown = _sm.comportementManager.rocketData.rocketOffCooldown;

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
        if (firstRocket)
        {
            timer += onCooldown - onFirstCooldown;
            firstRocket = false;
        }
        if (timer > onCooldown && !rocketOn)
        {
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
            rocketOn = true;
            timer = 0f;
        }
        if(timer >  (onCooldown -1) && !rocketOn && startingSoonSignalSended == false)
        {
            startingSoonSignalSended = true;
            GlobalEventManager.Instance.JustBeforeRocketStart(GetGameObject());
        }

        if (timer > offCooldown && rocketOn)
        {
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
            rocketOn = false;
            timer = 0f;
        }

        if (_sm.transform.InverseTransformDirection(_sm.rb.velocity).y > maxSpeed && rocketOn)// compare la velocity local y a la max speed
        {
            //_sm.rb.velocity = _sm.rb.velocity.normalized * maxSpeed;
            return;
        }

        if (rocketOn)
        {
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForceOnPlayer, ForceMode.Force);
            }
            else if (isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>()
                    .AddForce(_sm.transform.up * rocketForceWhenGrab, ForceMode.Acceleration);
            }
            else
            {
                _sm.rb.AddForce(_sm.transform.up * rocketForce, ForceMode.Force);
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);

    }
}
