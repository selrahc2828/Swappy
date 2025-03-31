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
    
    //Used by button - single responsibility
    public void TransitionToGame()
    {
        fsm.ChangeState(GetComponent<LoadingLevelState>());
    }

    public override void Exit()
    {
        menuGO.SetActive(false);
    }
}