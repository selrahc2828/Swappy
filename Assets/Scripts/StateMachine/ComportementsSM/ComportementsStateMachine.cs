using System.Collections;
using System.Collections.Generic;
using Sound;
using TMPro;
using UnityEngine;

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

[RequireComponent(typeof(Rigidbody))] 
public class ComportementsStateMachine : StateMachine
{
    public C_No_Comportement no_Comportement_state_0;

    public C_Solo_Impulse solo_Impulse_State_1;
    public C_Solo_Bouncing solo_Bouncing_State_3;
    public C_Solo_Immuable solo_Immuable_State_9;
    public C_Solo_Magnet solo_Magnet_State_27;
    public C_Solo_Rocket solo_Rocket_State_81;

    public C_Double_Impulse double_Impulse_State_2;
    public C_Double_Bouncing double_Bouncing_State_6;
    public C_Double_Immuable double_Immuable_State_18;
    public C_Double_Magnet double_Magnet_State_54;
    public C_Double_Rocket double_Rocket_State_162;

    public C_Impulse_Bouncing impulse_Bouncing_State_4;
    public C_Impulse_Immuable impulse_Immuable_State_10;
    public C_Impulse_Magnet impulse_Magnet_State_28;
    public C_Impulse_Rocket impulse_Rocket_State_82;

    public C_Bouncing_Immuable bouncing_Immuable_State_12;
    public C_Bouncing_Magnet bouncing_Magnet_State_30;
    public C_Bouncing_Rocket bouncing_Rocket_State_84;

    public C_Immuable_Magnet immuable_Magnet_State_36;
    public C_Immuable_Rocket immuable_Rocket_State_90;

    public C_Magnet_Rocket magnet_Rocket_State_108;

    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public ComportementManager comportementManager;
    [HideInInspector] public SoundManager soundManager;

    //� voir si on garde �a
    [HideInInspector] public Material bounce;
    [HideInInspector] public Material rocket;
    [HideInInspector] public Material immuable;
    [HideInInspector] public Material rien;
    [HideInInspector] public Material impulse;
    [HideInInspector] public Material magnet;
    [HideInInspector] public MeshRenderer rend;

    [HideInInspector] public GameObject player;
    public bool isPlayer = false;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider objectCollider;
    
    public string displayComportementName;

    public FirstState initialState;
    public override void Initialize()
    {
        no_Comportement_state_0 = new C_No_Comportement(this);

        solo_Impulse_State_1 = new C_Solo_Impulse(this);
        solo_Bouncing_State_3 = new C_Solo_Bouncing(this); 
        solo_Immuable_State_9 = new C_Solo_Immuable(this);
        solo_Magnet_State_27 = new C_Solo_Magnet(this);
        solo_Rocket_State_81 = new C_Solo_Rocket(this);

        double_Impulse_State_2 = new C_Double_Impulse(this);
        double_Bouncing_State_6 = new C_Double_Bouncing(this);
        double_Immuable_State_18 = new C_Double_Immuable(this);
        double_Magnet_State_54 = new C_Double_Magnet(this);
        double_Rocket_State_162 = new C_Double_Rocket(this);

        impulse_Bouncing_State_4 = new C_Impulse_Bouncing(this);
        impulse_Immuable_State_10 = new C_Impulse_Immuable(this);
        impulse_Magnet_State_28 = new C_Impulse_Magnet(this);
        impulse_Rocket_State_82 = new C_Impulse_Rocket(this);

        bouncing_Immuable_State_12 = new C_Bouncing_Immuable(this);
        bouncing_Magnet_State_30 = new C_Bouncing_Magnet(this);
        bouncing_Rocket_State_84 = new C_Bouncing_Rocket(this);

        immuable_Magnet_State_36 = new C_Immuable_Magnet(this);
        immuable_Rocket_State_90 = new C_Immuable_Rocket(this);

        magnet_Rocket_State_108 = new C_Magnet_Rocket(this);

        GoToInitialState(initialState);
        gameManager = GameManager.Instance;
        comportementManager = ComportementManager.Instance;
        soundManager = SoundManager.Instance;
        rend = GetComponentInChildren<MeshRenderer>();//cherche dans lui même et enfant, les prefabs de comportement on le mesh en enfant
        player = gameManager.player;
        rb = GetComponent<Rigidbody>();
        //surtout pour Player qui a 2 collider en enfant
        objectCollider = GetComponentInChildren<Collider>();
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
                currentState = impulse_Bouncing_State_4;
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
