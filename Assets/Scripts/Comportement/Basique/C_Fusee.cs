using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Fusee : Comportment
{
    public float rocketForce;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * rocketForce, ForceMode.Force);
    }
}
