using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : GameState
{
    public GameObject pauseMenuPanel;

    public override void Enter()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToGame()
    {
        fsm.ChangeState(GetComponent<PlayingState>());
    }

    public void ReturnToMenu()
    {
        fsm.ChangeState(GetComponent<ReturnToMenuState>());
    }

    public override void Exit()
    {
        pauseMenuPanel.SetActive(false);
        //Mettre le jeu en pause = responsabilité de PauseState.
        Time.timeScale = 1f;
    }
}