using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AimAssist : MonoBehaviour
{

    public float radius = 20f;
    public float maxAngleDetection = 1.5f;
    
    private Transform origin;

    public LayerMask detectionLayer;

    public ComportementsStateMachine cible;

    private RaycastHit[] results = new RaycastHit[15];

    private Ray _ray;

    [Header("Debug")] 
    public float dotMax;
    
    void Start()
    {
        origin = GameManager.Instance.mainCamera.transform;
    }

    void Update()
    {
        cible = DetectComportementSphereCast();
    }

    ComportementsStateMachine DetectComportementSphereCast()
    {
        ComportementsStateMachine _cible = null;

        Vector3 dirPlayer = origin.forward;

        //calcul de l'angle le plus proche pour savoir lequel on vise
        float smallestDot = -1; //derriere le joueur, equivalent 360°, plus on a une valeur proche de 1, plus on est aligne à la vue du joueur

        float dotThreshold = Mathf.Cos(maxAngleDetection * Mathf.Deg2Rad); //le seuil, conversion angle en cos
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
            // Vérifie la ligne de vue directe
            Vector3 dirToTarget = (hit.point - origin.position).normalized;

            if (Physics.Raycast(origin.position, dirToTarget, out RaycastHit visibilityHit, Mathf.Infinity))
            {
                if (visibilityHit.collider != hit.collider)
                    continue; // Un obstacle bloque la vue
            }

            ComportementsStateMachine comportement = hit.collider.GetComponent<ComportementsStateMachine>();
            if (comportement is null)
                continue;

            float dot = Vector3.Dot(dirPlayer, dirToTarget);

            if (dot >= dotThreshold && dot > smallestDot)
            {
                smallestDot = dot;
                _cible = hit.collider.gameObject.GetComponent<ComportementsStateMachine>();

                Debug.Log($"{hit.collider.name} - dot: {dot} / smallest {smallestDot}");
            }
        }
        return _cible;
    }

    void OnDrawGizmosSelected()
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