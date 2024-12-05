using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Bounce : Comportment
{
    public PhysicMaterial bouncyMaterial;

    void Start()
    {
        stealable = true;
        gameObject.GetComponent<Collider>().material = bouncyMaterial;
    }

    private void OnDisable()
    {
        gameObject.GetComponent<Collider>().material = null;
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Collider>().material = bouncyMaterial;
    }
}
