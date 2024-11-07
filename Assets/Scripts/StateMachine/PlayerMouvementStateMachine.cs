using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvementStateMachine : StateMachine
{
    public static Walking walkingState;
    public static Sprinting sprintingState;
    public static Crouching crouchingState;
    public static Jumping jumpingState;
    public static Projecting projectingState;

    public Rigidbody rb;
    public override void Initialize()
    {
        DontDestroyOnLoad(this);
        walkingState = new Walking(this);
        sprintingState = new Sprinting(this);
        crouchingState = new Crouching(this);
        jumpingState = new Jumping(this);
        projectingState = new Projecting(this);
        currentState = walkingState;
        rb = GetComponent<Rigidbody>();
        currentState.Enter();
    }
}
