using System.Collections.Generic;
using System;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("La copie scindee de l'objet a instantier lors de sa destruction")]
    public GameObject shatteredVersion;
    [Tooltip("energie cinetique minimum a partir duquel une collision detruit l'objet (Ec = m/2 * v�)")]
    [SerializeField] private float minShatterPower;
    [Tooltip("Avoid breaking object with regular collisions and force")]
    [SerializeField] private bool avoidCollisionBreak;

    private Rigidbody thisRb;
    private Vector3 previousVelocity;
    private Vector3 currentVelocity;
    private Vector3 previousAngularVelocity;
    private Vector3 currentAngular;
    private bool hasShattered;

    private SpawnPot _spawner;
    public SpawnPot Spawner
    {
        set => _spawner = value;
    }

    public GameObject fragmentPrefab;

    private void Start()
    {
        thisRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        currentAngular = thisRb.angularVelocity;
        previousAngularVelocity = thisRb.angularVelocity;

        previousVelocity = currentVelocity;
        currentVelocity = thisRb.velocity;

        if (!avoidCollisionBreak)
        {
            if (thisRb.mass * thisRb.velocity.magnitude * Vector3.Angle(currentVelocity, previousVelocity) / 3 >= minShatterPower)
            {
                if (!hasShattered)
                {
                    hasShattered = true;
                    ApplyCurrentVelocity(ShatterObject());
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!avoidCollisionBreak)
        {
            if (collision.rigidbody == null)
            {
                return;
            }

            Rigidbody rb = collision.rigidbody;
            float cineticForce = (rb.mass / 2) * (rb.velocity.magnitude * rb.velocity.magnitude);

            if (cineticForce >= minShatterPower)
            {
                if (!hasShattered)
                {
                    hasShattered = true;
                    ApplyCurrentVelocity(ShatterObject());
                }
            }
        }
    }

    public Rigidbody[] ShatterObject()
    {
        GameObject shatteredObject = Instantiate(shatteredVersion, transform.position, transform.rotation);
        Rigidbody[] shatteredRbs = shatteredObject.GetComponentsInChildren<Rigidbody>();

        if (_spawner is not null)
        {
            GlobalEventManager.Instance.BrokenPot(gameObject);
            _spawner.isBroken = true;
            ExplodeFragments();
        }

        Destroy(gameObject);
        return shatteredRbs;
    }

    private void ApplyCurrentVelocity(Rigidbody[] rbs)
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.velocity = currentVelocity;
            rb.angularVelocity = currentAngular;
        }
    }

    private void ExplodeFragments()
    {
        for (int i = 0; i < (int)_spawner.potData.tier; i++)
        {
            Instantiate(fragmentPrefab, transform.position + new Vector3(0f,0.5f,0f), Quaternion.identity);
        }
    }
}
