using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InventorySystem: MonoBehaviour
{
    // [Header("References")]
    // [SerializeField] private IventoryUIMenu ui;
    
    private Dictionary<ItemData, InventorySlot> inventoryItems = new Dictionary<ItemData, InventorySlot>();

    public Dictionary<ItemData, InventorySlot> InventoryItems
    {
        get => inventoryItems;
        //set => inventoryItems = value;
    }
    // private Dictionary<TapeData, int> tapes = new Dictionary<TapeData, int>();
    
    //emet event
    public event Action OnInventoryChanged;
    
    public void AddItem(ItemData newItem)
    {
        if (!inventoryItems.ContainsKey(newItem))
        {
            inventoryItems.Add(newItem, new InventorySlot(newItem, 0));
            OnInventoryChanged?.Invoke();
        }

        inventoryItems[newItem].quantity++;
        Debug.Log($"name {newItem.itemName}, quantity {inventoryItems[newItem].quantity}");
    }

    public void RemoveItem(ItemData itemToRemove)
    {
        if (inventoryItems[itemToRemove].quantity > 0)
        {
            inventoryItems[itemToRemove].quantity--;
            OnInventoryChanged?.Invoke();
        }
    }
}