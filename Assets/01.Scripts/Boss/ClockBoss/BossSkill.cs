using UnityEngine;

[System.Serializable]
public abstract class BossSkill : MonoBehaviour
{
    protected BossScript parent = null;
    protected bool isReinforce = false;
    //��ų �ߵ� Ȯ�� ���
    public abstract float ChanceFactor { get; }
    //��ų ����
    public abstract void ExecuteSkill();
    //�� ������ ����
    public abstract void PreDelay();
    //�� ������ ����
    public abstract void HitTime();
    //�� ������ ����
    public abstract void PostDelay();
    //�� ������ ����
    public abstract void SkillEnd();
    //��ų ���� ����
    public abstract void StopSkill();

    public void SetParent(BossScript parent) {
        this.parent = parent;
    }

    public void Reinforce(bool value) {
        isReinforce = value;
    }
}
