using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    public ComportementsStateMachine playerStateMachine;
    
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
    public GameObject ImpulseTimer;
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

    public void FeedbackEnterState(GameObject sender)
    {
        int stateValue = 0;
        ComportementsStateMachine stateMachine = sender.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
            ComportementState currentObjectState = (ComportementState)stateMachine.currentState;
            stateValue = currentObjectState.stateValue;
        }

        switch (stateValue)
        {
            case 0: // pas de comportement
                if (sender.CompareTag("Player"))
                {
                    ImpulseSIMDroite.SetActive(false);
                    ImpulseSIMGauche.SetActive(false);
                    BounceSIMDroite.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }
                else
                {
                    //NO COMP
                }

                break;
            case 1: // solo impulse
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 3: // solo bouncing
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(true);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Bounce
                }

                break;
            case 9: // solo immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImmuSIMDroite.SetActive(true);
                    }
                    
                    else
                    {
                        ImmuSIMGauche.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Immu
                }

                break;
            case 27: // solo magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        MagnetSIMDroite.SetActive(true);
                    }
                    
                    else
                    {
                        MagnetSIMGauche.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Magnet
                }

                break;
            case 81: // solo rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        RocketSIMDroite.SetActive(true);
                    }
                    
                    else
                    {
                        RocketSIMGauche.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Rocket
                }

                break;
            case 2: // double impulse
                if (sender.CompareTag("Player"))
                {
                    ImpulseSIMDroite.SetActive(true);
                    ImpulseSIMGauche.SetActive(true);
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 6: // double bouncing
                if (sender.CompareTag("Player"))
                {
                    BounceSIMDroite.SetActive(true);
                    BounceSIMGauche.SetActive(true);
                }
                else
                {
                    //Feedback Constant Bounce
                }

                break;
            case 18: // double immuable
                if (sender.CompareTag("Player"))
                {
                    ImmuSIMDroite.SetActive(true);
                    ImmuSIMGauche.SetActive(true);
                }
                else
                {
                    //Feedback Constant Immu
                }

                break;
            case 54: // double magnet
                if (sender.CompareTag("Player"))
                {
                    MagnetSIMDroite.SetActive(true);
                    MagnetSIMGauche.SetActive(true);
                }
                else
                {
                    //Feedback Constant Magnet
                }

                break;
            case 162: // double rocket
                if (sender.CompareTag("Player"))
                {
                    RocketSIMDroite.SetActive(true);
                    RocketSIMGauche.SetActive(true);
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 4: // Impulse - Bouncing
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        BounceSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(true);
                        BounceSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Bounce
                }

                break;
            case 10: // Impulse - Immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        ImmuSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(true);
                        ImmuSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Immu
                }

                break;
            case 28: // Impulse - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        MagnetSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(true);
                        MagnetSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Magnet
                }

                break;
            case 82: // Impulse - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(true);
                        RocketSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(true);
                        RocketSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Rocket
                }

                break;
            case 12: // Bouncing - Immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(true);
                        ImmuSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Bounce + Immu
                }

                break;
            case 30: // Bouncing - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(true);
                        ImmuSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(true);
                        ImmuSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Bounce + Magnet
                }

                break;
            case 84: // Bouncing - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(true);
                        RocketSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(true);
                        RocketSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Bounce / Rocket
                }

                break;
            case 36: // Immuable - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImmuSIMDroite.SetActive(true);
                        MagnetSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        ImmuSIMGauche.SetActive(true);
                        MagnetSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Immu + Magnet
                }

                break;
            case 90: // Immuable - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImmuSIMDroite.SetActive(true);
                        RocketSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        ImmuSIMGauche.SetActive(true);
                        RocketSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Immu + Rocket
                }

                break;
            case 108: // Magnet - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        MagnetSIMDroite.SetActive(true);
                        RocketSIMGauche.SetActive(true);
                    }
                    
                    else
                    {
                        MagnetSIMGauche.SetActive(true);
                        RocketSIMDroite.SetActive(true);
                    }
                }
                else
                {
                    //Feedback Constant Magnet + Rocket
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
                if (sender.CompareTag("Player"))
                {
                }
                else
                {
                }

                break;
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
        }

        switch (stateValue)
        {
            case 0: // pas de comportement
                if (sender.CompareTag("Player"))
                {
                    ImpulseSIMDroite.SetActive(false);
                    ImpulseSIMGauche.SetActive(false);
                    BounceSIMDroite.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                    ImmuSIMDroite.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                    MagnetSIMDroite.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                    RocketSIMDroite.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }
                else
                {
                    //NO COMP
                }

                break;
            case 1: // solo impulse
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 3: // solo bouncing
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(false);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Bounce
                }

                break;
            case 9: // solo immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImmuSIMDroite.SetActive(false);
                    }
                    
                    else
                    {
                        ImmuSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Immu
                }

                break;
            case 27: // solo magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        MagnetSIMDroite.SetActive(false);
                    }
                    
                    else
                    {
                        MagnetSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Magnet
                }

                break;
            case 81: // solo rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        RocketSIMDroite.SetActive(false);
                    }
                    
                    else
                    {
                        RocketSIMGauche.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Rocket
                }

                break;
            case 2: // double impulse
                if (sender.CompareTag("Player"))
                {
                    ImpulseSIMDroite.SetActive(false);
                    ImpulseSIMGauche.SetActive(false);
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 6: // double bouncing
                if (sender.CompareTag("Player"))
                {
                    BounceSIMDroite.SetActive(false);
                    BounceSIMGauche.SetActive(false);
                }
                else
                {
                    //Feedback Constant Bounce
                }

                break;
            case 18: // double immuable
                if (sender.CompareTag("Player"))
                {
                    ImmuSIMDroite.SetActive(false);
                    ImmuSIMGauche.SetActive(false);
                }
                else
                {
                    //Feedback Constant Immu
                }

                break;
            case 54: // double magnet
                if (sender.CompareTag("Player"))
                {
                    MagnetSIMDroite.SetActive(false);
                    MagnetSIMGauche.SetActive(false);
                }
                else
                {
                    //Feedback Constant Magnet
                }

                break;
            case 162: // double rocket
                if (sender.CompareTag("Player"))
                {
                    RocketSIMDroite.SetActive(false);
                    RocketSIMGauche.SetActive(false);
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 4: // Impulse - Bouncing
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        BounceSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        BounceSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Bounce
                }

                break;
            case 10: // Impulse - Immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Immu
                }

                break;
            case 28: // Impulse - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Magnet
                }

                break;
            case 82: // Impulse - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImpulseSIMDroite.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        ImpulseSIMGauche.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Impulse + Rocket
                }

                break;
            case 12: // Bouncing - Immuable
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Bounce + Immu
                }

                break;
            case 30: // Bouncing - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(false);
                        ImmuSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(false);
                        ImmuSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Bounce + Magnet
                }

                break;
            case 84: // Bouncing - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        BounceSIMDroite.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        BounceSIMGauche.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Bounce / Rocket
                }

                break;
            case 36: // Immuable - Magnet
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImmuSIMDroite.SetActive(false);
                        MagnetSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        ImmuSIMGauche.SetActive(false);
                        MagnetSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Immu + Magnet
                }

                break;
            case 90: // Immuable - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        ImmuSIMDroite.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        ImmuSIMGauche.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Immu + Rocket
                }

                break;
            case 108: // Magnet - Rocket
                if (sender.CompareTag("Player"))
                {
                    if (playerStateMachine.inversion == true)
                    {
                        MagnetSIMDroite.SetActive(false);
                        RocketSIMGauche.SetActive(false);
                    }
                    
                    else
                    {
                        MagnetSIMGauche.SetActive(false);
                        RocketSIMDroite.SetActive(false);
                    }
                }
                else
                {
                    //Feedback Constant Magnet + Rocket
                }

                break;
        }
    }

    public void FeedbackExplosion(GameObject explodingGameObject, float timeBeforeNextExplosion, bool isStartingState)
    {
        // feedback au moment de l'explosion et au start de l'etat impulse, le float est le temps avant la prochaine explosion et le bool est vrai uniquement au start du state
    }

    public void FeedbackJustBeforeExplosion(GameObject explodingGameObjec)
    {
        // feedback 1.5s avant l'explosion (valeur modifiable dans le script du state)
    }

    public void FeedbackBounceLocation(ContactPoint[] locations) // appelé quand un comportement bounce rebondit
    {
        // feedback de bounce, la position est dans locations[id].point
    }

    public void FeedbackJustBeforeRocketStart(GameObject rocketGameObject)
    {
        // feedback 1s avant l'explosion (valeur modifiable dans le script du state)
    }

    public void FeedbackSIM(float leftValue, float rightValue, bool inverse)
    {
        //instantie/active les comportements à gauche et à droite en fonction des comportements sur le player.
        
        
        
    }
}