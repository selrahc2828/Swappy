using UnityEngine;

public class PlayerCondition : Condition
{
    private GameManager gameManager;
    private GameObject player;

    private void Start()
    {
        gameManager = GameManager.Instance;
        player = gameManager.player;
    }
}
