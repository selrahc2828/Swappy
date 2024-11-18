using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Controls controls;
    //public CameraController camControllerScript;
    //public GrabObject grabScript;

    [HideInInspector]
    public bool slowMotion, slowTimerActive;
    [HideInInspector]
    public float slowTimeDuration, slowTimer;

    [Header("Couleurs d'interaction")]
    public Material defaultColor;
    public Material interactAVolerMat;
    public Material interactRienAVolerMat;
    public Material interactNOTPossibleMat;

    [Header("Comportement colors")] 
    public Color Repulsive_color;
    public Color Rebond_color;
    public Color fusee_color;
    public Color aimant_color;
    public Color immuable_color;
    public Color Uncomportemented_color;

    public string scene1;
    public string scene2;
    public string scene3;

    public KeyCode keyForScene1 = KeyCode.Alpha1;
    public KeyCode keyForScene2 = KeyCode.Alpha2;
    public KeyCode keyForScene3 = KeyCode.Alpha3;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        slowMotion = false;
        slowTimer = 0f;

        //camControllerScript = FindObjectOfType<CameraController>();
        //grabScript = FindObjectOfType<GrabObject>();

        controls.Player.Enable();

        controls.Player.ReloadScene.performed += ReloadScene;

    }

    private void OnDisable()
    {
        controls.Player.ReloadScene.performed -= ReloadScene;
    }

    // Update is called once per frame
    void Update()
    {

        SlowTime();


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

        if (Input.GetKeyDown(keyForScene3))
        {
            ChangeScene(scene3);
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
    }
}
