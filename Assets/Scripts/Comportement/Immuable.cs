using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immuable : Comportment
{
    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Réinitialise la vélocité à zéro
            //rb.angularVelocity = Vector3.zero; // Réinitialise la vélocité angulaire à zéro
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
