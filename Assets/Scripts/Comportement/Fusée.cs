using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusée : Comportment
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * speed, ForceMode.Force);
        Debug.Log(rb.velocity.y);
    }
}
