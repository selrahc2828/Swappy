using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FragmentSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    
    private FragmentBankData bankData;

    public void Initialize(FragmentBankData data)
    {
        if (data is null)
            return;

        if (data.fragmentBankIcon is not null)
            iconImage.sprite = data.fragmentBankIcon;
        
        quantityText.text = data.bankInventoryFragmentQuantity.ToString();
    }
    
}
