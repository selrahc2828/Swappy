using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuState : GameState
{
    public GameObject loadingMenu;

    public override void Enter()
    {
        Scene activeScene = fsm.selectedLevels[fsm.sceneID].LoadedScene;
        SceneManager.UnloadSceneAsync(activeScene);
        fsm.ChangeState(GetComponent<MainMenuState>());
    }
}
