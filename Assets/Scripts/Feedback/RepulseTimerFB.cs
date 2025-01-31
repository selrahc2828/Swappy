using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulseTimerFB : MonoBehaviour
{
    public ParticleSystem particle_Zone;

    public float repulseTimer;
    private float repulseTime;
    
    
    [HideInInspector]
    public float targetRadius;
    public float durationScaling = .2f;
    public float elapsedTime = 0f;
    private Vector3 _initialScale;
    private Vector3 _targetScale;


    private void Start()
    {
        _initialScale = transform.localScale;

        SetTargetScale(targetRadius);
    }

    // Update is called once per frame
    void Update()
    {
        repulseTime += Time.deltaTime;

        if (repulseTime >= repulseTimer)
        {
            particle_Zone.Play();
            repulseTime = 0;
        }
    }
    
    
    public void SetTargetScale(float radius)
    {
        _targetScale = new Vector3(radius * 2, radius * 2, radius * 2); //* 2 pour appliquer le diametre pas le rayon
    }
}
