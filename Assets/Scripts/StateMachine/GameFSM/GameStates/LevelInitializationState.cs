using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializationState : GameState
{
    public override void Enter()
    {
        InitPlayer();
        fsm.ChangeState(GetComponent<PlayingState>());
    }
    
    private void InitPlayer()
    {
        //Todo : spawn player
    }
}
