using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Immuable : Comportment
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void OnDisable()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

}
