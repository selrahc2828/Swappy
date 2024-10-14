using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : Comportment
{
    public PhysicMaterial bouncyMaterial;

    // Start is called before the first frame update
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
