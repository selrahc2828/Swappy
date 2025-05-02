using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementState : State
{
    public int stateValue;
    public int leftValue;
    public int rightValue;
    public bool isGrabbed = false;
    public bool isKinematic = false;

    public GameObject feedBack_GO_Left;
    public GameObject feedBack_GO_Right;

    protected ComportementsStateMachine _sm;
    public ComportementsStateMachine smGet => _sm; // get
    // public ComportementsStateMachine smGet
    // {
    //     get { return _sm; }
    // }

    public ComportementState(StateMachine stateMachine) : base(stateMachine)
    {
        _sm = ((ComportementsStateMachine)_stateMachine);
    }

    public override void Enter()
    {
        GlobalEventManager.Instance.ComportmentStateEnter(_sm.gameObject);
    }

    public override void TickLogic()
    {
        // Debug.Log(_sm.currentState + " logic uppdate");
        
    }

    public override void TickPhysics()
    {
        // Debug.Log(_sm.currentState + " physic update");
        
        // if (!isKinematic)
        // {
        //     _sm.rb.isKinematic = isGrabbed;
        // }
    }

    public override void Exit()
    {
        GlobalEventManager.Instance.ComportmentStateExit(_sm.gameObject);
        // Debug.Log(_sm.currentState + " exit");

        // if (feedBack_GO_Left != null)
        // {
        //     _sm.comportementManager.DestroyObj(feedBack_GO_Left);
        // }
        //
        // if (feedBack_GO_Right != null)
        // {
        //     _sm.comportementManager.DestroyObj(feedBack_GO_Right);
        // }
    }

    public override void CollisionStart(Collision other)
    {
        // Debug.Log($"{_sm.name } collision start/enter with {other.collider.name}");
    }

    public override void CollisionDuring(Collision other)
    {
    }

    public override void CollisionEnd(Collision other)
    {
    }

    public override void DisplayGizmos()
    {
    }

    public void GetThrown(Vector3 thrownDirection)
    {
        _sm.rb.AddForce(thrownDirection, ForceMode.Impulse);
    }
    public GameObject GetGameObject()
    {
        return _sm.gameObject;
    }

    public void CalculateNewtState(int newValue)
    {
        switch (newValue)
        {
            case 0:
                _sm.ChangeState(_sm.no_Comportement_state_0);
                break;
            case 1:
                _sm.ChangeState(_sm.solo_Impulse_State_1);
                break;
            case 2:
                _sm.ChangeState(_sm.double_Impulse_State_2);
                break;
            case 3:
                _sm.ChangeState(_sm.solo_Bouncing_State_3);
                break;
            case 4:
                _sm.ChangeState(_sm.impulse_Bouncing_State_4);
                break;
            case 6:
                _sm.ChangeState(_sm.double_Bouncing_State_6);
                break;
            case 9:
                _sm.ChangeState(_sm.solo_Immuable_State_9);
                break;
            case 10:
                _sm.ChangeState(_sm.impulse_Immuable_State_10);
                break;
            case 12:
                _sm.ChangeState(_sm.bouncing_Immuable_State_12);
                break;
            case 18:
                _sm.ChangeState(_sm.double_Immuable_State_18);
                break;
            case 27:
                _sm.ChangeState(_sm.solo_Magnet_State_27);
                break;
            case 28:
                _sm.ChangeState(_sm.impulse_Magnet_State_28);
                break;
            case 30:
                _sm.ChangeState(_sm.bouncing_Magnet_State_30);
                break;
            case 36:
                _sm.ChangeState(_sm.immuable_Magnet_State_36);
                break;
            case 54:
                _sm.ChangeState(_sm.double_Magnet_State_54);
                break;
            case 81:
                _sm.ChangeState(_sm.solo_Rocket_State_81);
                break;
            case 82:
                _sm.ChangeState(_sm.impulse_Rocket_State_82);
                break;
            case 84:
                _sm.ChangeState(_sm.bouncing_Rocket_State_84);
                break;
            case 90:
                _sm.ChangeState(_sm.immuable_Rocket_State_90);
                break;
            case 108:
                _sm.ChangeState(_sm.magnet_Rocket_State_108);
                break;
            case 162:
                _sm.ChangeState(_sm.double_Rocket_State_162);
                break;
            default:
                _sm.ChangeState(_sm.no_Comportement_state_0);
                break;
        }
    }

    public void ColorShaderOutline(Color colorSlot1, Color colorSlot2)
    {
        if (colorSlot1 == null)
        {
            colorSlot1 = _sm.comportementManager.noComportementColor;
        }
        if (colorSlot2 == null)
        {
            colorSlot2 = _sm.comportementManager.noComportementColor;
        }

        // Debug.Log($"Change color {_sm.name} into slot 1: {colorSlot1} and slot2: {colorSlot2}");
        if (_sm.rend.materials.Length >1)//verif si on a plus de 2 materials
        {
            // on part du principe que le mat index 1 est le shader outline
            _sm.rend.materials[1].SetColor("_Color2", colorSlot1);
            _sm.rend.materials[1].SetColor("_Color3", colorSlot2);
        }
    }

    public GameObject SetFeedbackComportement(int intSlot)
    {
        switch (intSlot)
        {
            case 1:
                return _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Impulse, _sm.transform.position, _sm.transform.rotation, _sm.transform);
            case 3:
                return _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Bouncing, _sm.transform.position, _sm.transform.rotation, _sm.transform);
            case 9:
                return _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Immuable, _sm.transform.position, _sm.transform.rotation, _sm.transform);
            case 27:
                return _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Magnet, _sm.transform.position, _sm.transform.rotation, _sm.transform);
            case 81:
                return _sm.comportementManager.InstantiateFeedback(_sm.comportementManager.feedBack_Rocket, _sm.transform.position, _sm.transform.rotation, _sm.transform);            
            default:
                return null;
        }
    }
}