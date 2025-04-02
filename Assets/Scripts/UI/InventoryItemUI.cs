using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class InventoryItemUI: MonoBehaviour
{
    public InventorySystem inventory;
    public GameObject itemSlotPrefab;
    public Transform itemSlotParent;
    
    public List<GameObject> itemSlots = new List<GameObject>();

    void Start()
    {
        // Initialiser l'UI en créant des slots pour chaque capacité
        // for (int i = 0; i < inventory.inventoryItems.Count; i++) {
        //     GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
        //     itemSlots.Add(slot);
        // }

        UpdateUI();
    }

    public void UpdateUI()
    {
        // affiche items dans les slots créé

        // for (int i = 0; i < inventory.inventoryItems.Count; i++)
        // {
        //     Image iconImage = itemSlots[i].GetComponent<Image>();
        //     Text quantityText = itemSlots[i].GetComponentInChildren<Text>();
        //     
        //     if (i < inventory.inventoryItems.Count && inventory.inventoryItems[i].item != null) {
        //         iconImage.sprite = inventory.inventoryItems[i].item.itemSprite;
        //         iconImage.SetEnabled(true);
        //
        //         // Mettre à jour la quantité affichée
        //         quantityText.text = inventory.inventoryItems[i].quantity.ToString();
        //         quantityText.enabled = true;
        //     } else {
        //         iconImage.sprite = null;
        //         iconImage.SetEnabled(false);
        //         quantityText.enabled = false;
        //     }
        //     
        // }
    }
}