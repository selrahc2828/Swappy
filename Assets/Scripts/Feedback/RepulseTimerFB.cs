using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulseTimerFB : MonoBehaviour
{
    public ParticleSystem particle_Zone;

    public float repulseTimer;
    private float repulseTime;
    

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
}
