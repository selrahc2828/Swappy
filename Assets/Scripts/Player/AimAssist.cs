using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AimAssist : MonoBehaviour
{

    public float radius;
    public float maxAngleDetection;
    public Transform origin;
    
    public LayerMask detectionLayer;
    
    public ComportementsStateMachine cible;
    
    private RaycastHit[] results = new RaycastHit[15];

    public AnimationCurve angleToleranceByDistance; // distance [0-max], valeur = angle toléré en degrés
    private Ray _ray;

    [Header("Debug")] 
    public float dotMax;
    void Start()
    {
        origin = GameManager.Instance.mainCamera.transform;
    }

    void Update()
    {
        // cible = DetectComportementRayCast();
        cible = DetectComportementSphereCast();
        if (cible != null)
        {
            // Exemple d'effet : on affiche le nom dans la console
            Debug.Log("Cible visée : " + cible.name);
        }
    }

    ComportementsStateMachine DetectComportementSphereCast()
    {
        ComportementsStateMachine _cible = null;
        
        Vector3 dirPlayer = origin.forward;

        //calcul de l'angle le plus proche pour savoir lequel on vise
        float smallestDot = -1; //derriere le joueur, equivalent 360°, plus on a une valeur proche de 1, plus on est aligne à la vue du joueur
            
        float dotThreshold= Mathf.Cos(maxAngleDetection * Mathf.Deg2Rad);//le seuil, conversion angle en cos
        dotMax = dotThreshold;
        
        
        _ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(origin.position, dirPlayer * 100f, Color.red);

        if (Physics.Raycast(_ray, out var hitTest, Mathf.Infinity, detectionLayer))
        {
            ComportementsStateMachine comportement = hitTest.collider.GetComponent<ComportementsStateMachine>();
            if (comportement is not null)
                return comportement;
        }
        
        results = Physics.SphereCastAll(origin.position, radius, dirPlayer, Mathf.Infinity, detectionLayer);
        
        foreach (RaycastHit hit in results)
        {
            ComportementsStateMachine comportement = hit.collider.GetComponent<ComportementsStateMachine>();
            if (comportement is null)
                continue;

            //Vector3 dirToTarget = (hit.collider.transform.position - origin.position).normalized;
            // il faudrait utiliser hit.collider.bounds.center, meilleur pratique mais demande plus de test,
            // car juste lui, le centre doit absolument être dans le dot sinon pas detecte
            
            Vector3 closestPoint = hit.collider.ClosestPoint(origin.position);
            Vector3 dirToTarget = (closestPoint - origin.position).normalized;

            
            
            float dot = Vector3.Dot(dirPlayer,dirToTarget);
            
            
            if (dot >= dotThreshold && dot > smallestDot)
            {
                smallestDot = dot;
                _cible = hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
                
                Debug.Log($"{hit.collider.name} - dot: {dot} / smallest {smallestDot}");

            }
        }
        
        return _cible;
    }

    GameObject DetectComportementRayCast()
    {
        GameObject _cible = null;
        float bestDot = -1f;
        float dotThreshold = Mathf.Cos(maxAngleDetection * Mathf.Deg2Rad);

        Vector3 originPos = origin.position;
        Vector3 forward = origin.forward;

        int raysPerAxis = 4; // grille 3x3 = 9 raycasts
        float angleStep = maxAngleDetection / (raysPerAxis - 1);
        float rayDistance = 100f;

        for (int y = 0; y < raysPerAxis; y++)
        {
            for (int x = 0; x < raysPerAxis; x++)
            {
                // Centrer autour de la direction frontale
                float xAngle = (x - (raysPerAxis - 1) / 2f) * angleStep;
                float yAngle = (y - (raysPerAxis - 1) / 2f) * angleStep;

                Quaternion rotation = Quaternion.Euler(yAngle, xAngle, 0);
                Vector3 rayDirection = rotation * forward;

                // 💡 Affiche le rayon dans la scène (jaune si rien touché, rouge si hit)
                Color rayColor = Color.yellow;

                if (Physics.Raycast(originPos, rayDirection, out RaycastHit hit, rayDistance, detectionLayer))
                {
                    rayColor = Color.red;

                    var comportement = hit.collider.GetComponent<ComportementsStateMachine>();
                    if (comportement != null)
                    {
                        Vector3 dirToTarget = (hit.point - originPos).normalized;
                        float dot = Vector3.Dot(forward, dirToTarget);

                        if (dot >= dotThreshold && dot > bestDot)
                        {
                            bestDot = dot;
                            _cible = hit.collider.gameObject;
                        }
                    }
                }

                Debug.DrawRay(originPos, rayDirection * rayDistance, rayColor);
            }
        }

        return _cible;
    }

    GameObject DetectTarget()
    {
        Collider[] hits = Physics.OverlapSphere(origin.position, 500f, detectionLayer);
        float bestScore = float.MaxValue;
        GameObject bestTarget = null;

        foreach (var hit in hits)
        {
            var comportement = hit.GetComponent<ComportementsStateMachine>();
            if (comportement == null) continue;

            Vector3 dirToTarget = (hit.transform.position - origin.position);
            float distance = dirToTarget.magnitude;

            if (distance > 500f) continue;

            float angle = Vector3.Angle(origin.forward, dirToTarget.normalized);
            float angleTolerance = angleToleranceByDistance.Evaluate(distance); // e.g. 2° à 2m, 10° à 50m...

            if (angle <= angleTolerance)
            {
                // Optional: check line of sight
                if (Physics.Linecast(origin.position, hit.transform.position, out RaycastHit lineHit))
                {
                    if (lineHit.collider != hit) continue;
                }

                if (angle < bestScore)
                {
                    bestScore = angle;
                    bestTarget = comportement.gameObject;
                }
            }
        }

        return bestTarget;
    }
    
    
    void OnDrawGizmos()
    {
        if (origin == null) 
            return;

        float rangeView = 100f;
        
        float angle = maxAngleDetection;

        Vector3 originPos = origin.position;
        Vector3 forward = origin.forward;

        // Angle horizontal/vertical
        Quaternion upRot = Quaternion.AngleAxis(angle, origin.right);
        Quaternion downRot = Quaternion.AngleAxis(-angle, origin.right);
        Quaternion leftRot = Quaternion.AngleAxis(-angle, origin.up);
        Quaternion rightRot = Quaternion.AngleAxis(angle, origin.up);

        Vector3 up = upRot * forward;
        Vector3 down = downRot * forward;
        Vector3 left = leftRot * forward;
        Vector3 right = rightRot * forward;

        Gizmos.color = Color.green;

        // Lignes limites du cône
        Gizmos.DrawRay(originPos, up * rangeView);
        Gizmos.DrawRay(originPos, down * rangeView);
        Gizmos.DrawRay(originPos, left * rangeView);
        Gizmos.DrawRay(originPos, right * rangeView);

        // Cercle pour illustrer la base du cône
        int segments = 32;
        for (int i = 0; i < segments; i++)
        {
            float angleA = ((float)i / segments) * 360f;
            float angleB = ((float)(i + 1) / segments) * 360f;

            Quaternion rotA = Quaternion.AngleAxis(angleA, origin.forward);
            Quaternion rotB = Quaternion.AngleAxis(angleB, origin.forward);

            Vector3 dirA = Quaternion.AngleAxis(angle, origin.right) * rotA * origin.forward;
            Vector3 dirB = Quaternion.AngleAxis(angle, origin.right) * rotB * origin.forward;

            Gizmos.DrawLine(originPos + dirA * rangeView, originPos + dirB * rangeView);
        }
        
        
        
        
        // Couleur du SphereCast
        Gizmos.color = Color.cyan;

        Vector3 endPoint = originPos + forward * rangeView;

        // Dessine les sphères au début et à la fin
        Gizmos.DrawWireSphere(originPos, radius);
        Gizmos.DrawWireSphere(endPoint, radius);

        // Cylindre simulé entre les 2 sphere
        Gizmos.DrawLine(originPos + origin.right * radius, endPoint + origin.right * radius);
        Gizmos.DrawLine(originPos - origin.right * radius, endPoint - origin.right * radius);
        Gizmos.DrawLine(originPos + origin.up * radius, endPoint + origin.up * radius);
        Gizmos.DrawLine(originPos - origin.up * radius, endPoint - origin.up * radius);
    }
    
}
