using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance;

    public event Action<GameObject, bool, bool> OnComportmentExtracted;
    public event Action<GameObject, bool, bool> OnComportmentAdded;
    public event Action<GameObject,bool> OnComportmentExchanged;

    public event Action<GameObject,bool> OnSelfImpactMod;

    public event Action<GameObject> OnComportementStateEnter;
    public event Action<GameObject> OnComportementStateExit;
    public event Action<GameObject, float> OnComportementStatePlay;
    public event Action<GameObject> OnFootstep;
    public event Action<GameObject> OnJump;
    public event Action<GameObject> OnLand;
    
    public event Action<GameObject> OnCollide;

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

    public void ComportmentExchanged(GameObject player, bool rightHand) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnComportmentExchanged?.Invoke(player, rightHand);
    }
    
    public void SelfImpactMod(GameObject player, bool active) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnSelfImpactMod?.Invoke(player, active);
    }

    public void ComportmentStateEnter(GameObject comportableObject) // appele dans le Enter des comportements
    {
        OnComportementStateEnter?.Invoke(comportableObject);
    }

    public void ComportmentStateExit(GameObject comportableObject) // appele dans le Exit des comportements
    {
        OnComportementStateExit?.Invoke(comportableObject);
    }

    public void ComportmentStatePlay(GameObject comportableObject, float force = 0.8f) // appele lorsque le comportement agit
    {
        OnComportementStatePlay?.Invoke(comportableObject,force);
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

    public void Collision(GameObject gameObject) // appele lors d'un collision d'un objet
    {
        OnCollide?.Invoke(gameObject);
    }
}
