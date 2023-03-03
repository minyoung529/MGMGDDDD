using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPage 
{
    public float TimeToNextSkill = 3f;
    public float TTNRandomRange = 1f;
    //이 페이지로 진입하기 위한 체력 조건
    [Range(0, 1)]
    public float ConditionHp = 1f;

    private float totalChance;

    private int skillIndex = 0;
    public List<BossSkill> skillList;
    public BossSkill CurSkill => skillList[skillIndex];

    public void Execute() {
        CurSkill.ExecuteSkill();
    }

    /// <summary>
    /// 랜덤으로 보스가 사용할 스킬 선택
    /// </summary>
    public void SelectSkill() {
        float rand = Random.Range(0, totalChance);
        int index = 0;
        for (float sum = skillList[0].ChanceFactor; sum < rand; sum += skillList[++index].ChanceFactor);
        //만약 3개의 스킬 확률이 1 : 1 : 1 이라면 합 3을 구한 뒤 0 ~ 3 까지의 랜덤한 수를 구하고 1이하라면 첫 번째 스킬을 2이하라면 두 번째 스킬을 사용한다.
        skillIndex = index;
    }

    /// <summary>
    /// 인덱스에 해당하는 스킬 선택
    /// </summary>
    public void SelectSkill(int index) {
        skillIndex = index;
    }
}
