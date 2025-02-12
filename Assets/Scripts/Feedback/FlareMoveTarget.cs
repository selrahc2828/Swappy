using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlareMoveTarget : MonoBehaviour
{
    private Vector3 startPosition;
    public Transform target; // Point d'arrivée
    public float speed = 2f; // Vitesse de déplacement
    private ParticleSystem flare;

    public AnimationCurve speedCurve;
    
    // public Transform spawnPosition;
    
    //private float startTime;
    private float distance;
    public float duration;
    private float elapsedTime;
    public bool isAttribute;

    public float progress = 0f;
    
    void Start()
    {
        flare = GetComponentInChildren<ParticleSystem>();
        
        if (target == null)
        {
            Debug.LogError("Aucun point cible défini !");
            return;
        }
        startPosition = transform.position;
        SetDistance();

        //startTime = Time.time;
    }

    void Update()
    {
        
        if (target == null) return;
        
        // Vector3 dist = target.position - transform.position;
        if (Vector3.Distance(transform.position, target.position) < .2f)
        {
            flare?.gameObject.SetActive(false);
            transform.position = target.position;

            if (isAttribute)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            flare?.gameObject.SetActive(true);
            
            // On évalue la courbe en fonction de la progression actuelle.
            // La courbe peut, par exemple, donner une valeur élevée (vitesse maximale)
            // lorsque progress est faible (objet loin de la cible) et décroître à l'approche.
            float speedMultiplier = speedCurve.Evaluate(progress);
            float currentSpeed = speed * speedMultiplier;

            // On incrémente la progression en fonction de la vitesse actuelle.
            // nomalise le déplacement pour que progress atteigne 1 à l'arrivée.
            progress += (currentSpeed * Time.deltaTime) / distance;
            progress = Mathf.Clamp01(progress);

            // On met à jour la position en interpolant entre la position de départ et la cible
            transform.position = Vector3.Lerp(startPosition, target.position, progress);

            // Debug.Log($"Progression : {progress:F2} \n" +
            //           $"speedMultiplier : {speedMultiplier}" +
            //           $"Distance restante : {Vector3.Distance(transform.position, target.position):F2} \n" +
            //           $"Coefficient de vitesse : {speedMultiplier:F2} \n" +
            //           $"Vitesse actuelle : {currentSpeed:F2}");
        }
    }

    public void SetDistance()
    {
        distance = Vector3.Distance(startPosition, target.position);
    }
}
