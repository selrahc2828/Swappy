using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet_Effect : MonoBehaviour
{
    public float magnet_Force;
    private void OnTriggerStay(Collider other)
    {
        Vector3 forceDirection = transform.position - other.transform.position;
        other.GetComponent<Rigidbody>().AddForce((forceDirection * magnet_Force), ForceMode.Force);
    }
}
