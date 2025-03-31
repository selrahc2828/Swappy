using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvementStateMachine : StateMachine
{
    public static Walking walkingState;
    public static Sprinting sprintingState;
    public static Crouching crouchingState;
    public static Jumping jumpingState;
    public static Falling fallingState;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public SoundManager soundManager;
    public override void Initialize()
    {
        //DontDestroyOnLoad(this);
        walkingState = new Walking(this);
        sprintingState = new Sprinting(this);
        crouchingState = new Crouching(this);
        jumpingState = new Jumping(this);
        fallingState = new Falling(this);
        currentState = walkingState;
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
    }
    
    void OnDrawGizmos()
    {
    }
}
