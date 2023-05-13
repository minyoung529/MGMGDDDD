using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetStateMachine : StateMachine<Pet>
{
    public PetStateMachine(Pet parent, List<IState> states, int startIndex = 0) : base(parent, states, startIndex) { }
    public PetStateMachine(Pet parent, IState[] states, int startIndex = 0) : base(parent, states, startIndex) { }

    #region Event
    public void ChangeState(EventParam param) {
        ChangeState((int)param["Index"]);
    }
    #endregion
}
