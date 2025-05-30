using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    public ComportementsStateMachine playerStateMachine;

    private GameObject SpawnPoint;
    private CubeTagFeedbackToDestroy[] feedbacksToDestroy;
    private GameObject rocketTrail;
    
    [Header("SIM")]
    public GameObject ImpulseSIMGauche;
    public GameObject ImpulseSIMDroite;
    
    public GameObject BounceSIMGauche;
    public GameObject BounceSIMDroite;
    
    public GameObject ImmuSIMGauche;
    public GameObject ImmuSIMDroite;
    
    public GameObject MagnetSIMGauche;
    public GameObject MagnetSIMDroite;
    
    public GameObject RocketSIMGauche;
    public GameObject RocketSIMDroite;
    
    
    [Header("Impulse")] 
    public GameObject ImpulseConstantFeedback;
    public GameObject ImpulseTimerFeedback;
    public GameObject ImpulseImpactFeedback;
    

    [Header("Bounce")] 
    public GameObject BounceConstantFeedback;
    public GameObject BounceActiveFeedback;

    [Header("Immuable")] 
    public GameObject ImmuConstantFeedback;

    [Header("Magnet")] 
    public GameObject MagnetConstantFeedback;

    [Header("Rocket")] 
    public GameObject RocketConstantFeedback;
    public GameObject RocketImpact;
    public GameObject RocketOnFeedback;

    private void OnEnable()
    {
        GlobalEventManager.Instance.OnComportementStateEnter += FeedbackEnterState;
        GlobalEventManager.Instance.OnComportementStatePlay += FeedbackPlayState;
        GlobalEventManager.Instance.OnComportementStateExit += FeedbackExitState;
        GlobalEventManager.Instance.OnExplosion += FeedbackExplosion;
        GlobalEventManager.Instance.OnJustBeforeExplosion += FeedbackJustBeforeExplosion;
        GlobalEventManager.Instance.OnBounceLocation += FeedbackBounceLocation;
        GlobalEventManager.Instance.OnJustBeforeRocketStart += FeedbackJustBeforeRocketStart;
    }

    public void FeedbackEnterState(GameObject sender,bool haveEarlyStartUSELESSHERE, float defaultValueBetweenHitUSELESSHERE, float startingValueBetweenHitUSELESSHERE)
    {
        int stateValue = 0;
        ComportementsStateMachine stateMachine = sender.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
            ComportementState currentObjectState = (ComportementState)stateMachine.currentState;
            stateValue = currentObjectState.stateValue;
        }
        
        if(!sender.CompareTag("Player"))
        {
            SpawnPoint = sender.GetComponentInChildren<CubeTagFeedback>().gameObject;
        }
        
        switch (stateValue)
        {
            case 0: // pas de comportement
                if (sender.CompareTag("Player"))
                {
                    ImpulseSIMDroite.SetActive(false);
                    BounceSIMDroite.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                        
                    ImpulseSIMGauche.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }
                else
                {
                    ImpulseSIMDroite.SetActive(false);
                    BounceSIMDroite.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                        
                    ImpulseSIMGauche.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }

                break;
            case 1: // solo impulse
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMGauche.SetActive(true);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                        
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImpulseConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 3: // solo bouncing
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(true);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                        
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(BounceConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 9: // solo immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(true);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                        
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImmuConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 27: // solo magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(true);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);

                    }
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(true);
                        RocketSIMGauche.SetActive(false);
                        
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(MagnetConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 81: // solo rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(true);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(true);
                        
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(RocketConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 2: // double impulse
                if (sender.CompareTag("Player"))
                {
                    ImpulseSIMDroite.SetActive(true);
                    ImpulseSIMGauche.SetActive(true);
                    
                    BounceSIMDroite.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                    
                    BounceSIMGauche.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }
                else
                {
                    Instantiate(ImpulseConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 6: // double bouncing
                if (sender.CompareTag("Player"))
                {
                    BounceSIMDroite.SetActive(true);
                    BounceSIMGauche.SetActive(true);
                    
                    ImpulseSIMDroite.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                    
                    ImpulseSIMGauche.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }
                else
                {
                    Instantiate(BounceConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 18: // double immuable
                if (sender.CompareTag("Player"))
                {
                    ImmuSIMDroite.SetActive(true);
                    ImmuSIMGauche.SetActive(true);
                    
                    ImpulseSIMDroite.SetActive(false);
                    BounceSIMDroite.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                    
                    ImpulseSIMGauche.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }
                else
                {
                    Instantiate(ImmuConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 54: // double magnet
                if (sender.CompareTag("Player"))
                {
                    MagnetSIMDroite.SetActive(true);
                    MagnetSIMGauche.SetActive(true);
                    
                    ImpulseSIMDroite.SetActive(false);
                    BounceSIMDroite.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                    
                    ImpulseSIMGauche.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                    
                }
                else
                {
                    Instantiate(MagnetConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 162: // double rocket
                if (sender.CompareTag("Player"))
                {
                    RocketSIMDroite.SetActive(true);
                    RocketSIMGauche.SetActive(true);
                    
                    ImpulseSIMDroite.SetActive(false);
                    BounceSIMDroite.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    
                    ImpulseSIMGauche.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                }
                else
                {
                    Instantiate(RocketConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 4: // Impulse - Bouncing
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(true);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(true);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImpulseConstantFeedback, SpawnPoint.transform);
                    Instantiate(BounceConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 10: // Impulse - Immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(true);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(true);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImpulseConstantFeedback, SpawnPoint.transform);
                    Instantiate(ImmuConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 28: // Impulse - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(true);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(true);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(true);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImpulseConstantFeedback, SpawnPoint.transform);
                    Instantiate(MagnetConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 82: // Impulse - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(true);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(true);
                        
                        ImpulseSIMGauche.SetActive(true);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImpulseConstantFeedback, SpawnPoint.transform); 
                    Instantiate(RocketConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 12: // Bouncing - Immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(BounceConstantFeedback, SpawnPoint.transform);
                    Instantiate(ImmuConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 30: // Bouncing - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(true);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(true);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(BounceConstantFeedback, SpawnPoint.transform);
                    Instantiate(MagnetConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 84: // Bouncing - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(true);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(true);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(true);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(BounceConstantFeedback, SpawnPoint.transform);
                    Instantiate(RocketConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 36: // Immuable - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(true);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(true);
                        RocketSIMGauche.SetActive(false);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(true);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImmuConstantFeedback, SpawnPoint.transform);
                    Instantiate(MagnetConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 90: // Immuable - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(true);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(true);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(true);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(ImmuConstantFeedback, SpawnPoint.transform);
                    Instantiate(RocketConstantFeedback, SpawnPoint.transform);
                }

                break;
            case 108: // Magnet - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(true);
                        RocketSIMDroite.SetActive(false);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMGauche.SetActive(true);
                    }
                    else
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMDroite.SetActive(true);
                        
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMGauche.SetActive(true);
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    Instantiate(MagnetConstantFeedback, SpawnPoint.transform);
                    Instantiate(RocketConstantFeedback, SpawnPoint.transform);
                }

                break;
        }
    }

    public void FeedbackPlayState(GameObject sender, float value)
    {
        int stateValue = 0;
        ComportementsStateMachine stateMachine = sender.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
            ComportementState currentObjectState = (ComportementState)stateMachine.currentState;
            stateValue = currentObjectState.stateValue;

            if (!sender.CompareTag("Player"))
            {
                SpawnPoint = sender.GetComponentInChildren<CubeTagFeedback>().gameObject;
            }

            switch (stateValue)
            {
                case 0: // pas de comportement
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 1: // solo impulse
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 3: // solo bouncing
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 9: // solo immuable
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 27: // solo magnet
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 81: // solo rocket
                    C_Solo_Rocket rocketState = currentObjectState as C_Solo_Rocket;

                    if (rocketState.rocketOn)
                    {
                        rocketTrail = Instantiate(RocketOnFeedback, sender.transform);
                    }
                    else
                    {
                        foreach (Transform child in sender.transform) // recherche dans les enfants directes
                        {
                            CubeTagFeedbackToDestroy feedback = child.gameObject.GetComponent<CubeTagFeedbackToDestroy>(); // on cherche le script qui marcque les feedback a détruire
                            if (feedback != null)
                            {
                                Destroy(feedback.gameObject);
                            }
                        }
                    }

                    if (sender.CompareTag("Player"))
                    {

                    }
                    else
                    {

                    }

                    break;
                case 2: // double impulse
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 6: // double bouncing
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 18: // double immuable
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 54: // double magnet
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 162: // double rocket
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 4: // Impulse - Bouncing
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 10: // Impulse - Immuable
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 28: // Impulse - Magnet
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 82: // Impulse - Rocket
                    C_Impulse_Rocket impulseRocketState = currentObjectState as C_Impulse_Rocket;

                    if (impulseRocketState.rocketOn)
                    {
                        rocketTrail = Instantiate(RocketOnFeedback, sender.transform);
                    }
                    else
                    {
                        foreach (Transform child in sender.transform) // recherche dans les enfants directes
                        {
                            CubeTagFeedbackToDestroy feedback = child.gameObject.GetComponent<CubeTagFeedbackToDestroy>(); // on cherche le script qui marcque les feedback a détruire
                            if (feedback != null)
                            {
                                Destroy(feedback.gameObject);
                            }
                        }
                    }

                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 12: // Bouncing - Immuable
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 30: // Bouncing - Magnet
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 84: // Bouncing - Rocket
                    C_Bouncing_Rocket bouncingRocketState = currentObjectState as C_Bouncing_Rocket;
                    if (bouncingRocketState.rocketOn)
                    {
                        rocketTrail = Instantiate(RocketOnFeedback, sender.transform);
                    }
                    else
                    {
                        foreach(Transform child in sender.transform) // recherche dans les enfants directes
                        {
                            CubeTagFeedbackToDestroy feedback = child.gameObject.GetComponent<CubeTagFeedbackToDestroy>(); // on cherche le script qui marcque les feedback a détruire
                            if (feedback != null)
                            {
                                Destroy(feedback.gameObject);
                            }
                        }
                    }

                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 36: // Immuable - Magnet
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 90: // Immuable - Rocket
                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
                case 108: // Magnet - Rocket
                    C_Magnet_Rocket magnetRocketState = currentObjectState as C_Magnet_Rocket;

                    if (magnetRocketState.rocketOn)
                    {
                        rocketTrail = Instantiate(RocketOnFeedback, sender.transform);
                    }
                    else
                    {
                        foreach (Transform child in sender.transform) // recherche dans les enfants directes
                        {
                            CubeTagFeedbackToDestroy feedback = child.gameObject.GetComponent<CubeTagFeedbackToDestroy>(); // on cherche le script qui marcque les feedback a détruire
                            if (feedback != null)
                            {
                                Destroy(feedback.gameObject);
                            }
                        }
                    }

                    if (sender.CompareTag("Player"))
                    {
                    }
                    else
                    {
                    }

                    break;
            }
        }
    }

    public void FeedbackExitState(GameObject sender)
    {
        int stateValue = 0;
        ComportementsStateMachine stateMachine = sender.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
            ComportementState currentObjectState = (ComportementState)stateMachine.currentState;
            stateValue = currentObjectState.stateValue;

            if (!sender.CompareTag("Player"))
            {
                feedbacksToDestroy = sender.GetComponentInChildren<CubeTagFeedback>().gameObject.GetComponentsInChildren<CubeTagFeedbackToDestroy>();

                foreach (CubeTagFeedbackToDestroy feedback in feedbacksToDestroy)
                {
                    Destroy(feedback.gameObject);
                }
            }

            switch (stateValue)
            {
                case 0: // pas de comportement
                    if (sender.CompareTag("Player"))
                    {

                    }
                    else
                    {

                    }

                    break;
                case 1: // solo impulse
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 3: // solo bouncing
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 9: // solo immuable
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 27: // solo magnet
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 81: // solo rocket
                    C_Solo_Rocket rocketState = currentObjectState as C_Solo_Rocket;

                    if (rocketState.rocketOn)
                    {
                        foreach(CubeTagFeedbackToDestroy feedback in sender.GetComponentsInChildren<CubeTagFeedbackToDestroy>())
                        {
                            Destroy(feedback.gameObject);
                        }
                    }

                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 2: // double impulse
                    if (sender.CompareTag("Player"))
                    {
                        
                    }
                    else
                    {

                    }

                    break;
                case 6: // double bouncing
                    if (sender.CompareTag("Player"))
                    {
                        
                    }
                    else
                    {

                    }

                    break;
                case 18: // double immuable
                    if (sender.CompareTag("Player"))
                    {
                        
                    }
                    else
                    {

                    }

                    break;
                case 54: // double magnet
                    if (sender.CompareTag("Player"))
                    {
                        
                    }
                    else
                    {

                    }

                    break;
                case 162: // double rocket
                    if (sender.CompareTag("Player"))
                    {
                        
                    }
                    else
                    {

                    }

                    break;
                case 4: // Impulse - Bouncing
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 10: // Impulse - Immuable
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 28: // Impulse - Magnet
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 82: // Impulse - Rocket
                    C_Impulse_Rocket impulseRocketState = currentObjectState as C_Impulse_Rocket;

                    if (impulseRocketState.rocketOn)
                    {
                        foreach (CubeTagFeedbackToDestroy feedback in sender.GetComponentsInChildren<CubeTagFeedbackToDestroy>())
                        {
                            Destroy(feedback.gameObject);
                        }
                    }
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 12: // Bouncing - Immuable
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 30: // Bouncing - Magnet
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 84: // Bouncing - Rocket
                    C_Bouncing_Rocket bouncingRocketState = currentObjectState as C_Bouncing_Rocket;

                    if (bouncingRocketState.rocketOn)
                    {
                        foreach (CubeTagFeedbackToDestroy feedback in sender.GetComponentsInChildren<CubeTagFeedbackToDestroy>())
                        {
                            Destroy(feedback.gameObject);
                        }
                    }

                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 36: // Immuable - Magnet
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 90: // Immuable - Rocket
                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
                case 108: // Magnet - Rocket
                    C_Magnet_Rocket magnetRocketState = currentObjectState as C_Magnet_Rocket;

                    if (magnetRocketState.rocketOn)
                    {
                        foreach (CubeTagFeedbackToDestroy feedback in sender.GetComponentsInChildren<CubeTagFeedbackToDestroy>())
                        {
                            Destroy(feedback.gameObject);
                        }
                    }

                    if (sender.CompareTag("Player"))
                    {
                        if (playerStateMachine.inversion == true)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {

                    }

                    break;
            }
        }
    }

    public void FeedbackExplosion(GameObject explodingGameObject, float timeBeforeNextExplosion, bool isStartingState)
    {
        // feedback au moment de l'explosion et au start de l'etat impulse, le float est le temps avant la prochaine explosion et le bool est vrai uniquement au start du state
        if(explodingGameObject.CompareTag("Player")){}
        else
        {
            GameObject ImpulseTime =Instantiate(ImpulseTimerFeedback, explodingGameObject.transform);
            ImpulseTime.GetComponent<ImpulseTimer>().burstCycleCount = Mathf.RoundToInt(timeBeforeNextExplosion);
            ImpulseTime.GetComponent<ImpulseTimer>().repulserTime = timeBeforeNextExplosion;
        }
    }

    public void FeedbackJustBeforeExplosion(GameObject explodingGameObjec)
    {
        // feedback 1.5s avant l'explosion (valeur modifiable dans le script du state)
        if(explodingGameObjec.CompareTag("Player")){}
        else
        {
            Instantiate(ImpulseImpactFeedback, explodingGameObjec.transform);
        }
    }

    public void FeedbackBounceLocation(ContactPoint[] locations) // appelé quand un comportement bounce rebondit
    {
        // feedback de bounce, la position est dans locations[id].point
    }

    public void FeedbackJustBeforeRocketStart(GameObject rocketGameObject)
    {
        // feedback 1s avant l'explosion (valeur modifiable dans le script du state)
        
        if(rocketGameObject.CompareTag("Player")){}
        else
        {
            Instantiate(RocketImpact, rocketGameObject.transform);
        }
    }
}