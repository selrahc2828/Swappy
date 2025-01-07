using System;
using UnityEngine;

public class RocketMagnetEffect : MonoBehaviour
{
    public Transform targetObject; // L'objet que la zone suit
    public float delay = 1f;       // Délai en secondes pour le PointB

    private Transform _pointA; // PointA attaché directement à l'objet
    private Transform _pointB; // PointB qui suit avec un délai
    private Vector3 _pointBPosition; // Position interpolée de PointB
    
    [SerializeField] private BoxCollider _boxCollider;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Aucun objet cible assigné !");
        }

        _pointA = new GameObject("PointA").transform;
        _pointB = new GameObject("PointB").transform;
        
        _pointA.position = targetObject.position;
        // Initialise PointB à la position actuelle du targetObject
        _pointBPosition = targetObject.position;
        _pointB.position = _pointBPosition;

        _boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (targetObject != null)
        {
            // Point A suit instantanément la position du targetObject
            transform.Find("PointA").position = targetObject.position;

            // Point B suit avec un délai
            _pointBPosition = Vector3.Lerp(_pointBPosition, targetObject.position, Time.deltaTime / delay);
            transform.Find("PointB").position = _pointBPosition;
        }
        
        // Calcul de la position centrale entre PointA et PointB
        Vector3 centerPosition = (_pointA.position + _pointB.position) / 2;
        transform.position = centerPosition;

        // Calcul de la distance entre PointA et PointB
        float distance = Vector3.Distance(_pointA.position, _pointB.position);

        if (_boxCollider is BoxCollider boxCollider)
        {
            // Ajustement du BoxCollider
            //boxCollider.size = new Vector3(colliderWidth, colliderWidth, distance);
            boxCollider.center = Vector3.zero; // Garde le collider centré
            transform.LookAt(_pointB.position); // Oriente le collider
        }
       
    }
}
