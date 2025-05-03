using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IK_PotController : MonoBehaviour
{
    [Header("Velocity")] 
    public Transform pot;
    Vector3 _velocity;
    Vector3 _lastVelocity = Vector3.one;
    private Vector3 _lastBodyPosition;
    public float bodySmoothness = 8; // contrôle du lissage de la velocite 
                                     // plus elle est haute, plus la vitesse change lentement (fluide)
                                     // plus elle est basse, plus la vitesse suit le mouvement du corps

    public float velocityMultiplier;
    
    [Header("References")]
    public Transform[] legTargets; // liste des jambes à bouger
    public Transform[] legCubesDefault;// liste des positions par défaut
    
    private Vector3[] _legPositions; //position jambe
    private Vector3[] _legOriginalPositions; // dernière position / position d'origine
    
    [Header("Step Parameters")]
    public float areaOffset = 1.5f; // zone dans laquelle la jambe doit rester
                                    // si elle est plus loin, la jambe est replacé
    public float stepHeight = .5f; // hauteur de pas
    public float localPlacement = .3f; // ecart entre corps et jambe
    public AnimationCurve stepCurve;
    public float stepDuration;
    public float waitTimeBetweenTwoSteps = 1f;
    
    public bool[] legMoving;

    public int nbLeg;


    
    void Start()
    {
        nbLeg = legTargets.Length;
        _legPositions = new Vector3[nbLeg];
        _legOriginalPositions = new Vector3[nbLeg];

        legMoving = new bool[nbLeg];
        
        for (int i = 0; i < nbLeg; i++)
        {
            
            //Vector3 worldIdealPos = defaultLegTransforms[i].parent.TransformPoint(defaultLegTransforms[i].localPosition);
            
            legTargets[i].position = legCubesDefault[i].position;// set à la position par defaut (ou localPosition)
            
            Vector3 position = legTargets[i].position;

            
            _legPositions[i] = position;
            _legOriginalPositions[i] = position;
            
            legMoving[i] = false;
        }
        
        _lastBodyPosition = pot.transform.position;
    }

    void FixedUpdate()
    {
        _velocity = pot.transform.position - _lastBodyPosition;
        _velocity = (_velocity + bodySmoothness * _lastVelocity) / (bodySmoothness + 1f);
        Debug.DrawRay(pot.transform.position, _velocity * 15, Color.blue);
        
        
        for (int i = 0; i < nbLeg; i++)
        {
            // position ideal dans le monde
            Vector3 worldIdealPos = legCubesDefault[i].position; //.parent.TransformPoint(legCubesDefault[i].localPosition)
            
            float distance = Vector3.Distance(_legOriginalPositions[i], worldIdealPos);
            
            // si jambe est trop loin de sa position ideal, on la déplace
            if (distance >= areaOffset && !legMoving[i])
            {
                // on doit definir une position "future" de prediction, sinon la jambe restera en arriere lorsque le corps avance
                // si on prend juste la position par defaut voulu, le coprs continue d'avancer, resultat la jambe est en retard
                Vector3 targetPosition = 
                    legCubesDefault[i].position
                    + Mathf.Clamp(_velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) 
                    * (legCubesDefault[i].position - legTargets[i].transform.position) 
                    + _velocity * velocityMultiplier;
                
                StartCoroutine(MoveLeg(i, targetPosition));
            }
            else
            {
                legTargets[i].position = _legOriginalPositions[i];
            }
        }
        
        _lastBodyPosition = pot.transform.position;
        _lastVelocity = _velocity;
    }

    IEnumerator  MoveLeg(int legIndex, Vector3 targetPosition)
    { 
        legMoving[legIndex] = true;

        float t = 0f;
        Vector3 start = _legOriginalPositions[legIndex];

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
        _legPositions[legIndex] = targetPosition;
        _legOriginalPositions[legIndex] = targetPosition;

        for (int i = 1; i <= waitTimeBetweenTwoSteps; ++i) yield return new WaitForFixedUpdate();
        //temps d'attente avant que la jambe refasse un pas
        
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
            Gizmos.DrawWireSphere(pos.position, areaOffset);
        }
        Gizmos.color = Color.yellow;
        foreach (Transform pos in legTargets)
        {
            Gizmos.DrawWireSphere(pos.position, areaOffset);
        }
        Gizmos.color = Color.blue;
        foreach (Vector3 pos in _legOriginalPositions)
        {
            Gizmos.DrawSphere(pos, .1f);
        }
    }
}
