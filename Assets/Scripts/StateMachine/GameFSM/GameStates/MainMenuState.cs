using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : GameState
{
    public GameObject menuGO;
    
    public override void Enter()
    {
        menuGO.SetActive(true);
    }
    
    public void TransitionToGame(int levelIndex)
    {
        fsm.sceneID = levelIndex;
        fsm.ChangeState(GetComponent<LoadingLevelState>());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public override void Exit()
    {
        menuGO.SetActive(false);
    }
}