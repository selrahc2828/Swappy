using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Repulse : Comportment
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
    
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        repulserTimer = 0;
        rb = GetComponent<Rigidbody>();
        //material = GetComponent<MeshRenderer>().material;
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
        
       // material.SetFloat("_timer", repulserTimer);
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
                if (objectInRange.gameObject.tag == "Player")
                {
                    GameObject objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>().gameObject;
                    // Debug.Log("objectAffected repulse : " + objectAffected.name);
                    if (impulseGradiantForce)
                    {
                        objectAffected.GetComponent<Rigidbody>().AddExplosionForce(repulserForce, transform.position, repulserRange);
                    }
                    else
                    {
                        objectAffected.GetComponent<Rigidbody>().AddForce((objectInRange.transform.position - transform.position) * repulserForce, ForceMode.Impulse);
                    }

                    // player relache l'objet repulse
                    if (objectAffected.GetComponent<GrabObject>().carriedObject == gameObject)
                    {
                        objectAffected.GetComponent<GrabObject>().Drop(true);
                    }
                }
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
