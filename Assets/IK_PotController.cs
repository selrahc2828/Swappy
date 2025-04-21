using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IK_PotController : MonoBehaviour
{
    [Header("References")]

    public Transform[] legTargets; // liste des jambes à bouger
    [FormerlySerializedAs("defaultLegTransforms")] public Transform[] legCubesDefault;// liste des positions par défaut
    
    public Vector3[] legPositions; //position jambe
    [FormerlySerializedAs("lastLegPosition")] public Vector3[] legOriginalPositions; // dernière position / position d'origine
    
    [Header("Step Parameters")]
    public float areaOffset = 1.5f; // zone dans laquelle la jambe doit rester
                                    // si elle est plus loin, la jambe est replacé
    [FormerlySerializedAs("stepOffset")] public float stepHeight = .5f; // hauteur de pas
    public float localPlacement = .3f; // ecart entre corps et jambe
    public AnimationCurve stepCurve;
    public float stepDuration;
    
    public bool[] legMoving;

    public int nbLeg;

    public Rigidbody rb;
    public float velocityMultiplier;
    private Vector3 lastBodyPosition;
    private Vector3 bodyVelocity;
    
    void Start()
    {
        nbLeg = legTargets.Length;
        legPositions = new Vector3[nbLeg];
        legOriginalPositions = new Vector3[nbLeg];

        legMoving = new bool[nbLeg];
        
        for (int i = 0; i < nbLeg; i++)
        {
            
            //Vector3 worldIdealPos = defaultLegTransforms[i].parent.TransformPoint(defaultLegTransforms[i].localPosition);
            
            legTargets[i].position = legCubesDefault[i].position;// set à la position par defaut (ou localPosition)
            
            Vector3 position = legTargets[i].position;

            
            legPositions[i] = position;
            legOriginalPositions[i] = position;
            
            legMoving[i] = false;
        }
        
        //lastBodyPosition = transform.position;

    }

    void FixedUpdate()
    {
        // si on utilise pas la physique pour bouger le corps : 
        // bodyVelocity = (transform.position - lastBodyPosition) / Time.fixedDeltaTime;
        // lastBodyPosition = transform.position;
        
        // Debug.Log($"calc : {bodyVelocity}\nrb : {rb.velocity}");
        // Debug.DrawRay(transform.position, bodyVelocity * 20, Color.green);
        // Debug.DrawRay(transform.position, rb.velocity * 20, Color.red);

        
        for (int i = 0; i < nbLeg; i++)
        {
            // position ideal dans le monde
            Vector3 worldIdealPos = legCubesDefault[i].parent.TransformPoint(legCubesDefault[i].localPosition);
            //
            //
            // //Debug.DrawLine(legTargets[i].position, lastLegPosition[i], Color.green);
            //
            // // on doit definir une position "futur" de prediction, sinon la jambe restera en arriere lorsque le corps avance
            // // si on prend juste la position par defaut voulu, le coprs continue d'avancer, résultat la jambe est en retard
            //
            // // prediction du mouvement / de la position
            //
            // float velocityFactor = Mathf.Clamp(rb.velocity.magnitude * velocityMultiplier, 0f, 1.5f); // mesure de la vitesse de déplacement, qu'on limite pour ne pas avoir un pas trop grand
            // Vector3 correction = worldIdealPos - legTargets[i].position; // vecteur qui indique la direction et la taille pour ramener la jambe à la position idéal
            // Vector3 predictedOffset = velocityFactor * correction + bodyVelocity * velocityMultiplier;
            //
            // Vector3 targetPoint = worldIdealPos + Mathf.Clamp(rb.velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) * (worldIdealPos - legTargets[i].position) + rb.velocity * velocityMultiplier;
            //
            // Vector3 predictedIdeal = worldIdealPos + predictedOffset;
            // Debug.DrawLine(legTargets[i].position, predictedIdeal, Color.magenta);



            float distance = Vector3.Distance(legOriginalPositions[i], worldIdealPos);
            // float distance = Vector3.Distance(lastLegPosition[i], predictedIdeal);

            
            
            // si jambe est trop loin de sa position ideal, on la déplace
            if (distance >= areaOffset && !legMoving[i])
            {
            //     // legTargets[i].position = defaultLegTransforms[i].position;
            //     // Vector3 targetPost = transform.TransformPoint(defaultLegTransforms[i].position);
            //     
            //     
            //
            //     // Vector3 desiredPositions = transform.TransformPoint(defaultLegTransforms[i].localPosition);
            //     
            //     // Vector3 dir = worldIdealPos - legTargets[i].position;
            //     //
            //     // Vector3 targetPoint = defaultLegTransforms[i].position + Mathf.Clamp(rb.velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) * 
            //     //     (dir) + rb.velocity * velocityMultiplier;
            //
            //     
            Vector3 velocityBody = rb.velocity;
            Vector3 dir = worldIdealPos - legTargets[i].position;
            Vector3 targetPosition = worldIdealPos;
            // targetPosition = targetPosition + Mathf.Clamp(velocityBody.magnitude * velocityMultiplier, 0f, 2f) * dir + velocityBody * velocityMultiplier;
            //     
            // Debug.Log($"velocity: {velocityBody}");
            // Vector3 predictedPosition = worldIdealPos + velocityBody * velocityMultiplier;
            
            Debug.DrawLine(legTargets[i].position, targetPosition, Color.green);
            Debug.DrawRay(legTargets[i].position, velocityBody * 10, Color.red);

            
            //     // Vector3 targetPost = defaultLegTransforms[i].position;
            //     //legTargets[i].position = defaultLegTransforms[i].position;
            StartCoroutine(MoveLeg(i, targetPosition));
            //     //StartCoroutine(MoveLeg(i, defaultLegTransforms[i].position));
            }
            else
            {
                legTargets[i].position = legOriginalPositions[i];
            }

            //legTargets[i].position = lastLegPosition[i];
        }
    }

    IEnumerator  MoveLeg(int legIndex, Vector3 targetPosition)
    { 
        legMoving[legIndex] = true;

        float t = 0f;
        Vector3 start = legOriginalPositions[legIndex];

        while (t < 1f)
        {
            t += Time.deltaTime / stepDuration;

            Vector3 mid = Vector3.Lerp(start, targetPosition, t);
            mid.y += stepCurve.Evaluate(t) * stepHeight;

            legTargets[legIndex].position = mid;

            yield return new WaitForFixedUpdate(); // attend prochaine frame physique (en FixedUpdate)	
        }
        
        Debug.Log($"after move {legIndex}");
        legTargets[legIndex].position = targetPosition;
        legPositions[legIndex] = targetPosition;
        legOriginalPositions[legIndex] = targetPosition;

        legMoving[legIndex] = false;
    }
    
    private void OnDrawGizmos()
    {
        if (legTargets.Length == 0)
        {
            return;
        }
        //Gizmos.color = Color.green;
        foreach (Transform pos in legCubesDefault)
        {
            // Gizmos.DrawWireSphere(pos.position, areaOffset);
            Gizmos.DrawWireSphere(pos.position, areaOffset);
        }
        Gizmos.color = Color.yellow;
        foreach (Transform pos in legTargets)
        {
            // Gizmos.DrawWireSphere(pos.position, areaOffset);
            Gizmos.DrawWireSphere(pos.position, areaOffset);
        }
        Gizmos.color = Color.blue;
        foreach (Vector3 pos in legOriginalPositions)
        {
            // Gizmos.DrawWireSphere(pos.position, areaOffset);
            Gizmos.DrawSphere(pos, .02f);
        }
    }
}
