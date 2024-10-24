using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn_after_time : MonoBehaviour
{
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    public Rigidbody rb;

    public bool startTimer = false;
    public float respawnTime = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        respawnTime = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            respawnTime -= Time.deltaTime;
            if (respawnTime <= 0)
            {
                startTimer = false;
                transform.position = spawnPosition;
                transform.rotation = spawnRotation;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                respawnTime = 4f;
            }
        }

        
    }

    private void OnCollisionEnter(Collision other)
    {
        startTimer = true;
    }
}
