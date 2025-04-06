using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InventorySystem: MonoBehaviour
{
    
    [SerializeField] private int maxSlots = 20;

    public int MaxSlots => maxSlots;

    private Dictionary<ItemData, InventorySlot> inventoryItems = new Dictionary<ItemData, InventorySlot>();

    public Dictionary<ItemData, InventorySlot> InventoryItems
    {
        get => inventoryItems;
        //set => inventoryItems = value;
    }
    // private Dictionary<TapeData, int> tapes = new Dictionary<TapeData, int>();
    
    //emet event
    public event Action OnInventoryChanged;
    
    public void AddItem(ItemData newItem, int quantity = 1)
    {
        if (!inventoryItems.ContainsKey(newItem))
        {
            if (inventoryItems.Count >= maxSlots)
            {
                Debug.Log("Inventaire plein. Impossible d'ajouter plus d'objets.");
                return;
            }
            
            inventoryItems.Add(newItem, new InventorySlot(newItem, 0));
        }

        inventoryItems[newItem].quantity += quantity;
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData itemToRemove,  int quantity = 1)
    {
        
        if (!inventoryItems.ContainsKey(itemToRemove))
        {
            Debug.LogWarning("L'objet à retirer n'existe pas dans l'inventaire.");
            return;
        }
        
        
        inventoryItems[itemToRemove].quantity -= quantity;
        
        // si on a plus rien, voir si on conserve ou non 
        if (inventoryItems[itemToRemove].quantity <= 0)
        {
            inventoryItems.Remove(itemToRemove);
            Debug.Log($"L'objet {itemToRemove.itemName} a été retiré de l'inventaire.");
        }
        
        OnInventoryChanged?.Invoke();
    }
}