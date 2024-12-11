using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ComportementManager : MonoBehaviour
{
    public static ComportementManager Instance;
    public GameManager gm;
    
    [Header("Comportement colors")]
    public Color impulseColor;
    public Color bouncingColor;
    public Color rocketColor;
    public Color magnetColor;
    public Color immuableColor;
    public Color noComportementColor;
    
    [Header("Impulse")]
    public float repulserTime = 5f;
    [HideInInspector]
    public float repulserTimer;
    public float repulserRange;
    public float repulserForce;
    public bool destroyOnUse = false;
    public bool impulseGradiantForce = false;
    public GameObject feedback;
    [Tooltip("Si Rigidbody sur lui")]
    public bool applyOnMe = false;
    
    [Header("Rocket")]
    public float rocketForce = 20f;
    
    [Header("Magnet")]
    public float magnetRange = 10f;
    public float magnetForce;
    public bool magnetGradiantForce;

    [Header("Bouncing")]
    public Collider playerBouncingCollider;
    public Collider playerSlidingCollider;
    public PhysicMaterial bouncyMaterial;
    [Header("DoubleBounce")]
    public PhysicMaterial doubleBouncyMaterial;

    [Header("Magnet Bounce")]
    public float magnetForceMultiplier;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de ComportementManager dans la scène");
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        repulserTimer = 0;
        var colliders = GameManager.Instance.player.GetComponentsInChildren<CapsuleCollider>();
        foreach (var collider in colliders)
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject InstantiateFeedback(GameObject feedbackPrefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(feedbackPrefab, position, rotation);
    }

    public void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }
}
