using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity_Stealable : Comportment
{
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;
    private bool savedUseGravity;
    private bool saved = false;
    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 20f;
        stealable = true;
        rb = GetComponent<Rigidbody>();
        if (saved)
        {
            RestoreRigidbodyState();
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(this);
        }
    }
    private void OnDisable()
    {
        SaveRigidbodyState();
        ResetRigidbody();
    }
    private void OnEnable()
    {
        if (saved)
        {
            RestoreRigidbodyState();
            rb.useGravity = savedUseGravity;
        }
    }
    public void SaveRigidbodyState()
    {
        if (rb != null)
        {
            savedVelocity = rb.velocity;
            //savedAngularVelocity = rb.angularVelocity;
            savedUseGravity = rb.useGravity;
            saved = true;
        }
    }
    public void RestoreRigidbodyState()
    {
        if (rb != null)
        {
            rb.velocity += savedVelocity;
            //rb.angularVelocity = savedAngularVelocity;
            saved = false;
        }
    }
    public void ResetRigidbody()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Réinitialise la vélocité à zéro
            //rb.angularVelocity = Vector3.zero; // Réinitialise la vélocité angulaire à zéro
            //rb.useGravity = false;
        }
    }
}
