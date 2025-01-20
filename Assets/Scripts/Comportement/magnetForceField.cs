using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetForceField : MonoBehaviour
{
    public Transform originObj;
    public float force;

    public bool isCollide;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && other.gameObject != originObj.gameObject)// applique pas sur player et lui même
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                // if (isCollide) // collision
                // {
                //     ApplyForce(false,other.GetComponent<Rigidbody>(), other.gameObject, trueMagnetForce);
                // }
                // else
                // {
                //     ApplyForce(magnetGradiantForce,other.GetComponent<Rigidbody>(), other.gameObject, trueMagnetForce);
                // }
            }
        }
    }

    void ApplyForce()
    {
        
    }
}
