using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementsStateMachine : StateMachine
{
    public static No_Comportement no_Comportement_state_0;

    public static Solo_Impulse solo_Impulse_State_1;
    public static Solo_Bouncing solo_Bouncing_State_3;
    public static Solo_Immuable solo_Immuable_State_9;
    public static Solo_Magnet solo_Magnet_State_27;
    public static Solo_Rocket solo_Rocket_State_81;

    public static Double_Impulse double_Impulse_State_2;
    public static Double_Bouncing double_Bouncing_State_6;
    public static Double_Immuable double_Immuable_State_18;
    public static Double_Magnet double_Magnet_State_54;
    public static Double_Rocket double_Rocket_State_162;

    public static Impulse_Bouncing impulse_Bouncing_State_4;
    public static Impulse_Immuable impulse_Immuable_State_10;
    public static Impulse_Magnet impulse_Magnet_State_28;
    public static Impulse_Rocket impulse_Rocket_State_82;

    public static Bouncing_Immuable bouncing_Immuable_State_12;
    public static Bouncing_Magnet bouncing_Magnet_State_30;
    public static Bouncing_Rocket bouncing_Rocket_State_84;

    public static Immuable_Magnet immuable_Magnet_State_36;
    public static Immuable_Rocket immuable_Rocket_State_90;

    public static Magnet_Rocket magnet_Rocket_State_108;

    public GameManager gameManager;
    public enum FirstState
    {
        NoComportement = 0,
        SoloImpulse = 1,
        SoloBouncing = 3,
        SoloImmuable = 9,
        SoloMagnet = 27,
        SoloRocket = 81,
        DoubleImpulse = 2,
        DoubleBounce = 6,
        DoubleImmuable = 18,
        DoubleMagnet = 54,
        DoubleRocket = 162,
        ImpulseBouncing = 4,
        ImpulseImmuable = 10,
        ImpulseMagnet = 28,
        ImpulseRocket = 82,
        BouncingImmuable = 12,
        BouncingMagnet = 30,
        BouncingRocket = 84,
        ImmuableMagnet = 36,
        ImmuableRocket = 90,
        MagnetRocket = 108
    }

    public FirstState initialState;
    public override void Initialize()
    {
        no_Comportement_state_0 = new No_Comportement(this);

        solo_Impulse_State_1 = new Solo_Impulse(this);
        solo_Bouncing_State_3 = new Solo_Bouncing(this); 
        solo_Immuable_State_9 = new Solo_Immuable(this);
        solo_Magnet_State_27 = new Solo_Magnet(this);
        solo_Rocket_State_81 = new Solo_Rocket(this);

        double_Impulse_State_2 = new Double_Impulse(this);
        double_Bouncing_State_6 = new Double_Bouncing(this);
        double_Immuable_State_18 = new Double_Immuable(this);
        double_Magnet_State_54 = new Double_Magnet(this);
        double_Rocket_State_162 = new Double_Rocket(this);

        impulse_Bouncing_State_4 = new Impulse_Bouncing(this);
        impulse_Immuable_State_10 = new Impulse_Immuable(this);
        impulse_Magnet_State_28 = new Impulse_Magnet(this);
        impulse_Rocket_State_82 = new Impulse_Rocket(this);

        bouncing_Immuable_State_12 = new Bouncing_Immuable(this);
        bouncing_Magnet_State_30 = new Bouncing_Magnet(this);
        bouncing_Rocket_State_84 = new Bouncing_Rocket(this);

        immuable_Magnet_State_36 = new Immuable_Magnet(this);
        immuable_Rocket_State_90 = new Immuable_Rocket(this);

        magnet_Rocket_State_108 = new Magnet_Rocket(this);

        //currentState = no_Comportement_state_0;
        GoToInitialState(initialState);
        gameManager = FindAnyObjectByType<GameManager>();
        currentState.Enter();
    }
    
    public void GoToInitialState(FirstState newValue)
    {
        switch ((int)newValue)
        {
            case 0:
                currentState = no_Comportement_state_0;
                break;
            case 1:
                currentState = solo_Impulse_State_1;
                break;
            case 2:
                currentState = double_Impulse_State_2;
                break;
            case 3:
                currentState = solo_Bouncing_State_3;
                break;
            case 4:
                currentState = impulse_Bouncing_State_4         ;
                break;
            case 6:
                currentState = double_Bouncing_State_6;
                break;
            case 9:
                currentState = solo_Immuable_State_9;
                break;
            case 10:
                currentState = impulse_Immuable_State_10;
                break;
            case 12:
                currentState = bouncing_Immuable_State_12;
                break;
            case 18:
                currentState = double_Immuable_State_18;
                break;
            case 27:
                currentState = solo_Magnet_State_27;
                break;
            case 28:
                currentState = impulse_Magnet_State_28;
                break;
            case 30:
                currentState = bouncing_Magnet_State_30;
                break;
            case 36:
                currentState = immuable_Magnet_State_36;
                break;
            case 54:
                currentState = double_Magnet_State_54;
                break;
            case 81:
                currentState = solo_Rocket_State_81;
                break;
            case 82:
                currentState = impulse_Rocket_State_82;
                break;
            case 84:
                currentState = bouncing_Rocket_State_84;
                break;
            case 90:
                currentState = immuable_Rocket_State_90;
                break;
            case 108:
                currentState = magnet_Rocket_State_108;
                break;
            case 162:
                currentState = double_Rocket_State_162;
                break;
            default:
                currentState = no_Comportement_state_0;
                break;
        }
    }
}
