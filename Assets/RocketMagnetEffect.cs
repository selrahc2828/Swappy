using System;
using UnityEngine;

public class RocketMagnetEffect : MonoBehaviour
{
    public Transform rocketObject; // L'objet que la zone suit, le magnet rocket
    public float delay = 1f;       // délais de B vers A, "temps de vie" du dernier point qui va suivre
    
    public Transform pointA; // PointA attaché directement à l'objet
    public Transform pointB; // PointB qui suit A avec un délai
    private Vector3 _pointBPosition; // Position interpolée de PointB
    
    public Transform trailMagnetObject; // L'objet à scaler
    public bool atDetachAndDestroy = false; // true quand la rocket se stop, on attend que la trainé se termine pour détruire
    
    [Header("Forces")]
    public float effectForce = 20f;
    public float effectForceOnPlayer = 20f;
    public float effectForceWhenGrab = 20f;
    public float timeBeforeMove = 3f;
    private float _timer;
    
    void Start()
    {
        if (rocketObject == null)
        {
            Debug.LogError("Aucun objet cible assigné");
            return;
        }
        Vector3 positionStart = pointA.position;
        if (!atDetachAndDestroy)
        {
            positionStart = new Vector3(
                rocketObject.position.x,
                rocketObject.position.y - rocketObject.GetComponent<Collider>().bounds.extents.magnitude,
                rocketObject.position.z
                );            
        }
        
        pointA.position = positionStart;
        // Initialise PointB à la position actuelle du targetObject
        _pointBPosition = pointA.position;
        pointB.position = _pointBPosition;
        
        // on retire pointB du parent temporairement,
        // sinon il suis la position de rocketObject alors que le timer pour le faire commencer à bougé n'est pas atteint
        // pointB.parent = null;
    }

    void Update()
    {
        if (rocketObject != null && pointA != null && pointB != null)
        {
            SetPositionPoints();
            RescalePoints();
            
            // si distance entre A et B quasi 0, et rocket stop, on retire du parent et on le détruit
            if (atDetachAndDestroy)
            {
                if (transform.parent != null)
                {
                    transform.parent = null;
                }
                
                if (Vector3.Distance(pointA.position, pointB.position) <= 0.2f)
                {
                    Destroy(gameObject);
                }
            }
        }

        MagnetTrailForce();
    }

    void MagnetTrailForce()
    {
        
        Vector3 boxCenter = (pointA.position + pointB.position) / 2;
        // _boxCenter = interactorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);

        Vector3 boxHalfExtents = new Vector3(
            trailMagnetObject.transform.localScale.x / 2,
            Vector3.Distance(pointA.position, pointB.position) / 2,
            trailMagnetObject.transform.localScale.z / 2 
        );
        
        Collider[] objectsInRange = Physics.OverlapBox(boxCenter, boxHalfExtents);
        
        if (objectsInRange.Length > 0)
        {
            foreach (Collider objectInRange in objectsInRange)
            {

                if (objectInRange.gameObject.CompareTag("Player"))
                {
                    //collider et rigid body pas au même endroit pour lui
                    Rigidbody objectAffected = objectInRange.gameObject.GetComponentInParent<Rigidbody>();
                    
                    // pb pour appliquer la force à cause du drag sur le rigidbody
                    ApplyForce(objectAffected,effectForce);
                }
                else if (objectInRange.GetComponent<Rigidbody>() != null)
                {
                    ApplyForce(objectInRange.GetComponent<Rigidbody>(),effectForce);
                }  
            }
        }
    }
    public void ApplyForce(Rigidbody rbObj, float force)
    {
        rbObj.AddForce(Vector3.up * effectForce, ForceMode.Force);
    }
    
    private void SetPositionPoints()
    {
        if (rocketObject != null)
        {
            // Point A suit instantanément la position du targetObject
            Vector3 positionMove = pointA.position;

            if (!atDetachAndDestroy)
            {
                positionMove = new Vector3(
                    rocketObject.position.x,
                    rocketObject.position.y - rocketObject.GetComponent<Collider>().bounds.extents.magnitude,
                    rocketObject.position.z
                );
            }
            
            pointA.position = positionMove;
        }
        
        // le but dans le proto est que la zone soit toujours sous rocketObject, sans prendre en compte les angles
        // X et Z doivent toujours être update du coup
        _pointBPosition.x = pointA.position.x;
        _pointBPosition.z = pointA.position.z;
        
        // Point B suit avec un délai
        
        if (_timer >= timeBeforeMove)
        {
            // pointB.parent = transform;
            // _pointBPosition = Vector3.Lerp(_pointBPosition, pointA.position, Time.deltaTime / delay);
            _pointBPosition.y = Mathf.Lerp(_pointBPosition.y, pointA.position.y, Time.deltaTime / delay);

            // pointB.position = _pointBPosition;
        }
        else
        {
            _timer += Time.deltaTime;
        }

        pointB.position = _pointBPosition;
        
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
            scaleFactor.x,
            scaleFactorY,
            scaleFactor.z 
        );
        
        // Appliquer la nouvelle échelle
        trailMagnetObject.transform.localScale = newScale;

        // Positionner l'objet pour qu'il soit centré entre les points
        trailMagnetObject.transform.position = (pointA.position + pointB.position) / 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        // Meme calcul pour le gizmo de la box de detection
        Vector3 boxCenter = (pointA.position + pointB.position) / 2;
        // Quaternion boxRotation = .transform.rotation;

        // Pour dessiner la boite dans la scene avec Gizmos (comme avec Physics.OverlapBox)
        // Matrix4x4.TRS permet de dessiner, position, rotation et echelle, juste DrawWireCube ne suffit pas 
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, trailMagnetObject.localScale);            
            
    }
}
