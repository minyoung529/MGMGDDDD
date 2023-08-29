using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCatchState : BossState
{
    private Cupboard[] cupboards;


    public override BossStateName StateName => BossStateName.Catch;
    private float delayTime = 5f;

    private void Awake()
    {
        cupboards = FindObjectsOfType<Cupboard>();
    }
    public override void OnEnter()
    {
        if (boss.Target == null)
        {
            boss.ChangeState(BossStateName.Patrol);
            return;
        }

        foreach (var cupboard in cupboards)
        {
            if (cupboard.PlayerIn)
            {
                boss.ChangeState(BossStateName.Patrol);
                return;
            }
        }

        Pet pet = boss.Target.GetComponent<Pet>();

        // 잡은 게 펫이라면
        if (pet != null)
        {
            // 잡은 적 없고 플레이어가 소유한 펫이면~
            if (!boss.CatchingPet.IsContain(pet) && PetManager.Instance.Contain(pet))
            {
                boss.ChangeState(BossStateName.PetCatch);
            }
            else
            {
                // 이미 잡았다면 잡지 않는다
                boss.ChangeState(BossStateName.Patrol);
            }
        }
        else
        {
            // Game Over
            if (boss.Target.CompareTag("Player"))
            {
                EventManager.TriggerEvent(EventName.BossFail);
            }
            else
            {
                // TEMP
                boss.ChangeState(BossStateName.Patrol);
            }
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {

    }
}
