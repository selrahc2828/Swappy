using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MagnetEffect : MonoBehaviour
{
    public float magnet_Force;
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == true)
        {
            Vector3 forceDirection = transform.position - other.transform.position;
            other.GetComponent<Rigidbody>().AddForce((forceDirection * magnet_Force), ForceMode.Force);
        } 
    }
}
