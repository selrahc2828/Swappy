using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class C_Bouncing_Immuable : ComportementState
{
    private PhysicMaterial _bouncyMaterial;
    private PhysicMaterial _basePlayerMaterial;
    private PhysicMaterial _basePlayerSlideMaterial;
    
    private Vector3 _baseVelocity;
    private Vector3 _baseAngularVelocity;
    
   
    
    public C_Bouncing_Immuable(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isKinematic = false;
        stateValue = 12;
        leftValue = 3;
        rightValue = 9;
        base.Enter();
       
        _bouncyMaterial = _sm.comportementManager.bounceData.bouncyMaterial;

        if (_sm.isPlayer)
        {
            _basePlayerMaterial = _sm.comportementManager.playerBouncingCollider.material;
            _sm.comportementManager.playerBouncingCollider.material = _bouncyMaterial;
            
            _baseVelocity = _sm.rb.velocity;
            _baseAngularVelocity = _sm.rb.angularVelocity;
            _sm.rb.isKinematic = true;
        }
        else
        {
            ColorShaderOutline(_sm.comportementManager.bouncingColor, _sm.comportementManager.immuableColor);
        }
    }

    public override void TickLogic()
    {
        base.TickLogic();
    }

    public override void TickPhysics()
    {
        if (isGrabbed)
        {
            _sm.GetComponent<Collider>().material = null;
            
            _sm.rb.isKinematic = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        if (_sm.isPlayer)
        {
            _sm.comportementManager.playerBouncingCollider.material = _basePlayerMaterial;
            
            _baseVelocity = _sm.rb.velocity;
            _baseAngularVelocity = _sm.rb.angularVelocity;
            _sm.rb.isKinematic = true;
        }
        else
        {
            _sm.GetComponent<Collider>().material = null;
            
            _sm.rb.isKinematic = false;
            _sm.rb.velocity = _baseVelocity;
            _sm.rb.angularVelocity = _baseAngularVelocity;
        }
    }

    public override void CollisionStart(Collision other)
    {
        base.CollisionStart(other);
        if (!_sm.isPlayer)
        {
            if (!_sm.rb.isKinematic)
            {
                if (other.gameObject.GetComponent<Rigidbody>() != null)
                {
                    other.gameObject.GetComponent<Rigidbody>().velocity = _sm.rb.velocity;
                }
                _baseVelocity = _sm.rb.velocity;
                _baseAngularVelocity = _sm.rb.angularVelocity;
                _sm.rb.isKinematic = true;
                GlobalEventManager.Instance.ComportmentStatePlay(_sm.gameObject);
                GlobalEventManager.Instance.BounceLocation(other.contacts);

                _sm.GetComponent<Collider>().material = _bouncyMaterial;
            }
        }
    }
}
