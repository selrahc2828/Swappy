using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentSystem : MonoBehaviour
{
    public FragmentSaveData fragmentData;

    public void AddFragment(int quantity) // quantity de FragmentData
    {
        fragmentData.inventoryFragmentQuantity += quantity;
    }

    public void RemoveFragment(int quantity)
    {
        if (fragmentData.inventoryFragmentQuantity - quantity < 0)
        {
            fragmentData.inventoryFragmentQuantity = 0;
            return;
        }
        fragmentData.inventoryFragmentQuantity -= quantity;
    }
    
    public void SetFragment(int quantity)
    {
        fragmentData.inventoryFragmentQuantity = quantity;
    }
}
