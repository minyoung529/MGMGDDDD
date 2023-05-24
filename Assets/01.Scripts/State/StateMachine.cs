using System.Collections.Generic;
using UnityEngine;

//바꾸지마세요
public class StateMachine<T> {
    private T parent = default;
    public T Parent => parent;

    private List<IState> states = new List<IState>();
    private IState curState = null;

    private int curStateIndex;
    public int CurStateIndex => curStateIndex;

    public StateMachine(T parent, List<IState> states, int startIndex = 0) {
        this.parent = parent;
        this.states = states;
        ChangeState(startIndex);
    }

    public StateMachine(T parent, IState[] states, int startIndex = 0) {
        this.parent = parent;
        foreach (IState item in states) {
            this.states.Add(item);
        }
        ChangeState(startIndex);
    }

    public void ChangeState(int index) {
        curState?.OnExit();
        curStateIndex = index;
        curState = states[index];
        curState.OnEnter();
    }

    public void ChangeState(IState state) {
        for (int i = 0; i < states.Count; i++) {
            if(states[i].GetType() == state.GetType()) {
                curStateIndex = i;
                ChangeState(i);
                return;
            };
        }
        curStateIndex = states.Count;
        states.Add(state);
    }

    public void OnUpdate() {
        curState.OnUpdate();
    }

    public void OnDisable() {
        curState.OnExit();
    }
}