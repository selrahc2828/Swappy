using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Double_Rocket : ComportementState
{
    private float rocketForce = 20;
    private float rocketForceOnPlayer = 20;
    private float rocketForceWhenGrab= 20;
    private float onOffCouldown;
    private float timer;
    private bool isSonOn;
    
    public C_Double_Rocket(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        stateValue = 162;
        leftValue = 81;
        rightValue = 81;
        base.Enter();
        ColorShaderOutline(_sm.comportementManager.rocketColor, _sm.comportementManager.rocketColor);
        feedBack_GO_Left = _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);

        timer = 0f;
        rocketForce = _sm.comportementManager.rocketDoubleForce;
        rocketForceOnPlayer = _sm.comportementManager.rocketDoubleForceOnPlayer;
        rocketForceWhenGrab = _sm.comportementManager.rocketDoubleForceWhenGrab;
        onOffCouldown = _sm.comportementManager.rocketDoubleCouldown;
    }

    public override void TickLogic()
    {
        base.TickLogic();
        
        timer += Time.deltaTime;
        if (timer > onOffCouldown)
        {

            timer = 0f;
            SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.propelerStart,_sm.gameObject);
            SoundManager.Instance.PlaySoundComponenent(SoundManager.SoundComp.propelerStart,_sm.gameObject);
            if (_sm.isPlayer)
            {
                _sm.rb.AddForce(Vector3.up * rocketForceOnPlayer, ForceMode.Impulse);
            }
            else if(isGrabbed)
            {
                _sm.gameManager.player.GetComponent<Rigidbody>().AddForce(Vector3.up * rocketForceWhenGrab, ForceMode.Impulse);
            }
            else
            {
                _sm.rb.AddForce(Vector3.up * rocketForce, ForceMode.Impulse);
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
        _sm.comportementManager.DestroyObj(feedBack_GO_Left);

    }
}
