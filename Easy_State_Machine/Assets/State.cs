using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected StateMachineScriptable _sm;

    public State(StateMachineScriptable myMachine)
    {
        _sm = myMachine;
    }

    public virtual void Awake() { }
    public virtual void Sleep() { }
    public virtual void Execute() { }
    public virtual void LateExecute() { }
}
