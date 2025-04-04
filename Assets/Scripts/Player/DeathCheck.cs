using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCheck : MonoBehaviour

{
    public float seuilVitesse = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        float vitesseCollision = collision.relativeVelocity.magnitude;

        if (vitesseCollision > seuilVitesse)
        {
            Debug.Log("dead");
        }

    }
}