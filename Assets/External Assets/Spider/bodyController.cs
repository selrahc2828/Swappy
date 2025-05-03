using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyController : MonoBehaviour
{
    public Rigidbody rb;
    public AnimationCurve curve;
    Vector3 velocity;
    Vector3 lastVelocity = Vector3.one;
    Vector3 lastSpiderPosition;
    Vector3[] legPositions;
    Vector3[] legOriginalPositions;
    [SerializeField] List<int> nextIndexToMove = new List<int>();
    List<int> IndexMoving = new List<int>();
    Vector3 lastBodyUp;
    List<int> oppositeLeg = new List<int>();
    bool currentLeg = true;
    float resetTimer = 0.5f;

    [Space(10)]
    [Header("GameObject Assignment")]
    [Space(10)]

    public GameObject spider;
    public GameObject[] legTargets;
    public GameObject[] legCubes;

    [Space(10)]
    [Header("Rotation of Body and Movment of leg")]
    [Space(10)]

    public bool enableBodyRotation = false;
    public bool enableMovementRotation = false;
    public bool rigidBodyController;

    [Space(10)]
    [Header("Values for leg Movement")]
    [Space(10)]

    public float moveDistance = 0.7f;
    public float stepHeight = .15f;
    public float spiderJitterCutOff = 0f;
    public int waitTimeBetweenEveryStep = 0;
    public float LegSmoothness = 8;
    public float BodySmoothness = 8;
    public float OverStepMultiplier = 4;





    void Start()
    {
        lastBodyUp = transform.up;

        legPositions = new Vector3[legTargets.Length];
        legOriginalPositions = new Vector3[legTargets.Length];

        for (int i = 0; i < legTargets.Length; i++)
        {
            legPositions[i] = legTargets[i].transform.position;
            legOriginalPositions[i] = legTargets[i].transform.position;

            if (currentLeg) { oppositeLeg.Add(i + 1); currentLeg = false; }
            else if (!currentLeg) { oppositeLeg.Add(i - 1); currentLeg = true; }
        }

        lastSpiderPosition = spider.transform.position;

        rotateBody();
    }


    void FixedUpdate()
    {
        velocity = spider.transform.position - lastSpiderPosition;
        velocity = (velocity + BodySmoothness * lastVelocity) / (BodySmoothness + 1f);

        moveLegs();
        rotateBody();


        lastSpiderPosition = spider.transform.position;
        lastVelocity = velocity;
        
    }

    void moveLegs()
    {
        if (!enableMovementRotation) return;
        for (int i = 0; i < legTargets.Length; i++)
        {
            if (Vector3.Distance(legTargets[i].transform.position, legCubes[i].transform.position) >= moveDistance) // si jambe trop loin on la bouge
            {
                if (!nextIndexToMove.Contains(i) && !IndexMoving.Contains(i))//si la jambe n'est pas dans la liste a bouger et ne bouge pas deja, on l'ajoute à la liste a bouger
                {
                    nextIndexToMove.Add(i);
                }
            }
            else if (!IndexMoving.Contains(i)) // si la jambe n'est pas trop loin et n'est pas dans la liste a bouger, on la set a la position par defaut/original
            {
                legTargets[i].transform.position = legOriginalPositions[i];
            }

        }

        if (nextIndexToMove.Count == 0 || IndexMoving.Count != 0) return; //si on a rien a faire bouger ou que rien n'est en mouvement, on ne fait rien
        Vector3 targetPosition = 
            legCubes[nextIndexToMove[0]].transform.position 
            + Mathf.Clamp(velocity.magnitude * OverStepMultiplier, 0.0f, 1.5f) 
            * (legCubes[nextIndexToMove[0]].transform.position - legTargets[nextIndexToMove[0]].transform.position) 
            + velocity * OverStepMultiplier;
        StartCoroutine(step(nextIndexToMove[0], targetPosition, false));
    }

    IEnumerator step(int index, Vector3 moveTo, bool isOpposite)
    {
        if (!isOpposite) moveOppisteLeg(oppositeLeg[index]);

        if (nextIndexToMove.Contains(index)) nextIndexToMove.Remove(index);//on retire la jambe de la file a bouger

        if (!IndexMoving.Contains(index)) IndexMoving.Add(index); // on l'ajoute en mouvement

        Vector3 startPos = legOriginalPositions[index];

        for (int i = 1; i <= LegSmoothness; ++i)
        {
            legTargets[index].transform.position = Vector3.Lerp(startPos, moveTo + new Vector3(0, Mathf.Sin(i / (float)(LegSmoothness + spiderJitterCutOff) * Mathf.PI) * stepHeight, 0), (i / LegSmoothness + spiderJitterCutOff));
            yield return new WaitForFixedUpdate();
        }
        // for (int i = 1; i <= LegSmoothness; ++i)
        // {
        //     float t = i / (float)LegSmoothness;
        //     float height = curve.Evaluate(t) * stepHeight;
        //     Vector3 arcPoint = Vector3.Lerp(startPos, moveTo, t) + new Vector3(0, height, 0);
        //
        //     legTargets[index].transform.position = arcPoint;
        //     yield return new WaitForFixedUpdate();
        // }

        legOriginalPositions[index] = moveTo;


        for (int i = 1; i <= waitTimeBetweenEveryStep; ++i) yield return new WaitForFixedUpdate();//temps d'attente avant de retirer la jambe de la liste "en mouvement" et qu'elle recommence a bouger, evite des pas trop rapide 

        if (IndexMoving.Contains(index)) IndexMoving.Remove(index);// le mouvement est fini, on retire la jambe de la liste

    }

    void moveOppisteLeg(int index)
    {
        Vector3 targetPosition = legCubes[index].transform.position + Mathf.Clamp(velocity.magnitude * OverStepMultiplier, 0.0f, 1.5f) * (legCubes[index].transform.position - legTargets[index].transform.position) + velocity * OverStepMultiplier;
        StartCoroutine(step(index, targetPosition, true));
    }

    void rotateBody()
    {
        if (!enableBodyRotation) return;

        Vector3 v1 = legTargets[0].transform.position - legTargets[1].transform.position;
        Vector3 v2 = legTargets[2].transform.position - legTargets[3].transform.position;
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        Vector3 up = Vector3.Lerp(lastBodyUp, normal, 1f / (float)(BodySmoothness));
        transform.up = up;
        if (!rigidBodyController) transform.rotation = Quaternion.LookRotation(transform.parent.forward, up);
        lastBodyUp = transform.up;
    }

    private void OnDrawGizmos()
    {
        if (legTargets.Length == 0 ) return;
        
        Gizmos.color = Color.red;
        foreach (GameObject pos in legTargets)
        {
            Gizmos.DrawWireSphere(pos.transform.position, moveDistance);
        }
        
        Gizmos.color = Color.green;
        foreach (GameObject pos in legCubes)
        {
            Gizmos.DrawWireSphere(pos.transform.position, moveDistance);
        }

        if (legOriginalPositions == null) return;
        
        Gizmos.color = Color.blue;
        foreach (Vector3 pos in legOriginalPositions)
        {
            Gizmos.DrawSphere(pos, moveDistance/3);
        }
    }
}
