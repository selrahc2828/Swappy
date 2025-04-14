using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // === Singleton ===
    public static CanvasManager Instance { get; private set; }

    // === Références ===
    private InventorySystem inventorySystem;
    public InventorySystem Inventory => inventorySystem;
    
    public MenuManager menuManager;
    
    private TapeSystem tapeSystem;
    public TapeSystem TapeSystem => tapeSystem;
    
    [Header("Pick Item")]
    [SerializeField] private GameObject itemPopupPrefab;
    [SerializeField] private Transform popupItemParent;
    
    [Header("Pick Tape")]
    [SerializeField] private GameObject tapePopupPrefab;
    [SerializeField] private Transform popupTapeParent; 

    
    private void Awake()
    {
        // Debug.Log("awake CanvasManager");

        // Singleton pattern simple
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            // Debug.Log("awake CanvasManager instance already exists!");
            return;
        }

        Instance = this;
        // Debug.Log("awake CanvasManager instance Initialize");
        Initialize();
    }
    
    private void OnEnable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnPopupInventory += ShowPopupItem;
        }

        if (tapeSystem != null)
        {
            tapeSystem.OnPopupTape += ShowPopupTape;
        }
    }

    private void OnDisable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnPopupInventory -= ShowPopupItem;
        }

        if (tapeSystem != null)
        {
            tapeSystem.OnPopupTape -= ShowPopupTape;
        }
    }

    public void Initialize()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        if (inventorySystem != null)
        {
            menuManager.inventoryMenu.inventorySystem = inventorySystem;
        }
        
        tapeSystem = FindObjectOfType<TapeSystem>();
    }

    public void ShowPopupItem(ItemData item, int amount = 1)
    {
        if (itemPopupPrefab is null || popupItemParent is null) return;

        GameObject popup = Instantiate(itemPopupPrefab, popupItemParent);

        PopupItemGroup popupItemGroup = popup.GetComponent<PopupItemGroup>();
        if (popupItemGroup != null)
        {
            popupItemGroup.icon.sprite = item.itemSprite;
            popupItemGroup.textQuantity.text = $"+ {amount}";
        }
    }

    public void ShowPopupTape(TapeData tape)
    {
        if (tapePopupPrefab == null || popupTapeParent == null) return;
        
        GameObject popup = Instantiate(tapePopupPrefab, popupTapeParent);
        PopupTapeGroup popupTapeGroup = popup.GetComponent<PopupTapeGroup>();
        popupTapeGroup.icon.sprite = tape.itemSprite;
        popupTapeGroup.textName.text = tape.itemName;
    }
}