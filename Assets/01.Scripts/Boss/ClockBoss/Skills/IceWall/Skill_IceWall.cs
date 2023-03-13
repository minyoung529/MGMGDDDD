using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IceWall : BossSkill
{
    [Header("얼음 스포너 관련")]
    [SerializeField] private List<IceWallSpawner> spawner_Horizontal;
    [SerializeField] private List<IceWallSpawner> spawner_Vertical;
    [SerializeField] private SpawnData spawnData;
    [SerializeField] public float wallSpeed;

    private List<IceWallSpawner> executingSpawner = new List<IceWallSpawner>();

    [Header("스킬 관련")]
    [SerializeField] private float chanceFactor;
    public override float ChanceFactor => chanceFactor;
    private int hash_tIceWall = Animator.StringToHash("tIceWall");
    private int hash_tIceWall_2 = Animator.StringToHash("tIceWall_2");

    public override void ExecuteSkill() {
        parent.Anim.SetTrigger(hash_tIceWall);
    }

    public override void PreDelay() {
        //do nothing
    }

    public override void HitTime() {
        bool isHorizontal = Random.Range(0, 2) > 0;
        for (int i = 0; i < (isReinforce ? 2 : 1); i++) {
            if (isHorizontal)
                executingSpawner.Add(spawner_Horizontal[Random.Range(0, spawner_Horizontal.Count)]);
            else
                executingSpawner.Add(spawner_Vertical[Random.Range(0, spawner_Vertical.Count)]);
            isHorizontal = !isHorizontal;
        }
        for (int i = 0; i < executingSpawner.Count - 1; i++)
            executingSpawner[i].StartSpawn(spawnData, wallSpeed);
        executingSpawner[executingSpawner.Count - 1].StartSpawn(spawnData, wallSpeed, () => parent.Anim.SetTrigger(hash_tIceWall_2));
        executingSpawner.Clear();
    }

    public override void PostDelay() {
        //do nothing
    }

    public override void SkillEnd() {
        parent.CallNextSkill();
    }

    public override void StopSkill() {
        foreach (IceWallSpawner item in executingSpawner)
            item.StopSpawn();
        executingSpawner.Clear();
    }
}
