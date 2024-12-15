using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseMagnetZone : MonoBehaviour
{
    public float force;
    public float range;

    //tant que pas de modèle bien scale à 1
    public Transform followTransform;
    public bool isGrabbed;

    private void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Rigidbody>())//player rb dans parent
        {
            if (isGrabbed && other.CompareTag("Player"))//si en main et touche player, ne fait rien
            {
                // Debug.LogWarning($"Not affect {other.name}, isGrabbed : {isGrabbed}");
                return;
            }
            // Debug.LogWarning($"collider stay {other.name}, tag {other.tag}, isGrabbed : {isGrabbed}");

            Vector3 direction = other.transform.position - transform.position;
            
            // float distance = Vector3.Distance(other.transform.position, transform.position);
            // float attenuation = Mathf.Clamp01(1 - (distance / range));
            // other.GetComponentInParent<Rigidbody>().AddForce(direction * force * attenuation, ForceMode.Acceleration);
            
            other.GetComponentInParent<Rigidbody>().AddForce(direction * force, ForceMode.Force);//Acceleration

        }
    }
}
