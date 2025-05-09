using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class FragmentScript : MonoBehaviour
{
    public int quantity;
    public float force = 30f;
    private Rigidbody _rb;
    
    public Transform target;
    public float waitTime = 2f;
    public float lifeTime = 10f;

    public bool followWaittingAllow; // temps d'attente avant de follow player
    public bool followActive; //set player

    //si tous les enfants sont desactivé, détruit obj (parent)
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;// voir si ref ailleur

        StartCoroutine(WaitAndFollow());
        StartCoroutine(LifeTimer());
    }
    
    void FixedUpdate()
    {
        if (followWaittingAllow && target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            _rb.AddForce(direction * force);           
        }
    }
    
    IEnumerator WaitAndFollow()
    {
        yield return new WaitForSeconds(waitTime);
        followWaittingAllow = true;
    }
    
    IEnumerator  LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
