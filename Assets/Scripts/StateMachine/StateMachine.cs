using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void OnDisable()
    {
        currentState.Exit();
    }

    public void ChangeState(State newState)
    {
        if (newState != null)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        currentState?.CollisionStart(other);
    }
    public void OnCollisionStay(Collision other)
    {
        currentState?.CollisionDuring(other);
    }
    public void OnCollisionExit(Collision other)
    {
        currentState?.CollisionEnd(other);
    }

    void OnDrawGizmos()
    {
        currentState?.DisplayGizmos();
    }
}
