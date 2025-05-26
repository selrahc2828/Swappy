using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    [Header("PlayerInfo")] public GameObject SIMGauche;
    public GameObject SIMDroite;

    [Header("Impulse")] 
    public GameObject ImpulseConstantFeedback;
    public GameObject ImpulseTimer;
    public GameObject ImpulseImpactFeedback;
    public GameObject ImpulseSIMFeedback;

    [Header("Bounce")] public GameObject BounceConstantFeedback;
    public GameObject BounceActiveFeedback;
    public GameObject BounceSIMFeedback;

    [Header("Immuable")] public GameObject ImmuConstantFeedback;
    public GameObject ImmuSIMFeedback;

    [Header("Magnet")] public GameObject MagnetConstantFeedback;
    public GameObject MagnetSIMFeedback;

    [Header("Rocket")] public GameObject RocketConstantFeedback;
    public GameObject RocketOnFeedback;
    public GameObject RocketSIMFeedback;

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
                }
                else
                {
                }

                break;
            case 1: // solo impulse
                if (sender.CompareTag("Player"))
                {
                    //SIM Impulse
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 3: // solo bouncing
                if (sender.CompareTag("Player"))
                {
                    //SIM Bounce
                }
                else
                {
                    //Feedback Constant Bounce
                }

                break;
            case 9: // solo immuable
                if (sender.CompareTag("Player"))
                {
                    //SIM Immu
                }
                else
                {
                    //Feedback Constant Immu
                }

                break;
            case 27: // solo magnet
                if (sender.CompareTag("Player"))
                {
                    //SIM Magnet
                }
                else
                {
                    //Feedback Constant Magnet
                }

                break;
            case 81: // solo rocket
                if (sender.CompareTag("Player"))
                {
                    //SIM Rocket
                }
                else
                {
                    //Feedback Constant Rocket
                }

                break;
            case 2: // double impulse
                if (sender.CompareTag("Player"))
                {
                    //SIM 2XImpulse
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 6: // double bouncing
                if (sender.CompareTag("Player"))
                {
                    //SIM2XBounce
                }
                else
                {
                    //Feedback Constant Bounce
                }

                break;
            case 18: // double immuable
                if (sender.CompareTag("Player"))
                {
                    //SIM2XImmu
                }
                else
                {
                    //Feedback Constant Immu
                }

                break;
            case 54: // double magnet
                if (sender.CompareTag("Player"))
                {
                    //SIM2XMagnet
                }
                else
                {
                    //Feedback Constant Magnet
                }

                break;
            case 162: // double rocket
                if (sender.CompareTag("Player"))
                {
                    //SIM2XRocket
                }
                else
                {
                    //Feedback Constant Impulse
                }

                break;
            case 4: // Impulse - Bouncing
                if (sender.CompareTag("Player"))
                {
                    //SIM Impulse / Bounce
                }
                else
                {
                    //Feedback Constant Impulse + Bounce
                }

                break;
            case 10: // Impulse - Immuable
                if (sender.CompareTag("Player"))
                {
                    //SIM Impulse / Immu
                }
                else
                {
                    //Feedback Constant Impulse + Immu
                }

                break;
            case 28: // Impulse - Magnet
                if (sender.CompareTag("Player"))
                {
                    //SIM Impulse / Magnet
                }
                else
                {
                    //Feedback Constant Impulse + Magnet
                }

                break;
            case 82: // Impulse - Rocket
                if (sender.CompareTag("Player"))
                {
                    //SIM Impulse / Rocket
                }
                else
                {
                    //Feedback Constant Impulse + Rocket
                }

                break;
            case 12: // Bouncing - Immuable
                if (sender.CompareTag("Player"))
                {
                    //SIM Bounce / Immu
                }
                else
                {
                    //Feedback Constant Bounce + Immu
                }

                break;
            case 30: // Bouncing - Magnet
                if (sender.CompareTag("Player"))
                {
                    //SIM Bounce / Magnet
                }
                else
                {
                    //Feedback Constant Bounce + Magnet
                }

                break;
            case 84: // Bouncing - Rocket
                if (sender.CompareTag("Player"))
                {
                    //SIM Bounce / Rocket
                }
                else
                {
                    //Feedback Constant Bounce / Rocket
                }

                break;
            case 36: // Immuable - Magnet
                if (sender.CompareTag("Player"))
                {
                    //SIM Immu / Magnet
                }
                else
                {
                    //Feedback Constant Immu + Magnet
                }

                break;
            case 90: // Immuable - Rocket
                if (sender.CompareTag("Player"))
                {
                    //SIM Immu / Rocket
                }
                else
                {
                    //Feedback Constant Immu + Rocket
                }

                break;
            case 108: // Magnet - Rocket
                if (sender.CompareTag("Player"))
                {
                    //SIM Magnet / Rocket
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