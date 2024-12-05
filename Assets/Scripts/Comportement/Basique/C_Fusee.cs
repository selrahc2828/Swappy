using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Fusee : Comportment
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * speed, ForceMode.Force);
    }
}
