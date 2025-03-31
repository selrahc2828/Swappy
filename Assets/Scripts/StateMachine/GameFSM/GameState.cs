using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    protected GameManagerFSM fsm;
    
    //Called on FSM Start
    public virtual void Initialize(GameManagerFSM fsm)
    {
        this.fsm = fsm;
    }

    public virtual void Enter(){}
    public virtual void Tick(){}
    public virtual void Exit(){}
}
