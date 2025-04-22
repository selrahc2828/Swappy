using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityPlanete : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> planetesRB;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 offsetRotation;
    [SerializeField] private float gravityConstant = 9.81f;
    [SerializeField] private float maxAngle = 30f;

    private float marche;

    void Start()
    {
        for(int i = 0; i < GameManager.Instance.planeteCores.Count; i++)
        {
            planetesRB.Add(GameManager.Instance.planeteCores[i].GetComponent<Rigidbody>());
        }
        gravityConstant = GameManager.Instance.constanteGravitationelle;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Calculer la direction de la gravité par rapport au joueur
        Vector3 gravityDirection = CalculateGravityDirection();
        
        if (rb.CompareTag("Player"))
        {
            RotatePlayer(gravityDirection);
        }
        
        // Vérifier la normale du sol sous le joueur
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 2.5f))
        {
            Vector3 groundNormal = hit.normal;
            float angle = Vector3.Angle(groundNormal, gravityDirection);

            if (angle < maxAngle)
            {
                gravityDirection = groundNormal;
            }
        }
        ApplyGravity(gravityDirection);
    }

    Vector3 CalculateGravityDirection()
    {
        Vector3 gravityVector = Vector3.zero;
        foreach (Rigidbody planeteRB in planetesRB)
        {
            float distance = Vector3.Distance(planeteRB.transform.position, transform.position);
            float gravitationalForceMagnitude = gravityConstant * planeteRB.mass / Mathf.Pow(distance, 2); // F = G * M / r^2
            Vector3 gravitationalForce = (planeteRB.transform.position - transform.position).normalized * gravitationalForceMagnitude;
            gravityVector += gravitationalForce;
            // Appliquer la force au Rigidbody du joueur
            rb.AddForce(gravitationalForce, ForceMode.Acceleration);
        }
        return gravityVector;
    }
    
    void ApplyGravity(Vector3 gravityVector)
    {
        // Appliquer la force au Rigidbody du joueur
        rb.AddForce(gravityVector, ForceMode.Acceleration);
    }

    void RotatePlayer(Vector3 _directionToGravityForce)
    {
        // Calculer la rotation cible pour que le joueur "colle" à la planète
        Quaternion targetRotation = Quaternion.FromToRotation(-transform.up, _directionToGravityForce) * transform.rotation;

        // Appliquer un offset si nécessaire
        targetRotation *= Quaternion.Euler(offsetRotation);

        // Appliquer directement la rotation sans la physique
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Vector3 gravityDirection = Vector3.zero;
        foreach (Rigidbody planeteRB in planetesRB)
        {
            if(planeteRB != null)
            {
                Vector3 directionToPlanet = (transform.position - transform.position).normalized;
                float distance = Vector3.Distance(Vector3.zero, transform.position);
                float gravitationalForceMagnitude = gravityConstant * planeteRB.mass / Mathf.Pow(distance, 2);
                Vector3 gravitationalForce = directionToPlanet * gravitationalForceMagnitude;
                gravityDirection += gravitationalForce;
            }
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + gravityDirection);
    }
}
