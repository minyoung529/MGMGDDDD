using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSkill
{
    //선 딜레이 시작
    public abstract void PreDelay();

    //선 딜레이 종료
    public abstract void HitTime();

    //후 딜레이 시작
    public abstract void PostDelay();

    //후 딜레이 종료
    public abstract void Stay();

    //공격을 강제로 종료 시킬 때
    public abstract void StopAttack();
}
