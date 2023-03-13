using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FallObj : BossSkill
{
    [Header("생성 관련")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 spawnRange;
    [SerializeField] private Vector2 holeRange;
    [SerializeField] private FallingObject moveObj;
    [SerializeField] private FallingObject unmoveObj;
    [SerializeField] private int count;
    [SerializeField] private float duration = 10f;

    [Header("큐브 이동 관련")]
    [SerializeField] private Transform inPoint;
    [SerializeField] private Transform outPoint;

    private List<FallingObject> list = new List<FallingObject>();
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
            FallingObject obj;
            if (!isReinforce)
                obj = Instantiate(Random.Range(0, 2) > 0 ? moveObj : unmoveObj, spawnPoint);
            else
                obj = Instantiate(unmoveObj, spawnPoint);
            list.Add(obj);
            Vector3 pos = 
                new Vector3 (
                (Random.Range(0, 2) > 0 ? 1 : -1) * Random.Range(holeRange.x, spawnRange.x),
                Random.Range(-spawnRange.y, spawnRange.y),
                (Random.Range(0, 2) > 0 ? 1 : -1) * Random.Range(holeRange.y, spawnRange.z)
                );
            Debug.Log(pos);
            obj.transform.position += pos;
            obj.SetPoint(inPoint.position, outPoint.position);
        }
        yield return new WaitForSeconds(duration);
        foreach (FallingObject item in list)
            item.Destroy();
        list.Clear();
        parent.CallNextSkill();
    }

    public override void PostDelay() {
        
    }

    public override void SkillEnd() {

    }

    public override void StopSkill() {
        StopAllCoroutines();
        foreach (FallingObject item in list)
            item.Destroy();
        list.Clear();
    }
}
