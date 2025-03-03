using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityPlanete : MonoBehaviour
{
    [SerializeField] private Rigidbody planeteRB;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 offsetRotation;
    [SerializeField] private float gravityConstant = 9.81f;
    [SerializeField] private float massePlanete;

    private float marche;

    void Start()
    {
        marche = 8;
        planeteRB = GameManager.Instance.planeteCore.GetComponent<Rigidbody>();
        gravityConstant = GameManager.Instance.constanteGravitationelle;
        massePlanete = GameManager.Instance.massePlanete;
        planeteRB.mass = massePlanete;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Calculer la direction de la planète par rapport au joueur
        Vector3 directionToPlanet = (Vector3.zero - transform.position).normalized;

        applyGravity(directionToPlanet);
        if (rb.CompareTag("Player"))
        {
            rotatePlayer(directionToPlanet);
        }
    }


    void applyGravity(Vector3 _directionToPlanet)
    {

        // Appliquer une force gravitationnelle réaliste
        float distance = Vector3.Distance(Vector3.zero, transform.position);
        float gravitationalForceMagnitude = gravityConstant * planeteRB.mass / Mathf.Pow(distance, 2); // F = G * M / r^2
        Vector3 gravitationalForce = _directionToPlanet * gravitationalForceMagnitude;

        // Appliquer la force au Rigidbody du joueur
        rb.AddForce(gravitationalForce, ForceMode.Acceleration);
    }

    void rotatePlayer(Vector3 _directionToPlanet)
    {

        // Calculer la rotation cible pour que le joueur "colle" à la planète
        Quaternion targetRotation = Quaternion.FromToRotation(-transform.up, _directionToPlanet) * transform.rotation;

        // Appliquer un offset si nécessaire
        targetRotation *= Quaternion.Euler(offsetRotation);

        // Appliquer directement la rotation sans la physique
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        if(planeteRB != null)
        {
            Vector3 directionToPlanet = (transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, transform.position);
            float gravitationalForceMagnitude = gravityConstant * planeteRB.mass / Mathf.Pow(distance, 2);
            Vector3 gravitationalForce = directionToPlanet * gravitationalForceMagnitude;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + gravitationalForce);
        }
    }
}
