using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance;

    public event Action<GameObject, bool, bool> OnComportmentExtracted;
    public event Action<GameObject, bool, bool> OnComportmentAdded;
    public event Action<bool> OnComportmentExchanged;

    public event Action<bool> OnSelfImpactMod;

    public event Action<GameObject> OnComportementStateEnter;
    public event Action<GameObject> OnComportementStateExit;
    public event Action<GameObject> OnComportementStatePlay;
    public event Action<GameObject> OnFootstep;
    public event Action<GameObject> OnJump;
    public event Action<GameObject> OnLand;

    private void Awake()
    {
        Instance = this;
    }

    public void ComportmentExtracted(GameObject originOfComportment, bool rightValue, bool rightHand) //appele quand on vole un comportement a un objet
    {
        OnComportmentExtracted?.Invoke(originOfComportment, rightValue, rightHand);
    }

    public void ComportmentAdded(GameObject objectToAddComportment, bool rightValue, bool rightHand) //appele quand on donne un comportement a un objet
    {
        OnComportmentAdded?.Invoke(objectToAddComportment, rightValue, rightHand);
    }

    public void ComportmentExchanged(bool rightHand) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnComportmentExchanged?.Invoke(rightHand);
    }
    
    public void SelfImpactMod(bool active) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnSelfImpactMod?.Invoke(active);
    }

    public void ComportmentStateEnter(GameObject comportableObject) // appele dans le Enter des comportements
    {
        OnComportementStateEnter?.Invoke(comportableObject);
    }

    public void ComportmentStateExit(GameObject comportableObject) // appele dans le Exit des comportements
    {
        OnComportementStateExit?.Invoke(comportableObject);
    }

    public void ComportmentStatePlay(GameObject comportableObject) // appele lorsque le comportement agit
    {
        OnComportementStatePlay?.Invoke(comportableObject);
    }

    public void Footstep(GameObject groundObject) // appele lors d'un pas du player
    {
        OnFootstep?.Invoke(groundObject);
    }

    public void Jump(GameObject groundObject) // appele lors d'un saut du player
    {
        OnJump?.Invoke(groundObject);
    }

    public void Land(GameObject groundObject) // appele lors d'un atterissage du player
    {
        OnLand?.Invoke(groundObject);
    }
}
