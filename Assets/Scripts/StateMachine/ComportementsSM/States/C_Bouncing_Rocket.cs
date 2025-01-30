using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Bouncing_Rocket : ComportementState
{
    private PhysicMaterial bouncyMaterial;
    private PhysicMaterial basePlayerMaterial;
    private PhysicMaterial basePlayerSlideMaterial;
    private float rocketForce = 20;
    private float rocketForceOnPlayer = 20;
    private float rocketForceWhenGrab= 20;
    private float onOffCooldown;
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
        leftValue = 3;
        rightValue = 81;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.rocketColor);
        
        timer = 0f;
        rocketOn = false;
        rocketDirection = Vector3.up;
        maxSpeed = _sm.comportementManager.rocketMaxSpeed;
        rocketForce = _sm.comportementManager.rocketForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketForceWhenGrab;
        onOffCooldown = _sm.comportementManager.rocketOnOffCouldown;
        
        bouncyMaterial = _sm.comportementManager.bouncyMaterial;
        if (_sm.isPlayer)
        {
            basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            basePlayerSlideMaterial = _sm.comportementManager.playerSlidingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = bouncyMaterial;
            _sm.comportementManager.playerSlidingCollider.material = bouncyMaterial;
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
        if (timer > onOffCooldown)
        {
            rocketOn = !rocketOn;
            rocketDirection = Vector3.up;
            timer = 0f;
        }
        
        if (_sm.rb.velocity.magnitude > maxSpeed)
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
                _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(rocketDirection * rocketForceWhenGrab, ForceMode.Force);
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
            _sm.comportementManager.playerSlidingCollider.material = basePlayerSlideMaterial;
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
        }
    }

    public override void CollisionStart(Collision other)
    {
        base.CollisionStart(other);
        rocketDirection = -rocketDirection;
        SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.bounceHit,_sm.gameObject);
        SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.propelerStart,_sm.gameObject);
    }
}
