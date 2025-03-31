using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eflatun.SceneReference;

public class GameManagerFSM : MonoBehaviour
{
    
    public List<SceneReference> selectedLevels;
    public int sceneID;
    
    private GameState currentState;
    
    private void Start()
    {
        GameState[] states = GetComponents<GameState>();
        foreach (GameState state in states)
        {
            state.Initialize(this);
        }
        
        //Make persistent
        DontDestroyOnLoad(gameObject);
       
        currentState = GetComponent<MainMenuState>();
        currentState.Enter();
    }
    
    private void Update()
    {
        currentState?.Tick();
    }

    public void ChangeState(GameState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
