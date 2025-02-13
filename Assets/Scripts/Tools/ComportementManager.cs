using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public partial class ComportementManager : MonoBehaviour
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
    
    [Header("Comportement Feedbacks")]
    public FlareData flareData;
    
    [Header("A instancier pour chaque objet comportement")]
    public GameObject feedBack_Impulse;
    public GameObject feedBack_Bouncing;
    public GameObject feedBack_Rocket;
    public GameObject feedBack_Magnet;
    public GameObject feedBack_Immuable;

    [Header("PrefabComportementGeneric")]
    public GameObject magnetGenericPrefab;
    
    [Header("DATAS")]
    public ImpulseData impulseData;
    public RocketData rocketData;
    public DoubleRocketData doubleRocketData;
    public ImmuableRocketData immuableRocketData;
    public MagnetRocketData magnetRocketData;
    public MagnetData magnetData;
    public DoubleMagnetData doubleMagnetData;
    public BounceData bounceData;
    public DoubleBounceData doubleBounceData;
    public MagnetBounceData magnetBounceData;
    public ImpulseMagnetData impulseMagnetData;
    public ImpulseBounceData impulseBounceData;
    public ImpulseRocketData impulseRocketData;
    
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
