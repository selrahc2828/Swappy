using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FragmentSystem : MonoBehaviour
{
    public FragmentBankData fragmentBankData;

    public void AddFragment(int quantity)
    {
        fragmentBankData.bankInventoryFragmentQuantity += quantity;
    }

    public void RemoveFragment(int quantity)
    {
        if (fragmentBankData.bankInventoryFragmentQuantity - quantity < 0)
        {
            fragmentBankData.bankInventoryFragmentQuantity = 0;
            return;
        }
        fragmentBankData.bankInventoryFragmentQuantity -= quantity;
    }
    
    public void SetFragment(int quantity)
    {
        fragmentBankData.bankInventoryFragmentQuantity = quantity;
    }

    private void FixedUpdate()
    {
        // overlapSphere autour de player
        // detecte fragment
        // active le follow
        
        // comment detecte si plus en range ? 
        
        // si distance < X add fragment
    }
}
