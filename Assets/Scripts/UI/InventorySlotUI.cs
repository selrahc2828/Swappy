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

        if (itemData is not null)
        {
            if (iconImage is not  null && item.itemSprite is not  null)
                iconImage.sprite = item.itemSprite;

            if (quantityText is not  null)
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
            if (quantityText is not  null)
                quantityText.text = "";
            button.onClick.RemoveAllListeners();//GetComponent<Button>()
        }
        

    }
}