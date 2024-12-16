using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Controls controls;
    
    [Header("Rendu Lotha")]
    public string scene1;
    public string scene2;
    public KeyCode keyForScene1 = KeyCode.Alpha1;
    public KeyCode keyForScene2 = KeyCode.Alpha2;
    public KeyCode keyForScene3 = KeyCode.Alpha3;
    
    [HideInInspector]public CameraController camControllerScript;
    [HideInInspector]public PlayerCam playerCamScript;
    [HideInInspector]public MoveCamera moveCamScript;
    [HideInInspector]public GrabObject grabScript;

    [Header("SlowTimer")]
    [Range(0f, 1f)]
    public float slowCoeff = 0.3f;
    [HideInInspector]
    public bool slowMotion;
    public bool slowTimerActive = true;
    [HideInInspector]
    public float slowTimer;
    public float slowTimeDuration = 5f;
    public TextMeshProUGUI timerSlowText;
    
    //à supprimer, juste caché tant qu'on a interactColor
    [Header("Couleurs d'interaction")]
    [HideInInspector]public Material defaultColor;
    [HideInInspector]public Material interactAVolerMat;
    [HideInInspector]public Material interactRienAVolerMat;
    [HideInInspector]public Material interactNOTPossibleMat;
    //à supprimer, juste caché tant qu'on a interactColor

    [Header("Comportement colors")]
    [HideInInspector]public Color Repulsive_color;
    [HideInInspector]public Color Rebond_color;
    [HideInInspector]public Color fusee_color;
    [HideInInspector]public Color aimant_color;
    [HideInInspector]public Color immuable_color;
    [HideInInspector]public Color Uncomportemented_color;
    
    [Header("Player")]
    public GameObject[] players; //dans Steler proto et ComportementStateMachine(pas sûr qu'il soit utilisé dedans)
    public GameObject player;
    public Camera mainCamera;
    
    [Header("Player Movement Parameters")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public Transform orientation;

    [Header("Player Jumping Parameters")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Player Crouching Parameters")]
    public float crouchSpeed;
    public float crouchYScale;

    [Header("Player Ground Check Parameters")]
    public float playerHeight;
    public LayerMask whatIsGround;

    [Header("Player Slope Handeling Parameter")]
    public float maxSlopeAngle;
    
    [Header("Player Projecting Parameter")]
    public LayerMask hitLayer;
    public float projectionTimeDuration;
    [HideInInspector]
    public float projectionTimer;
    public float coeffRefill = 0.75f;
    public float projectionRange;
    public float coeffReducDistance;
    public bool activeGizmoRange;
    
    [HideInInspector] public bool etatIsProjected;
    public TextMeshProUGUI timerProjectionText;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la scène");
            return;
        }
        Instance = this;
        
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogWarning("Il n'y a pas de MainCamera dans la scène");
        }
        
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerGO in players)
        {
            if (playerGO.GetComponent<Rigidbody>())
            {
                player = playerGO;
                ComportementManager.Instance.InitPlayerCollider(player.GetComponentsInChildren<CapsuleCollider>());
                return;
            }
        }

        if (player == null)
        {
            Debug.LogWarning("GameManager Player non renseigné");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slowMotion = false;
        slowTimer = 0f;
        etatIsProjected = false;
        projectionTimer = projectionTimeDuration;

        timerProjectionText = GameObject.FindWithTag("TextTimerProjecting").GetComponent<TextMeshProUGUI>();
        if (!timerProjectionText)
        {
            Debug.Log("Il n'y a pas de text Timer Projection");
        }
        
        timerSlowText = GameObject.FindWithTag("TextTimerSlow").GetComponent<TextMeshProUGUI>();
        if (!timerSlowText)
        {
            Debug.Log("Il n'y a pas de text Timer Slow");
        }
        
        //à virer quand player greg supprimé
        camControllerScript = FindObjectOfType<CameraController>();
        if (camControllerScript == null)
        {
            //Debug.LogWarning("Il n'y a pas de CameraController dans la scène");
        }
        //

        playerCamScript = FindObjectOfType<PlayerCam>();
        if (playerCamScript == null)
        {
            Debug.LogWarning("Il n'y a pas de PlayerCam dans la scène");
        }
        
        moveCamScript = FindObjectOfType<MoveCamera>();
        if (moveCamScript == null)
        {
            Debug.LogWarning("Il n'y a pas de MoveCamera dans la scène");
        }
        
        orientation = GameObject.FindGameObjectWithTag("Orientation").transform;
        
        grabScript = FindObjectOfType<GrabObject>();
        if (grabScript == null)
        {
            Debug.LogWarning("Il n'y a pas de grabScript dans la scène");
        }

        
        
        controls.Player.Enable();
        controls.Player.ReloadScene.performed += ReloadScene;
        controls.Player.StopTime.performed += SlowMotionInput;
    }

    private void OnDisable()
    {
        controls.Player.ReloadScene.performed -= ReloadScene;
        controls.Player.StopTime.performed -= SlowMotionInput;
    }

    // Update is called once per frame
    void Update()
    {
        SlowTime();
        ProjectionTimer();

        // Vérifier si la touche pour la scène 1 est pressée.
        if (Input.GetKeyDown(keyForScene1))
        {
            ChangeScene(scene1);
        }
        // Vérifier si la touche pour la scène 2 est pressée.
        if (Input.GetKeyDown(keyForScene2))
        {
            ChangeScene(scene2);
        }

    }

    private void ReloadScene(InputAction.CallbackContext context)
    {
        SlowMotion(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ChangeScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Le nom de la scène n'est pas défini !");
        }
    }

    public void SlowMotion(bool etat, float slow = 0f) //float slow = 0f => si pas renseigné, par defaut = 0
    {
        Debug.Log(Time.fixedDeltaTime);
        if (etat)
        {
            Time.timeScale = slow;
            // unity utilise fixedDeltaTime (intervalle fixe) pour calculer la physique qui n'est pas affecté par le timescale ce qui produit donne un aspect saccadé car les 2 ne sont plus synchro
            // on synchronise dont le fixedDeltaTime par rapport au timescale
            Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
        }
        else
        {
            // valeurs par défaut de Unity
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    public void SlowTime()
    {
        if (slowTimerActive && slowMotion)
        {
            slowTimer += Time.unscaledDeltaTime; // unscaledDeltaTime = Utilise le temps réel

            if (slowTimer > slowTimeDuration)
            {
                // Reset timeIsStop et le timer
                slowMotion = false;
                slowTimer = 0f;
                SlowMotion(false);
            }
        }
        else
        {
            slowTimer = 0;
        }

        if (timerSlowText != null)
        {
            timerSlowText.text = (slowTimeDuration - slowTimer).ToString("F2");
        }
    }
    private void SlowMotionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // if bool == true set false et vice versa
            slowMotion = !slowMotion;
            SlowMotion(slowMotion, slowCoeff);
        }
    }
    
    void ProjectionTimer()
    {
        if (etatIsProjected)
        {
            projectionTimer -= Time.deltaTime;
        }
        else if (projectionTimer < projectionTimeDuration)
        {
            projectionTimer += Time.deltaTime * coeffRefill;
        }
        
        projectionTimer = Mathf.Clamp(projectionTimer, 0, projectionTimeDuration);
        if (timerProjectionText)
        {
            timerProjectionText.text = projectionTimer.ToString("F2");// F2 arrondi à 2 décimal
        }
    }
}
