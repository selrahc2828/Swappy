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
    public bool isOnPlayer = false;

    protected ComportementsStateMachine _sm;

    public ComportementState(StateMachine stateMachine) : base(stateMachine)
    {
        _sm = ((ComportementsStateMachine)_stateMachine);
    }

    public override void Enter()
    {
        Debug.Log(_sm.currentState + " enter " + stateValue);
    }

    public override void TickLogic()
    {
        Debug.Log(_sm.currentState + " logic uppdate");
    }

    public override void TickPhysics()
    {
        Debug.Log(_sm.currentState + " physic update");
    }

    public override void Exit()
    {
        Debug.Log(_sm.currentState + " exit");
    }

    public void CalculateNewtState(int newValue)
    {
        switch (newValue)
        {
            case 0:
                _sm.ChangeState(ComportementsStateMachine.no_Comportement_state_0);
                break;
            case 1:
                _sm.ChangeState(ComportementsStateMachine.solo_Impulse_State_1);
                break;
            case 2:
                _sm.ChangeState(ComportementsStateMachine.double_Impulse_State_2);
                break;
            case 3:
                _sm.ChangeState(ComportementsStateMachine.solo_Bouncing_State_3);
                break;
            case 4:
                _sm.ChangeState(ComportementsStateMachine.impulse_Bouncing_State_4);
                break;
            case 6:
                _sm.ChangeState(ComportementsStateMachine.double_Bouncing_State_6);
                break;
            case 9:
                _sm.ChangeState(ComportementsStateMachine.solo_Immuable_State_9);
                break;
            case 10:
                _sm.ChangeState(ComportementsStateMachine.impulse_Immuable_State_10);
                break;
            case 12:
                _sm.ChangeState(ComportementsStateMachine.bouncing_Immuable_State_12);
                break;
            case 18:
                _sm.ChangeState(ComportementsStateMachine.double_Immuable_State_18);
                break;
            case 27:
                _sm.ChangeState(ComportementsStateMachine.solo_Magnet_State_27);
                break;
            case 28:
                _sm.ChangeState(ComportementsStateMachine.impulse_Magnet_State_28);
                break;
            case 30:
                _sm.ChangeState(ComportementsStateMachine.bouncing_Magnet_State_30);
                break;
            case 36:
                _sm.ChangeState(ComportementsStateMachine.immuable_Magnet_State_36);
                break;
            case 54:
                _sm.ChangeState(ComportementsStateMachine.double_Magnet_State_54);
                break;
            case 81:
                _sm.ChangeState(ComportementsStateMachine.solo_Rocket_State_81);
                break;
            case 82:
                _sm.ChangeState(ComportementsStateMachine.impulse_Rocket_State_82);
                break;
            case 84:
                _sm.ChangeState(ComportementsStateMachine.bouncing_Rocket_State_84);
                break;
            case 90:
                _sm.ChangeState(ComportementsStateMachine.immuable_Rocket_State_90);
                break;
            case 108:
                _sm.ChangeState(ComportementsStateMachine.magnet_Rocket_State_108);
                break;
            case 162:
                _sm.ChangeState(ComportementsStateMachine.double_Rocket_State_162);
                break;
            default:
                _sm.ChangeState(ComportementsStateMachine.no_Comportement_state_0);
                break;
        }
    }


}
