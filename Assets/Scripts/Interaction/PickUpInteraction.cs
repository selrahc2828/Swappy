using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteraction : InteractionSystem
{
    public override string interactionText => "Pickup"; // "=>" simplification de : get { return "Grab"; }
    
    public override void Interact()
    {
        base.Interact();

        Debug.Log("PickUpInteraction Interact");
    }
}
