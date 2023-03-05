using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IceWall : BossSkill
{
    [Header("얼음 생성 관련")]
    [SerializeField] private List<Transform> spawnPoints_Horizontal;
    [SerializeField] private List<Transform> spawnPoints_Vertical;
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
        //애니메이션 제작 시 변경 (임시 코드)
        //Anim.SetTrigger(hash_tIceWall);
        bool isHorizontal = Random.Range(0, 2) > 0;
        StartCoroutine(SpawnWall(isHorizontal, false));
    }

    private IEnumerator SpawnWall(bool isHorizontal, bool isSecond) {
        List<Transform> points = isHorizontal ? spawnPoints_Horizontal : spawnPoints_Vertical;
        if (isReinforce && !isSecond)
            StartCoroutine(SpawnWall(!isHorizontal, true));

        int index = Random.Range(0, points.Count);
        Transform point = points[index];
        if (!isHorizontal) index += spawnPoints_Horizontal.Count;
        float range = spawnRange[index];

        for (int count = 0; count < spawnCount; count++) {
            IceWallMove obj = Instantiate(prefab, point);
            wallList.Add(obj);
            obj.transform.forward = point.forward;
            obj.transform.position += Vector3.Cross(point.forward, Vector3.up) * Random.Range(-(int)range, (int)range);
            obj.SpawnWall(wallSize, spawnTime, liveTime, moveSpeed);
            yield return new WaitForSeconds(spawnTerm);
        }

        if (isSecond) yield break;
        SkillEnd();
    }

    public override void PreDelay() {
        //do nothing
    }

    public override void HitTime() {
        bool isHorizontal = Random.Range(0, 2) > 0;
        StartCoroutine(SpawnWall(isHorizontal, false));
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
