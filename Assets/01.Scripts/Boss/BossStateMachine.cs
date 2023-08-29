using System.Collections.Generic;

public class BossStateMachine : StateMachine<Boss>
{
    public BossStateMachine(Boss parent, List<IState> states, int startIndex = 0) : base(parent, states, startIndex) { }
    public BossStateMachine(Boss parent, IState[] states, int startIndex = 0) : base(parent, states, startIndex) { }

    #region Event
    public void ChangeState(EventParam param)
    {
        ChangeState((int)param["Index"]);
    }
    #endregion
}