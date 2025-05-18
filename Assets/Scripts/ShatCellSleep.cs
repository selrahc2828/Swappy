using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatCellSleep : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    [SerializeField] private float forceSleepThreshold = 0.05f;
    [SerializeField] private float fallbackSleepTime = 1f; // pour éviter de le faire trop tot

    private float waitTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (rb == null || col == null || rb.isKinematic) 
            return;
        
        // IsSleeping peut ne pas etre active dans certains cas, donc on va faire des controles apres
        if (rb.IsSleeping())
        {
            rb.isKinematic = true;
            col.enabled = false;
            return;
        }
        
        if (rb.velocity.magnitude < forceSleepThreshold)
        {
            waitTime += Time.deltaTime;
            
            if (waitTime >= fallbackSleepTime) // on attend un peu, s'il est encore en train de bouger visuellement
            {
                rb.isKinematic = true;
                col.enabled = false;
            }
        }
        else
        {
            waitTime = 0f;
        }
    }
}
