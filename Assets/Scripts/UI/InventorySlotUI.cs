using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    private ItemData itemData;
    [SerializeField] Button button;
    
    public void Initialize(ItemData item, int quantity, System.Action<ItemData> onClickCallback)
    {
        itemData = item;

        if (itemData != null)
        {
            if (iconImage != null && item.itemSprite != null)
                iconImage.sprite = item.itemSprite;

            if (quantityText != null)
                quantityText.text = quantity.ToString();

            // Abonnement au clic sur le bouton
            button.onClick.RemoveAllListeners();//clear pour éviter de faire plusieurs abonnement 
            button.onClick.AddListener(() =>
            {
                onClickCallback?.Invoke(itemData);
            });      
        }
        else
        {
            if (quantityText != null)
                quantityText.text = "";
            GetComponent<Button>().onClick.RemoveAllListeners();
        }
        

    }
}