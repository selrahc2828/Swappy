using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlareMoveTarget : MonoBehaviour
{
    public Transform target; // Point d'arrivée
    public float speed = 2f; // Vitesse de déplacement
    
    public Transform spawnPosition;
    
    private Vector3 startPosition;
    private float startTime;
    private float journeyLength;
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Aucun point cible défini !");
            return;
        }
        startPosition = transform.position;
        journeyLength = Vector3.Distance(startPosition, target.position);
        startTime = Time.time;
    }

    void Update()
    {
        
        if (target == null) return;

        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, target.position, fractionOfJourney);
    }
}
