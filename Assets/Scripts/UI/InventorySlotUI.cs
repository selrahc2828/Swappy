using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    private ItemData itemData;

    public void Initialize(ItemData item, int quantity, System.Action<ItemData> onClickCallback)
    {
        if (item == null)
        {
            return;
        }
        
        itemData = item;

        if (iconImage != null && item.itemSprite != null)
            iconImage.sprite = item.itemSprite;

        if (quantityText != null)
            quantityText.text = $"{item.itemName} : {quantity}";

        // Abonnement au clic sur le bouton
        GetComponent<Button>().onClick.AddListener(() =>
        {
            onClickCallback?.Invoke(itemData);
        });
    }
}