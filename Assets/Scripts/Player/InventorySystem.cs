using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventorySystem: MonoBehaviour
{
    // [Header("References")]
    // [SerializeField] private IventoryUIMenu ui;
    
    private Dictionary<ItemData, InventorySlot> inventoryItems = new Dictionary<ItemData, InventorySlot>();
    // private Dictionary<TapeData, int> tapes = new Dictionary<TapeData, int>();
    
    public void AddItem(ItemData newItem)
    {
        if (!inventoryItems.ContainsKey(newItem))
        {
            inventoryItems.Add(newItem, new InventorySlot(newItem, 0));
        }

        inventoryItems[newItem].quantity++;
        Debug.Log($"name {newItem.itemName}, quantity {inventoryItems[newItem].quantity}");
    }

    public void RemoveItem(ItemData itemToRemove)
    {
        if (inventoryItems[itemToRemove].quantity > 0)
        {
            inventoryItems[itemToRemove].quantity--;
        }
    }
}