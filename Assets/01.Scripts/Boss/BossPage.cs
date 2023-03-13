using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPage 
{
    private BossScript parent = null;

    public float TimeToNextSkill = 3f;
    public float TTNRandomRange = 1f;
    //�� �������� �����ϱ� ���� ü�� ����
    [Range(0, 1)]
    public float ConditionHp = 1f;

    private float totalChance;

    private int skillIndex = 0;
    [SerializeField] private List<BossSkill> skillList;
    public BossSkill CurSkill => skillList[skillIndex];

    /// <summary>
    /// ���õ� ��ų�� ����մϴ�
    /// </summary>
    public void Execute() {
        CurSkill.ExecuteSkill();
    }

    /// <summary>
    /// �������� ������ ����� ��ų ����
    /// </summary>
    public void SelectSkill() {
        float rand = Random.Range(0, totalChance);
        int index = 0;
        for (float sum = skillList[0].ChanceFactor; sum < rand; sum += skillList[++index].ChanceFactor);
        //���� 3���� ��ų Ȯ���� 1 : 1 : 1 �̶�� �� 3�� ���� �� 0 ~ 3 ������ ������ ���� ���ϰ� 1���϶�� ù ��° ��ų�� 2���϶�� �� ��° ��ų�� ����Ѵ�.
        skillIndex = index;
    }

    /// <summary>
    /// �ε����� �ش��ϴ� ��ų ����
    /// </summary>
    public void SelectSkill(int index) {
        skillIndex = index;
    }

    public void SetUp(BossScript parent) {
        if (this.parent) {
            Debug.LogError("�� ��ų �� �� �̻��� �θ�� ������ �� �����ϴ�");
            return;
        }
        this.parent = parent;
        for(int i = 0; i < skillList.Count; i++) {
            skillList[i].SetParent(parent);
            totalChance += skillList[i].ChanceFactor;
        }
    }

    public void Reinforce(bool value) {
        foreach (BossSkill item in skillList)
            item.Reinforce(value);
    }
}
