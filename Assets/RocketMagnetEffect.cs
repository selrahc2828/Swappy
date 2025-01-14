using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketMagnetEffect : MonoBehaviour
{
    public Transform rocketObject; // L'objet que la zone suit, le magnet rocket
    public float delay = 1f;       // Délai en secondes pour le PointB

    public Transform pointA; // PointA attaché directement à l'objet
    public Transform pointB; // PointB qui suit avec un délai
    private Vector3 _pointBPosition; // Position interpolée de PointB
    
    // test rescale
    public Transform trailMagnetObject; // L'objet à scaler
    public float divReplaceCenter = 2f;
    
    void Start()
    {
        if (rocketObject == null)
        {
            Debug.LogError("Aucun objet cible assigné");
        }
        
        Vector3 positionStart = new Vector3(
            rocketObject.position.x,
            rocketObject.position.y - GetComponentInParent<Collider>().bounds.extents.magnitude,
            rocketObject.position.z
            );
        
        pointA.position = positionStart;
        // Initialise PointB à la position actuelle du targetObject
        _pointBPosition = pointA.position;
        pointB.position = _pointBPosition;
    }

    void Update()
    {
        if (rocketObject != null && pointA != null && pointB != null)
        {
            if (rocketObject != null)
            {
                // Point A suit instantanément la position du targetObject
                Vector3 positionMove = new Vector3(
                    rocketObject.position.x,
                    rocketObject.position.y - GetComponentInParent<Collider>().bounds.extents.magnitude,
                    rocketObject.position.z
                );
        
                pointA.position = positionMove;
            }

            // si pas de target mettre rien
            // target B doit êre point A
            
        
            // Point B suit avec un délai
            _pointBPosition = Vector3.Lerp(_pointBPosition, pointA.position, Time.deltaTime / delay);
            pointB.position = _pointBPosition;
            
            RescalePoints();
        }
    }

    private void RescalePoints()
    {
        // Calcul de la distance entre PointA et PointB
        Vector3 direction = pointB.position - pointA.position;
        float distance = direction.magnitude;
        
        //rescale du mesh
        Renderer renderer = trailMagnetObject.GetComponent<Renderer>();
        Vector3 objectSize = renderer.bounds.size; // Taille actuelle de l'objet
        Vector3 scaleFactor = trailMagnetObject.transform.localScale; // Facteur d'échelle actuel
        if (distance <= 0)
            distance = .1f;
        
        float scaleFactorY = Math.Clamp(scaleFactor.y * (distance / objectSize.y), .1f, 1000f);// clamp scale si on a 0
        
        // Calcul de l'échelle proportionnelle
        Vector3 newScale = new Vector3(
            scaleFactor.x ,
            scaleFactorY,
            scaleFactor.z 
        );
        
        // Appliquer la nouvelle échelle
        trailMagnetObject.transform.localScale = newScale;

        // Positionner l'objet pour qu'il soit centré entre les points
        trailMagnetObject.transform.position = (pointA.position + pointB.position) / 2;
    }
}
