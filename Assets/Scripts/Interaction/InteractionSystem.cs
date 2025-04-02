using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractionSystem : MonoBehaviour
{
    public abstract string interactionText { get; }
    private void Start()
    {
    }
    
    public virtual void Interact()
    {
        Debug.Log("Je viens d'interagir youpi");
    }

}
