using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance;

    public event Action<GameObject, bool> OnComportmentExtracted;
    public event Action<GameObject, bool> OnComportmentAdded;
    public event Action<bool> OnComportmentExchanged;

    private void Awake()
    {
        Instance = this;
    }

    public void ComportmentExtracted(GameObject originOfComportment, bool rightHand) //appele quand on vole un comportement a un objet
    {
        OnComportmentExtracted?.Invoke(originOfComportment, rightHand);
    }

    public void ComportmentAdded(GameObject objectToAddComportment, bool rightHand) //appele quand on donne un comportement a un objet
    {
        OnComportmentAdded?.Invoke(objectToAddComportment, rightHand);
    }

    public void ComportmentExchanged(bool rightHand) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnComportmentExchanged?.Invoke(rightHand);
    }
}
