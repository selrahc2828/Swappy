using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractInteraction : InteractionSystem
{
    public override string interactionText => "Interact"; // "=>" simplification de : get { return "Grab"; }

    public override void Interact()
    {
        base.Interact();
        
        Debug.Log("Interact Object (lever/door)");
    }
}
