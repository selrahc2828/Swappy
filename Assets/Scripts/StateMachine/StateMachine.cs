using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State currentState;

    private void Start()
    {
        Initialize();
        if(currentState != null )
        {
            currentState.Enter();
        }

    }

    public abstract void Initialize();

    private void Update()
    {
        if(currentState != null)
        {
            currentState.TickLogic();
        }
    }

    private void FixedUpdate()
    {
        if(currentState != null)
        {
            currentState.TickPhysics();
        }
    }

    public void ChangeState(State newState)
    {
        if (newState != null)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
