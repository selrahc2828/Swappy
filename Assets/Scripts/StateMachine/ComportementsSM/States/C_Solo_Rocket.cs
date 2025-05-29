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
    public bool rocketOn;
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

        rocketOn = false;
        startingSoonSignalSended = false;
        maxSpeed = _sm.comportementManager.rocketData.rocketMaxSpeed;
        rocketForce = _sm.comportementManager.rocketData.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketData.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketData.rocketForceWhenGrab;
        onCooldown = _sm.comportementManager.rocketData.rocketOnCooldown;
        onFirstCooldown = _sm.comportementManager.rocketData.rocketFirstOnCooldown;
        offCooldown = _sm.comportementManager.rocketData.rocketOffCooldown;

        timer = 0f;
        //timer += onCooldown - onFirstCooldown; // a activer pour early start

        if (!_sm.isPlayer)
        {
            ColorShaderOutline(_sm.comportementManager.rocketColor, _sm.comportementManager.noComportementColor);
        }
    }

    public override void TickLogic()
    {
        base.TickLogic();
    }

    public override void TickPhysics()
    {
        base.TickPhysics();
        timer += Time.fixedDeltaTime;
        if (timer > onCooldown && !rocketOn)// la fusée décole
        {
            rocketOn = true;
            timer = 0f;
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
        }
        if(timer > (onCooldown -1) && !rocketOn && startingSoonSignalSended == false)// envoie du signal, la fusée décole bientot
        {
            startingSoonSignalSended = true;
            GlobalEventManager.Instance.JustBeforeRocketStart(GetGameObject());
        }

        if (timer > offCooldown && rocketOn)// la fusée s'arrete
        {
            rocketOn = false;
            timer = 0f;
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
        }

        if (_sm.transform.InverseTransformDirection(_sm.rb.velocity).y > maxSpeed && rocketOn)// compare la velocity local y a la max speed
        {
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
