using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializationState : GameState
{
    private GameObject startPosition;
    public GameObject player;
    public override void Enter()
    {
        InitPlayer();
        fsm.ChangeState(GetComponent<PlayingState>());
    }

    private void InitPlayer()
    {
        startPosition = GameObject.FindGameObjectWithTag("PlayerSpawn");
        Instantiate(player, startPosition.transform.position, startPosition.transform.rotation);
    }
}
