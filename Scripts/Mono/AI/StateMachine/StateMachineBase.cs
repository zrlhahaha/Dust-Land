using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBase  {

    StateMachine stateMachine;

    public StateMachineBase(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void OnStateEnter() { }

    public virtual void OnStateUpdate() { }

    public virtual void OnStateExit() { }
}
