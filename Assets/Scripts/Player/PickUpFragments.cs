using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpFragments : MonoBehaviour
{
    private FragmentSystem fragmentSystem;
    public float rangeDetection = 5f;
    public float attractionSpeed = 8f;
    public float pickUpDistance = .5f;// distance pour rammasser
    public LayerMask pickupMask;
    
    public float checkInterval = 0.2f;// lance detection autour du joueur toutes les x secondes
    private float timeSinceLastCheck = 0f;

    private Collider[] fragcolliders; // set pour réutiliser, mon de mémoire utilisé
    
    private List<GameObject> _fragmentPulling = new List<GameObject>(); // liste des fragments en train d'être attiré
    
    private void Start()
    {
        fragcolliders = new Collider[30];//30 pour "voir large", mieux vaut trop de palce que pas assez
        fragmentSystem = FindObjectOfType<FragmentSystem>(); // voir pour ref ailleur
    }

    private void Update()
    {
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            AttractFragments();
            timeSinceLastCheck = 0f;
        }
    }

    private void AttractFragments()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, rangeDetection, fragcolliders, pickupMask);

        for (int i = 0; i < count; i++)
        {
            FragmentObject fragment = fragcolliders[i].GetComponent<FragmentObject>();
            if (fragment is not null)
            {
                // check si tirable => attendre x seconde après le spawn
                
                if (_fragmentPulling.Contains(fragment.gameObject))
                {
                    //si est déjà en train d'être attiré, on ne l'attire pas
                    continue;
                }
                
                _fragmentPulling.Add(fragment.gameObject);
                StartCoroutine(PullingFragment(fragment));
            }
        }
    }

    IEnumerator PullingFragment(FragmentObject fragment)
    {
        while (fragment is not null)
        {
            Vector3 distance = fragment.transform.position - transform.position;
            if (Math.Abs(distance.magnitude) <= pickUpDistance)
            {
                break;
            }
            
            fragment.GetComponent<Rigidbody>().isKinematic = true;
            fragment.GetComponent<GravityPlanete>().enabled = false;
            
            fragment.transform.position = Vector3.MoveTowards(fragment.transform.position, transform.position, attractionSpeed * Time.deltaTime);
            
            yield return null; // attend prochaine frame
        }
        
        _fragmentPulling.Remove(fragment.gameObject);
        fragment.gameObject.SetActive(false);
        fragmentSystem.AddFragment(fragment.GetComponent<FragmentObject>().Quantity);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, rangeDetection);
        Gizmos.DrawWireSphere(transform.position, pickUpDistance);
    }
}
