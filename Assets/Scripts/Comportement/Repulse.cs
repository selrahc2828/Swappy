using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulse : Comportment
{
    public float repulserTime = 5f;
    public float repulserTimer;
    public float repulserRange;
    public float repulserForce;
    public bool destroyOnUse = false;
    
    // Start is called before the first frame update
    void Start()
    {
        repulserTimer = 0;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        repulserTimer += Time.deltaTime;
        if (repulserTimer >= repulserTime)
        {
            Expulse();
            repulserTimer = 0;
        }
    }

    public void Expulse()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, repulserRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    objectInRange.GetComponent<Rigidbody>().AddExplosionForce(repulserForce, transform.position, repulserRange);
                }  
            }
        }

        if (destroyOnUse)
        {
            Destroy(gameObject);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, repulserRange);
    }
}
