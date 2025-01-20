using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ComportementManager : MonoBehaviour
{
    public static ComportementManager Instance;
    
    [Header("Player")]
    public Collider playerBouncingCollider;
    public Collider playerSlidingCollider;
    
    [Header("Comportement colors")]
    public Color impulseColor;
    public Color bouncingColor;
    public Color rocketColor;
    public Color magnetColor;
    public Color immuableColor;
    public Color noComportementColor;
    
    [Header("Impulse")]
    public float repulserTime = 5f;
    public float doubleImpulseTime = 6f;
    public float repulserRange;
    public float repulserForce;
    public bool destroyOnUse = false;
    public bool impulseGradiantForce = false;
    public GameObject impulseFeedback;
    [Tooltip("Si Rigidbody sur lui")]
    public bool applyOnMe = false;
    
    [Header("Rocket")]
    public float rocketForce = 20f;
    public float rocketForceOnPlayer = 20f;
    public float rocketForceWhenGrab = 20f;
    public float rocketOnOffCouldown = 4f;
    public float rocketMaxSpeed = 20f;
    
    [Header("DoubleRocket")]
    public float rocketDoubleForce = 20f;
    public float rocketDoubleForceOnPlayer = 20f;
    public float rocketDoubleForceWhenGrab = 20f;
    public float rocketDoubleCouldown = 4f;
    
    [Header("Rocket Immuable")]
    public float rocketReleaseForce = 10f;
    public float chargeTimeMax = 6f;

    [Header("Magnet Rocket")]
    public GameObject prefabMagnetRocketForcefield;
    public float magnetRocketFlyTime = 4f;
    public float rocketMagnetForce = 20f;
    public float rocketMagnetForceOnPlayer = 20f;
    public float rocketMagnetForceWhenGrab = 20f;
    public float magnetTrailForce = 10f;
    public float magnetTrailLerp = 1f;
    public float magnetTrailTimeBeforeMove = 3f;
    
    [Header("Magnet")]
    public float magnetRange = 10f;
    public float magnetForce;
    [HideInInspector] public bool magnetGradiantForce;

    [Header("Bouncing")]
    public PhysicMaterial bouncyMaterial;
    [Header("DoubleBounce")]
    public PhysicMaterial doubleBouncyMaterial;

    [Header("Magnet Bounce")]
    public float magnetBounceForce;
    public float magnetBounceRange = 10f;
    [Tooltip("Ajoute x% de la velocité au moment de la collision et l'ajoute à magnet Force")]
    public float magnetForceVelocityMultiplier;
    [Range(1,5)]
    public float magnetScaleMultiplier;
    [Tooltip("delay pour scale magnet range collision quand grab, sinon trop court avec les rebond qui s'enchaine")]
    public float delayScale = .5f;
    
    [Header("Impulse Magnet")]
    public float zoneImpulseRange;
    public float zoneImpulseForce;
    public GameObject prefabImpulseMagnet;

    [Header("Impulse Bounce")]
    public float impulseBounceForce;
    public float impulseBounceRange;
    public float impulseBounceTimer;
    [Tooltip("Ajoute x% de la velocité au moment de la collision et l'ajoute à impulse bounce Force")]
    public float impulseForceMultiplier;
    
    [Header("Impulse Rocket")]
    public float impulseRocketExplosionForce;
    public float impulseRocketExplosionRange;
    public float impulseRocketFlyForce;
    public float impulseRocketFlyForceOnPlayer;
    public float impulseRocketFlyTime;
    public float timeBetweenImpulses;
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de ComportementManager dans la scène");
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void InitPlayerCollider(CapsuleCollider[] colliderplayer)
    {
        //init sinon se fait pas dans le bon ordre dans le start et créer erreur nullRef
        foreach (CapsuleCollider collider in colliderplayer)
        {
            if (collider.height == 2f)
            {
                playerBouncingCollider = collider;
            }

            if (collider.height == 1.9f)
            {
                playerSlidingCollider = collider;
            }
        }
    }
    
    public GameObject InstantiateFeedback(GameObject feedbackPrefab, Vector3 position, Quaternion rotation, Transform parent  = null)
    {
        return Instantiate(feedbackPrefab, position, rotation, parent);
    }

    public void DestroyObj(GameObject obj, float time = 0)
    {
        //Debug.LogWarning($"Destroy {obj.name}");
        Destroy(obj, time);
    }
}
