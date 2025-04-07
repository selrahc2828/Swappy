using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{

    public InventorySystem inventorySystem;
    
    public Transform inventoryContent;
    
    public GameObject inventorySlotPrefab;

    [Header("Info")]
    public TextMeshProUGUI slotInfoDescription;
    public TextMeshProUGUI slotInfoName;
    [HideInInspector] public Image slotInfoImage;
    [HideInInspector] public RawImage slotInfoImageRaw; // image UI qui utilise un RenderTexture

    [Header("Preview 3D")]
    public Transform previewSpawnPoint; // empty dans la scène pour instancier le modèle a render
    private GameObject currentPreviewInstance;
    
    
    //ecoute event
    
    void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
    }
    
    void Start()
    {
        RefreshUI();
    }

    void OnEnable()
    {
        if (inventorySystem != null)
        {
            // Abonnement à l'event avec une méthode callback
            inventorySystem.OnInventoryChanged += RefreshUI;
        }
        else
        {
            Debug.LogError("Inventory system is null");
        }
    }

    void OnDisable()
    {
        if (inventorySystem != null)
        {
            // Désabonnement de l'event pour éviter les erreurs de références invalides
            inventorySystem.OnInventoryChanged -= RefreshUI;
        }
    } 
    
    public void RefreshUI()
    {
        // On supprime tous les slots existants
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        int totalSlots = inventorySystem.MaxSlots;
        List<ItemData> itemKeys = new List<ItemData>(inventorySystem.InventoryItems.Keys);
        
        // instancie un nombre de slot == à maxSlots
        for (int i = 0; i < totalSlots ; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContent);
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();
            
            if (slotUI != null)
            {
                if (i < itemKeys.Count)
                {
                    //remplis avec donnees inventaire
                    
                    slotUI.Initialize(itemKeys[i], inventorySystem.InventoryItems[itemKeys[i]].quantity, OnItemSlotClicked);
                }
                else
                {
                    slotUI.Initialize(null, 0, null);
                }
            }
            
        }
        
    }
    
    private void OnItemSlotClicked(ItemData clickedItem)
    {
        slotInfoDescription.text = clickedItem.itemDescription;
        slotInfoName.text = clickedItem.itemName;
        
        if (currentPreviewInstance != null) 
            Destroy(currentPreviewInstance);

        // instancie prefab pour la preview
        if (clickedItem.itemPrefab != null && previewSpawnPoint != null)
        {
            currentPreviewInstance = Instantiate(clickedItem.itemPrefab, previewSpawnPoint.position, Quaternion.identity, previewSpawnPoint);

            //layer que la PreviewCamera voit
            currentPreviewInstance.layer = LayerMask.NameToLayer("PreviewObject");
            
            currentPreviewInstance.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            currentPreviewInstance.transform.localScale = Vector3.one * 1f;
        }
    }
    
    public void UpdateInventory()
    {
        RefreshUI();
    }
    
}
