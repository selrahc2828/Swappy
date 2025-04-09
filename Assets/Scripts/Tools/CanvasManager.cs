using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // === Singleton ===
    public static CanvasManager Instance { get; private set; }

    // === Références ===
    private InventorySystem inventorySystem;
    public InventorySystem Inventory => inventorySystem;
    
    public MenuManager menuManager;
    
    [Header("Pick Item")]
    [SerializeField] private GameObject itemPopupPrefab;
    [SerializeField] private Transform popupParent; // un panel dans ton Canvas
    
    private void Awake()
    {
        Debug.LogError("awake");

        // Singleton pattern simple
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("awake instance already exists!");
            return;
        }

        Instance = this;
        Debug.LogError("awake instance Initialize");
        Initialize();
    }
    
    private void OnEnable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnPopupInventory += ShowPopup;
        }
    }

    private void OnDisable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnPopupInventory -= ShowPopup;
        }
    }

    public void Initialize()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        if (inventorySystem != null)
        {
            menuManager.inventoryMenu.inventorySystem = inventorySystem;
        }
    }

    public void ShowPopup(ItemData item, int amount = 1)
    {
        if (itemPopupPrefab == null || popupParent == null) return;

        GameObject popup = Instantiate(itemPopupPrefab, popupParent);

        PopupGroup popupGroup = popup.GetComponent<PopupGroup>();
        if (popupGroup != null)
        {
            popupGroup.icon.sprite = item.itemSprite;
            popupGroup.textQuantity.text = $"+ {amount}";
        }
    }
}