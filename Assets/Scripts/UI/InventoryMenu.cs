using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [Header("Fragments")]
    public FragmentSystem fragmentSystem;
    public GameObject fragmentSlotPrefab;

    [Header("Items")]
    public InventorySystem inventorySystem;
    
    public Transform inventoryContent;
    
    public GameObject inventorySlotPrefab;

    [Header("Info")]
    public TextMeshProUGUI slotInfoDescription;
    public TextMeshProUGUI slotInfoName;
    [HideInInspector] public RawImage slotInfoImageRaw; // image UI qui utilise un RenderTexture

    [Header("Preview 3D")]
    public PreviewInventoryControl previewControl; // empty dans la scène pour instancier le modèle a render == prefab RenderTextureGroup
    private GameObject currentPreviewInstance;
    
    //ecoute event
    
    void Start()
    {
        previewControl = FindObjectOfType<PreviewInventoryControl>();
        // première initialisation
        RefreshItemUI();
        RefreshFragmentUI();
    }

    void OnEnable()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        fragmentSystem = FindObjectOfType<FragmentSystem>();
        
        // abonné aux events sinon n'est pas actualisé car le canvas est déjà ouvert quand on rammasse des items/fragments
        if (inventorySystem != null)
        {
            GlobalEventManager.Instance.OnAddInventory += RefreshItemUI;
        }

        if (fragmentSystem != null)
        {
            GlobalEventManager.Instance.OnAddFragment += RefreshFragmentUI;
        }
    }

    void OnDisable()
    {
        if (inventorySystem != null)
        {
            GlobalEventManager.Instance.OnAddInventory -= RefreshItemUI;
        }

        if (fragmentSystem != null)
        {
            GlobalEventManager.Instance.OnAddFragment -= RefreshFragmentUI;
        }
    } 
    
    public void RefreshItemUI()
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
        if (clickedItem.itemPrefab != null && previewControl != null)
        {
            currentPreviewInstance = Instantiate(clickedItem.itemPrefab, previewControl.PreviewPosition.position, Quaternion.identity, previewControl.PreviewPosition);
            previewControl.SetOffsetCamera(clickedItem.itemPrefab);
            
            //layer que la PreviewCamera voit
            currentPreviewInstance.layer = LayerMask.NameToLayer("PreviewObject");
            
            currentPreviewInstance.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            currentPreviewInstance.transform.localScale = Vector3.one * 1f;
        }
    }

    void RefreshFragmentUI()
    {
        // fait au plus simple ici, on pas du principe qu'on en a qu'un seul donc pas besoin de gérer une instantiation automatique comme les RefreshItem
        fragmentSlotPrefab.GetComponent<FragmentSlotUI>().Initialize(fragmentSystem.fragmentBankData);
    }
    
    public void UpdateInventory()
    {
        RefreshItemUI();
    }
}
