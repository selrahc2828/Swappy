using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjInteraction : InteractionSystem
{
    public override string interactionText => "Talk"; // "=>" simplification de : get { return "Grab"; }

    public override void Interact()
    {
        base.Interact();

        Debug.Log("PnjInteraction Interact");
    }
}
