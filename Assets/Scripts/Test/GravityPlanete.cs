using System;
using UnityEngine;

public class GravityPlanete : MonoBehaviour
{
    public GameObject player;
    public Rigidbody playerRB;
    public Rigidbody rb;
    public Vector3 offsetRotation;

    public float gravityConstant = 9.81f;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        playerRB.useGravity = false;
    }
    
    void FixedUpdate()
    {
        // Calculer la direction de la planète par rapport au joueur
        Vector3 directionToPlanet = (transform.position - player.transform.position).normalized;

        // Appliquer une force gravitationnelle réaliste
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float gravitationalForceMagnitude = gravityConstant * rb.mass / Mathf.Pow(distance, 2); // F = G * M / r^2
        Vector3 gravitationalForce = directionToPlanet * gravitationalForceMagnitude;

        // Appliquer la force au Rigidbody du joueur
        playerRB.AddForce(gravitationalForce, ForceMode.Acceleration);

        // Calculer la rotation cible pour que le joueur "colle" à la planète
        Quaternion targetRotation = Quaternion.FromToRotation(-player.transform.up, directionToPlanet) * player.transform.rotation;

        // Appliquer un offset si nécessaire
        targetRotation *= Quaternion.Euler(offsetRotation);

        // Appliquer la rotation au joueur
        playerRB.MoveRotation(targetRotation);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 directionToPlanet = (transform.position - player.transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            float gravitationalForceMagnitude = gravityConstant * rb.mass / Mathf.Pow(distance, 2);
            Vector3 gravitationalForce = directionToPlanet * gravitationalForceMagnitude;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(player.transform.position, player.transform.position + gravitationalForce);
        }
    }
}
