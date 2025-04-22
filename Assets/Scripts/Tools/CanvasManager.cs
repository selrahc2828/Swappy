using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // === Singleton ===
    public static CanvasManager Instance { get; private set; }

    // === Références ===
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
            return;
        }

        Instance = this;

        Initialize();
    }
    
    private void OnEnable()
    {
        GlobalEventManager.Instance.OnPopupInventory += ShowPopupItem;
        
        if (tapeSystem != null)
        {
            GlobalEventManager.Instance.OnPopupTape += ShowPopupTape;
            // refresh les boutons si on débloque une tape / change la valeur data.isUnlocked
            GlobalEventManager.Instance.OnSetStateTape += menuManager.tapeMenu.SetTapeButtons;
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.Instance.OnPopupInventory -= ShowPopupItem;
        
        if (tapeSystem != null)
        {
            GlobalEventManager.Instance.OnPopupTape -= ShowPopupTape;
            GlobalEventManager.Instance.OnSetStateTape -= menuManager.tapeMenu.SetTapeButtons;
        }
    }

    public void Initialize()
    {
        tapeSystem = FindObjectOfType<TapeSystem>();
        if (tapeSystem != null)
        {
            menuManager.tapeMenu.TapeList = tapeSystem.TapeList;
        }
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