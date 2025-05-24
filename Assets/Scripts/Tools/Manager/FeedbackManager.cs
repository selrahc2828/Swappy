using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

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
        int stateValue = sender.GetComponent<ComportementState>().stateValue;
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

    public void FeedbackPlayState(GameObject sender, float value)
    {
        int stateValue = sender.GetComponent<ComportementState>().stateValue;
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
        int stateValue = sender.GetComponent<ComportementState>().stateValue;
        switch (stateValue)
        {
            case 0 : // pas de comportement
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 1 : // solo impulse
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 3 : // solo bouncing
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 9 : // solo immuable
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 27 : // solo magnet
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 81 : // solo rocket
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 2 : // double impulse
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 6 : // double bouncing
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 18 : // double immuable
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 54 : // double magnet
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 162 : // double rocket
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 4 : // Impulse - Bouncing
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 10 : // Impulse - Immuable
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 28 : // Impulse - Magnet
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 82 : // Impulse - Rocket
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 12 : // Bouncing - Immuable
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 30 : // Bouncing - Magnet
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 84 : // Bouncing - Rocket
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 36 : // Immuable - Magnet
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 90 : // Immuable - Rocket
                if (sender.CompareTag("Player"))
                {
                    
                }
                else
                {
                    
                }
                break;
            case 108 : // Magnet - Rocket
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
}
