using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Controls controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            Debug.LogWarning("------ set controls ------");
            controls = new Controls(); // pour rebind, sinon pas même instance
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
        controls.PlayerGod.Enable();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
