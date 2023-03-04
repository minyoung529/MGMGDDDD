using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IceWall : BossSkill
{
    [Header("얼음 생성 관련")]
    [SerializeField] private List<Transform> spawnPoint;
    [SerializeField] private List<float> spawnRange;
    //스폰 주기
    [SerializeField] private float spawnTerm;
    //얼음 생성 소요 시간
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnCount;
    [SerializeField] private float liveTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 wallSize;
    [SerializeField] private IceWallMove prefab;
    private List<IceWallMove> wallList = new List<IceWallMove>();

    [Header("스킬 관련")]
    [SerializeField] private float chanceFactor;
    public override float ChanceFactor => chanceFactor;
    private int hash_tIceWall = Animator.StringToHash("tIceWall");

    public override void ExecuteSkill() {
        //Anim.SetTrigger(hash_tIceWall);
        StartCoroutine(SpawnWall());
    }

    private IEnumerator SpawnWall() {
        int count = 0;
        int index = Random.Range(0, spawnPoint.Count);
        Transform point = spawnPoint[index];
        float range = spawnRange[index];
        while(count < spawnCount) {
            IceWallMove obj = Instantiate(prefab, point);
            wallList.Add(obj);
            obj.transform.forward = point.forward;
            obj.transform.position += Vector3.Cross(point.forward, Vector3.up) * Random.Range(-(int)range, (int)range);
            obj.SpawnWall(wallSize, spawnTime, liveTime, moveSpeed);
            count++;
            yield return new WaitForSeconds(spawnTerm);
        }

        SkillEnd();
    }

    public override void PreDelay() {
        //do nothing
    }

    public override void HitTime() {
        StartCoroutine(SpawnWall());
    }

    public override void PostDelay() {
        //do nothing
    }

    public override void SkillEnd() {
        wallList.Clear();
        parent.CallNextSkill();
    }

    public override void StopSkill() {
        foreach (IceWallMove item in wallList)
            item.Destroy();
        wallList.Clear();
    }
}
