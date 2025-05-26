using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentBeforeLeaveInteraction : InteractionSystem
{
    public override string interactionText => "Talk"; // "=>" simplification de : get { return "Grab"; }

    public override void Interact()
    {
        base.Interact();
        
        FragmentSystem.Instance.AddFragment(FragmentSystem.Instance.fragmentBankData.hasCollect);
        FragmentSystem.Instance.SetFragmentHasCollect(0);
    }
    
    public override string InteractionText(string key)
    {
        string textInteraction = "Press";
        int hasCollect = FragmentSystem.Instance.fragmentBankData.hasCollect;
        string nameFragment = FragmentSystem.Instance.fragmentBankData.fragmentBankName;
        
        return $"{textInteraction} {key} to take: {hasCollect} {nameFragment}";
    }
}
