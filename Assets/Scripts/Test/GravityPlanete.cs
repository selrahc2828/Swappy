using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GravityPlanete : MonoBehaviour
{
    public GameObject player;
    public Rigidbody playerRB;
    public Vector3 offsetRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Calculer la direction entre la capsule et la sphère
        Vector3 directionToSphere = (transform.position - player.transform.position).normalized;

        // Conserver la rotation actuelle de la capsule
        Quaternion currentRotation = player.transform.rotation;

        // Calculer la rotation cible pour que le bas de la capsule (-Y) pointe vers la sphère
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, -directionToSphere) * currentRotation;

        // Ajouter une rotation d'offset si nécessaire
        targetRotation *= Quaternion.Euler(offsetRotation);

        // Appliquer la rotation au Rigidbody de la capsule
        playerRB.MoveRotation(targetRotation);
    }
}
