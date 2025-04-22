using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableInteraction : InteractionSystem
{
    public override string interactionText => "Grab"; // "=>" simplification de : get { return "Grab"; }

    public override void Interact()
    {
        base.Interact();
        
        Debug.Log("MovableInteraction Interact");
        
        if (!GameManager.Instance.grabScript.isCarrying)
        {
            if (gameObject.GetComponent<Rigidbody>())//on est closestObject ici
            {
                GameManager.Instance.grabScript.objToCarry = gameObject;
                //set obj que l'on peut grab
                GameManager.Instance.grabScript.Carry();
            }
        }
    }
}
