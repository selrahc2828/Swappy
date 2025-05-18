using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class FragmentSystem : MonoBehaviour
{
    
    public FragmentBankData fragmentBankData;
    public static FragmentSystem Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);//détuit le doublon
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }
    
    public void AddFragment(int quantity)
    {
        fragmentBankData.bankInventoryFragmentQuantity += quantity;
        
        GlobalEventManager.Instance.AddFragment();
        GlobalEventManager.Instance.DisplayPopupAddFragment(fragmentBankData);
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
    
    public void SetFragmentHasCollect(int quantity)
    {
        fragmentBankData.hasCollect = quantity;
    }
}
