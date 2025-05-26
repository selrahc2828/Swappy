using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// [RequireComponent(typeof(LineRenderer))]
public class FragmentObject : MonoBehaviour
{
    [SerializeField] private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set => _quantity = value;
    }

    [SerializeField] 
    private float force;
    
    private void Start()
    {
        // Choisir une direction aléatoire
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(randomDirection * force, ForceMode.VelocityChange);
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.Instance.AddFragmentSound(gameObject);
    }
}
