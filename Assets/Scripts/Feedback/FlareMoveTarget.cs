using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlareMoveTarget : MonoBehaviour
{
    [HideInInspector] public Vector3 startPosition;
    public Transform target;
    public float speed = 2f;
    public AnimationCurve speedCurve; // evolution de la vitesse en fonction de la distance 

    public ParticleSystem flare;
    public Renderer flareRenderer;

    
    public bool attributeToObject;
    
    private float distance; // distance à parcourir
    private float progress = 0f;
    
    void Start()
    {
        flare = GetComponentInChildren<ParticleSystem>();
        flareRenderer = GetComponentInChildren<MeshRenderer>();
        
        if (target == null)
        {
            Debug.LogError("Aucun point cible défini !");
            return;
        }
        startPosition = transform.position;
        SetDistance();
    }

    void Update()
    {
        
        if (target == null) return;
        
        // Vector3 dist = target.position - transform.position;
        if (Vector3.Distance(transform.position, target.position) < .5f)
        {
            flare?.gameObject.SetActive(false);
            transform.position = target.position;

            if (attributeToObject)
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
        }
    }

    public void SetDistance()
    {
        progress = 0f;//reset de la progression en même temps que le recalcule de la distance à parcourir
        distance = Vector3.Distance(startPosition, target.position);
    }
}
