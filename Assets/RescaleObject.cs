using System;
using UnityEngine;

public class RescaleObject : MonoBehaviour
{
    public GameObject targetObject; // L'objet à redimensionner
    public Transform pointA;        // Point A (Transform)
    public Transform pointB;        // Point B (Transform)

    void Update()
    {
        if (targetObject != null && pointA != null && pointB != null)
        {
            RescaleBetweenPoints();
        }
    }

    void RescaleBetweenPoints()
    {
        // Obtenir les dimensions entre les deux points
        Vector3 direction = pointB.position - pointA.position;
        float distance = direction.magnitude; // Distance entre pointA et pointB

        // Récupérer les dimensions de l'objet cible
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("L'objet cible n'a pas de Renderer.");
            return;
        }

        Vector3 objectSize = renderer.bounds.size; // Taille actuelle de l'objet
        Vector3 scaleFactor = targetObject.transform.localScale; // Facteur d'échelle actuel
        if (distance <= 0)
            distance = .1f;
        
        float scaleFactorY = Math.Clamp(scaleFactor.y * (distance / objectSize.y), .1f, 1000f);
        
        // Calcul de l'échelle proportionnelle
        Vector3 newScale = new Vector3(
            scaleFactor.x ,
            scaleFactorY,
            scaleFactor.z 
        );

        // Appliquer la nouvelle échelle
        targetObject.transform.localScale = newScale;

        // Positionner l'objet pour qu'il soit centré entre les points
        targetObject.transform.position = (pointA.position + pointB.position) / 2;
    }
}