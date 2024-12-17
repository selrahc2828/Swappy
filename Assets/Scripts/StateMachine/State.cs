using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void TickLogic();
    public abstract void TickPhysics();
    public abstract void Exit();
    public abstract void CollisionStart(Collision other);
    public abstract void CollisionDuring(Collision other);
    public abstract void CollisionEnd(Collision other);
    public abstract void DisplayGizmos();
}
