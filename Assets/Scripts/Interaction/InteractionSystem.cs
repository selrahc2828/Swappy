using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractionSystem : MonoBehaviour
{
    public abstract string interactionText { get; }
    private void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        
    }
    
    public virtual void Interact()
    {
        
    }

}
