using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IK_PotController : MonoBehaviour
{
    [Header("Velocity")] 
    public Transform pot;
    Vector3 velocity;
    Vector3 lastVelocity = Vector3.one;
    private Vector3 lastBodyPosition;
    public float bodySmoothness = 8; // contrôle du lissage de la velocite 
                                     // plus elle est haute, plus la vitesse change lentement (fluide)
                                     // plus elle est basse, plus la vitesse suit le mouvement du corps

    public float velocityMultiplier;
    
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
        
        lastBodyPosition = pot.transform.position;
    }

    void FixedUpdate()
    {
        velocity = pot.transform.position - lastBodyPosition;
        velocity = (velocity + bodySmoothness * lastVelocity) / (bodySmoothness + 1f);
        Debug.DrawRay(pot.transform.position, velocity * 10, Color.red);

        
        // si on utilise pas la physique pour bouger le corps : 
        // bodyVelocity = (transform.position - lastBodyPosition) / Time.fixedDeltaTime;
        // lastBodyPosition = transform.position;
        
        for (int i = 0; i < nbLeg; i++)
        {
            // position ideal dans le monde
            Vector3 worldIdealPos = legCubesDefault[i].position; //.parent.TransformPoint(legCubesDefault[i].localPosition)
            
            
            // on doit definir une position "futur" de prediction, sinon la jambe restera en arriere lorsque le corps avance
            // si on prend juste la position par defaut voulu, le coprs continue d'avancer, résultat la jambe est en retard

            float distance = Vector3.Distance(legOriginalPositions[i], worldIdealPos);
            
            // si jambe est trop loin de sa position ideal, on la déplace
            if (distance >= areaOffset && !legMoving[i])
            {
               


                
                //     // Vector3 targetPost = defaultLegTransforms[i].position;
                //     //legTargets[i].position = defaultLegTransforms[i].position;

                Vector3 targetPosition = 
                    legCubesDefault[i].position
                    + Mathf.Clamp(velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) 
                    * (legCubesDefault[i].position - legTargets[i].transform.position) 
                    + velocity * velocityMultiplier;
                Debug.Log($"default: {legCubesDefault[i].position}\n" +
                          $"recalc: {targetPosition}");
                StartCoroutine(MoveLeg(i, targetPosition));
                
                Debug.DrawLine(legTargets[i].position, targetPosition, Color.green);
                Debug.DrawRay(legTargets[i].position, velocity * 10, Color.red);
                
            }
            else
            {
                legTargets[i].position = legOriginalPositions[i];
            }

            //legTargets[i].position = lastLegPosition[i];
        }
        
        lastBodyPosition = pot.transform.position;
        lastVelocity = velocity;
        
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
            Gizmos.DrawSphere(pos, .1f);
        }
    }
}
