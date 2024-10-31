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
        // Vérifier si la touche pour la scène 3 est pressée.
        if (Input.GetKeyDown(keyForScene3))
        {
            ChangeScene(scene3);
        }
    }

    private void ReloadScene(InputAction.CallbackContext context)
    {
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

    public void StopTime(bool etat, float slow = 0f) //float slow = 0f => si pas renseigné, par defaut = 0
    {
        if (etat)
        {
            Time.timeScale = slow;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void SlowTime()
    {
        if (slowTimerActive && slowMotion)
        {
            slowTimer += Time.unscaledDeltaTime; // Utilise le temps réel

            if (slowTimer > slowTimeDuration)
            {
                // Reset timeIsStop et kle timer
                slowMotion = false;
                slowTimer = 0f;
                StopTime(false);
            }
        }
        else
        {
            slowTimer = 0;
        }
    }
}
