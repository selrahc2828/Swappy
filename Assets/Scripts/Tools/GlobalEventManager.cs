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

    // Inventory
    public event Action OnAddInventory;
    public event Action OnRemoveInventory;
    public event Action<ItemData, int> OnPopupInventory;
    
    // Tape
    public event Action OnAddTape;
    public event Action OnRemoveTape;
    public event Action OnSetStateTape;
    public event Action<TapeData> OnPopupTape;

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

    #region Inventaire
    public void AddInventory() //appele quand on ajoute un item dans l'inventaire
    {
        OnAddInventory?.Invoke();
    }
    
    public void RemoveInventory() //appele quand on retire un item de l'inventaire
    {
        OnRemoveInventory?.Invoke();
    }
    public void DisplayPopupPickUpItem(ItemData newItem, int quantity) //appele quand on ajoute un item dans l'inventaire
    {
        OnPopupInventory?.Invoke(newItem, quantity);
    }
    #endregion
    
    #region Tapes
    public void AddTape() //appele quand on ajoute une nouvelle cassette
    {
        OnAddTape?.Invoke();
    }
    
    public void RemoveTape() //appele quand on retire une cassette
    {
        OnRemoveTape?.Invoke();
    }
    
    public void SetStateTape() //appele quand on ramasse/déebloque une nouvelle cassette
    {
        OnSetStateTape?.Invoke();
    }
    
    public void DisplayPopupPickUpTape(TapeData newItem) //appele quand on ajoute un item dans l'inventaire
    {
        OnPopupTape?.Invoke(newItem);
    }
    #endregion
}
