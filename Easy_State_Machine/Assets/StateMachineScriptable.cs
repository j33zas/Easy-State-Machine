using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineScriptable : ScriptableObject
{
    public State _currentState;
    public List<State> _states = new List<State>();
    //public List<Node> nodes = new List<Node>();

    public void Update()
    {
        if (_currentState != null)
            _currentState.Execute();
    }
    public void LateUpdate()
    {
        if (_currentState != null)
            _currentState.LateExecute();
    }
    public void AddState(State newState)
    {
        _states.Add(newState);
        if (_currentState == null)
            _currentState = newState;
    }
    public void SetState<T>() where T : State
    {
        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i].GetType() == typeof(T))
            {
                _currentState.Sleep();
                _currentState = _states[i];
                _currentState.Awake();
            }
        }
    }
    public bool ActualState<T>() where T : State
    {
        return _currentState.GetType() == typeof(T);
    }
    private int FindState(State isState)
    {
        int ammount = _states.Count;
        for (int i = 0; i < ammount; i++)
            if (_states[i] == isState)
                return i;

        return -1;
    }

}
