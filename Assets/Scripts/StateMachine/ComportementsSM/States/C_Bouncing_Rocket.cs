using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Bouncing_Rocket : ComportementState
{
    private PhysicMaterial bouncyMaterial;
    private PhysicMaterial basePlayerMaterial;
    private PhysicMaterial basePlayerSlideMaterial;
    private float rocketForce = 20;
    private float rocketForceOnPlayer = 20;
    private float rocketForceWhenGrab= 20;
    private float onCooldown;
    private float onFirstCooldown;
    private float offCooldown;
    private float timer;
    private float maxSpeed;
    public bool rocketOn;
    private Vector3 rocketDirection;
    public C_Bouncing_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 84;
        leftValue = 3;
        rightValue = 81;
        base.Enter();
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Right = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

        rocketOn = false;
        rocketDirection = _sm.transform.up;
        maxSpeed = _sm.comportementManager.rocketData.rocketMaxSpeed;
        rocketForce = _sm.comportementManager.rocketData.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketData.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketData.rocketForceWhenGrab;
        onCooldown = _sm.comportementManager.rocketData.rocketOnCooldown;
        onFirstCooldown = _sm.comportementManager.rocketData.rocketFirstOnCooldown;
        offCooldown = _sm.comportementManager.rocketData.rocketOffCooldown;

        timer = 0f;
        //timer += onCooldown - onFirstCooldown; // a activer pour early start

        bouncyMaterial = _sm.comportementManager.bounceData.bouncyMaterial;
        if (_sm.isPlayer)
        {
            basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = bouncyMaterial;
        }
        else
        {
            ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.rocketColor);
            _sm.GetComponent<Collider>().material = bouncyMaterial;
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
        if (timer > onCooldown && !rocketOn)//la fusée décolle
        {
            rocketOn = true;
            rocketDirection = _sm.transform.up;
            timer = 0f;
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
        }

        if (timer > offCooldown && rocketOn)// la fusée s'arrette
        {
            rocketOn = false;
            timer = 0f;
            GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
        }

        if (_sm.rb.velocity.magnitude > maxSpeed && rocketOn)
        {
            _sm.rb.velocity = _sm.rb.velocity.normalized * maxSpeed;
        }
        
        if (rocketOn)
        {
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(rocketDirection * rocketForceOnPlayer, ForceMode.Force);
            }
            else if(isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(rocketDirection * rocketForceWhenGrab, ForceMode.Acceleration);
            }
            else
            {
                _sm.rb.AddForce(rocketDirection * rocketForce, ForceMode.Force);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (_sm.isPlayer)
        {
            _sm.comportementManager.playerBouncingCollider.material = basePlayerMaterial;
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
        }
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);
        _sm.comportementManager.DestroyObj(feedBack_GO_Right);

    }

    public override void CollisionStart(Collision other)
    {
        base.CollisionStart(other);
        rocketDirection = -rocketDirection;
        GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);

    }
}
