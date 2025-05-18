using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class FragmentSystem : MonoBehaviour
{
    public FragmentBankData fragmentBankData;

    public void AddFragment(int quantity)
    {
        fragmentBankData.bankInventoryFragmentQuantity += quantity;
        
        GlobalEventManager.Instance.AddFragment();
    }

    public void RemoveFragment(int quantity)
    {
        if (fragmentBankData.bankInventoryFragmentQuantity - quantity < 0)
        {
            fragmentBankData.bankInventoryFragmentQuantity = 0;
            return;
        }
        fragmentBankData.bankInventoryFragmentQuantity -= quantity;
        GlobalEventManager.Instance.RemoveFragment();

    }
    
    public void SetFragment(int quantity)
    {
        fragmentBankData.bankInventoryFragmentQuantity = quantity;
    }
}
