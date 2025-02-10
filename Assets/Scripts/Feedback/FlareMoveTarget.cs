using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlareMoveTarget : MonoBehaviour
{
    public Transform target; // Point d'arrivée
    public float speed = 2f; // Vitesse de déplacement
    private ParticleSystem flare;
    
    // public Transform spawnPosition;
    
    private Vector3 startPosition;
    private float startTime;
    private float journeyLength;

    public bool isAttribute;
    void Start()
    {
        flare = GetComponentInChildren<ParticleSystem>();
        
        if (target == null)
        {
            Debug.LogError("Aucun point cible défini !");
            return;
        }
        startPosition = transform.position;
        SetJourneyLenght();
        Debug.Log($"journeyLength : {journeyLength}");
        startTime = Time.time;
    }

    void Update()
    {
        
        if (target == null) return;

        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, target.position, fractionOfJourney);

        Vector3 dist = target.position - transform.position;
        if (dist.magnitude < 0.05f)
        {
            flare?.gameObject.SetActive(false);
            if (isAttribute)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            flare?.gameObject.SetActive(true);
        }
    }

    public void SetJourneyLenght()
    {
        journeyLength = Vector3.Distance(startPosition, target.position);
    }
}
