using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine{

    public StateMachineBase state;

    public void Update()
    {
        state.OnStateUpdate();
    }

    public void ChangeState(StateMachineBase state)
    {
        if (state != null)
            state.OnStateExit();

        state.OnStateEnter();
    }
}
