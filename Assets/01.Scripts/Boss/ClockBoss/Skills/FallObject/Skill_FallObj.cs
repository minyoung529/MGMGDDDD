using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FallObj : BossSkill 
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 spawnRange;
    [SerializeField] private Vector3 holeRange;
    [SerializeField] private int count;
    [SerializeField] private FallingObject moveObj;
    [SerializeField] private FallingObject unmoveObj;

    private List<FallingObject> list;
    private int hash_tFallObj = Animator.StringToHash("tFallObj");

    public override float ChanceFactor => 0;

    public override void ExecuteSkill() {
        parent.Anim.SetTrigger(hash_tFallObj);
    }

    public override void PreDelay() {

    }

    public override void HitTime() {
        StartCoroutine(SpawnObj());
    }

    private IEnumerator SpawnObj()
    {
        for (int i = 0; i < count; i++)
        {
            FallingObject obj = Instantiate(Random.Range(0, 2) > 0 ? moveObj : unmoveObj, spawnPoint);
            list.Add(obj);
            transform.position += Vector3.Lerp(-spawnRange, spawnRange, Random.Range(0, 1));
        }
        yield return new WaitForSeconds(10f);
        SkillEnd();
    }

    public override void PostDelay() {
        
    }

    public override void SkillEnd() {
        foreach (FallingObject item in list)
            item.Destroy();
    }

    public override void StopSkill() {
        
    }
}
