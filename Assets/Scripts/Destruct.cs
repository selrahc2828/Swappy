using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ListDestruct
{
    Null,
    Self,
    Generator
}

public class Destruct : Comportment
{
    public ListDestruct type;
    public float lifeTime;
    public float forceSouffle;
    public float radiusSouffle;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine("AutoDestruct");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator AutoDestruct()
    {

        yield return new WaitForSeconds(lifeTime);

        if (prefab)
        {
            GameObject souffle = Instantiate(prefab, transform.position, Quaternion.identity);
            souffle.transform.localScale = transform.localScale * 6;

            Collider[] objsImpact = Physics.OverlapSphere(transform.position, radiusSouffle);

            foreach (Collider obj in objsImpact)
            {
                //Debug.Log(obj.gameObject.name);
                Rigidbody rbObj = obj.GetComponent<Rigidbody>();
                if (rbObj)
                {
                    rbObj.AddExplosionForce(forceSouffle, transform.position, radiusSouffle);
                }
            }
        }
        Destroy(gameObject);

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSouffle);
    }
}
