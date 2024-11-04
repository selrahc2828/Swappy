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
    public bool impulseGradiantForce = false;
    public GameObject feedback;
    [Header("Si Rigidbody sur lui")]
    public bool applyOnMe = false; // si rigid body sur objet, pour le lancer par exemple

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
        if (feedback)
        {
            GameObject shockWave = Instantiate(feedback, transform.position, Quaternion.identity);
            shockWave.GetComponent<GrowToRadius>().targetRadius = repulserRange;
        }

        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, repulserRange);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    if (!applyOnMe && objectInRange.gameObject == gameObject)
                    {
                        // si rigid body sur objet, on applique pas la force sur lui pour le lancer par exemple
                        return;
                    }

                    if (impulseGradiantForce)
                    {
                        objectInRange.GetComponent<Rigidbody>().AddExplosionForce(repulserForce, transform.position, repulserRange);

                    }
                    else
                    {
                        objectInRange.GetComponent<Rigidbody>().AddForce((objectInRange.transform.position - transform.position) * repulserForce, ForceMode.Impulse);
                    }
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
