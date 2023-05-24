using System.Collections.Generic;
using UnityEngine;

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
        if (states[index].fence > 0)
        {
            Debug.Log((PetStateName)index);
            return;
        }

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

    public void BlockState(int index)
    {
        states[index].Block();
    }
    public void UnBlockState(int index)
    {
        states[index].UnBlock();
    }
    public void AllUnBlock()
    {
        foreach(IState item in states)
        {
            item.fence=0;
        }
    }
}