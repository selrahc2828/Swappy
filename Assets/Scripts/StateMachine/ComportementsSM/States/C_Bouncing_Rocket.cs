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
    private float offCooldown;
    private float timer;
    private float maxSpeed;
    private bool rocketOn;
    private Vector3 rocketDirection;
    public C_Bouncing_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateValue = 84;
        if (_sm.updateRight)  // Si on veut initialiser pour la main droite
        {
            leftValue = 81;
            rightValue = 3;
        }
        else  // Par défaut, initialisation pour la main gauche
        {
            leftValue = 3;
            rightValue = 81;
        }
        // leftValue = 3;
        // rightValue = 81;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.rocketColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);
        feedBack_GO_Right = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

        timer = 0f;
        rocketOn = false;
        rocketDirection = Vector3.up;
        maxSpeed = _sm.comportementManager.rocketData.rocketMaxSpeed;
        rocketForce = _sm.comportementManager.rocketData.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketData.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketData.rocketForceWhenGrab;
        onCooldown = _sm.comportementManager.rocketData.rocketOnCooldown;
        offCooldown = _sm.comportementManager.rocketData.rocketOffCooldown;

        bouncyMaterial = _sm.comportementManager.bounceData.bouncyMaterial;
        if (_sm.isPlayer)
        {
            basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = bouncyMaterial;
        }
        else
        {
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
        if (timer > onCooldown && !rocketOn)
        {
            rocketOn = true;
            rocketDirection = Vector3.up;
            timer = 0f;
        }

        if (timer > offCooldown && rocketOn)
        {
            rocketOn = false;
            timer = 0f;
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

    }
}
