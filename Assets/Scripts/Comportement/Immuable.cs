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
            rb.velocity = Vector3.zero; // R�initialise la v�locit� � z�ro
            //rb.angularVelocity = Vector3.zero; // R�initialise la v�locit� angulaire � z�ro
            rb.useGravity = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
