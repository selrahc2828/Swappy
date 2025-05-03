using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance;

    public event Action<GameObject, bool, bool> OnComportmentExtracted;
    public event Action<GameObject, bool, bool> OnComportmentAdded;
    public event Action<GameObject,bool, bool, bool> OnComportmentExchanged;

    public event Action<GameObject,bool> OnSelfImpactMod;

    public event Action<GameObject> OnComportementStateEnter;
    public event Action<GameObject> OnComportementStateExit;
    public event Action<GameObject> OnComportementStatePlay;
    public event Action<GameObject> OnFootstep;
    public event Action<GameObject> OnJump;
    public event Action<GameObject> OnLand;

    // Inventory
    public event Action OnAddInventory;
    public event Action OnRemoveInventory;
    public event Action<ItemData, int> OnPopupInventory;
    
    // Tape
    public event Action OnAddTape;
    public event Action OnRemoveTape;
    public event Action OnSetStateTape;
    public event Action<TapeData> OnPopupTape;
    
    // Fragment
    public event Action OnAddFragment;
    public event Action OnRemoveFragment;



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

    public void ComportmentExchanged(GameObject player, bool rightHand, bool comportementIn = false, bool comportementOut = false) // appele quand on echange le comportement d'une main avec un comportement du player
    {
        OnComportmentExchanged?.Invoke(player, rightHand,comportementIn, comportementOut);
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

    #region Fragment

    public void AddFragment() //appele quand on ramasse un fragment
    {
        OnAddFragment?.Invoke();
    }
    
    public void RemoveFragment() //appele quand on perd un fragment
    {
        OnRemoveFragment?.Invoke();
    }

    #endregion
}
