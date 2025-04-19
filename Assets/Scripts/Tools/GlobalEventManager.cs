using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance;

    public event Action<GameObject, bool, bool> OnComportmentExtracted;
    public event Action<GameObject, bool> OnComportmentAdded;
    public event Action<bool> OnComportmentExchanged;

    public event Action<bool> OnSelfImpactMod;

    private void Awake()
    {
        Instance = this;
    }

    public void ComportmentExtracted(GameObject originOfComportment, bool rightValue, bool rightHand) //appele quand on vole un comportement a un objet
    {
        OnComportmentExtracted?.Invoke(originOfComportment, rightValue, rightHand);
    }

    public void ComportmentAdded(GameObject objectToAddComportment, bool rightHand) //appele quand on donne un comportement a un objet
    {
        OnComportmentAdded?.Invoke(objectToAddComportment, rightHand);
    }

    public void ComportmentExchanged(bool rightHand) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnComportmentExchanged?.Invoke(rightHand);
    }
    
    public void SelfImpactMod(bool active) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnSelfImpactMod?.Invoke(active);
    }
}
