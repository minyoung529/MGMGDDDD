using UnityEngine;

[System.Serializable]
public abstract class BossSkill : MonoBehaviour
{
    protected BossScript parent = null;
    //스킬 발동 확률 배수
    public abstract float ChanceFactor { get; }
    //스킬 실행
    public abstract void ExecuteSkill();
    //선 딜레이 시작
    public abstract void PreDelay();
    //선 딜레이 종료
    public abstract void HitTime();
    //후 딜레이 시작
    public abstract void PostDelay();
    //후 딜레이 종료
    public abstract void SkillEnd();
    //스킬 강제 종료
    public abstract void StopSkill();

    public void SetParent(BossScript parent) {
        this.parent = parent;
    }
}
