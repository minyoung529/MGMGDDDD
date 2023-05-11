using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : PetState
{
    public override PetStateName StateName => PetStateName.Landing;

    public override void OnEnter() {
        FindButton();
        pet.State.ChangeState((int)PetStateName.Idle);
    }

    public override void OnExit() {
        pet.SetNavEnabled(true);
    }

    public override void OnUpdate() {

    }

    /// <summary>
    /// 시야 범위에 존재하는 활성화 되지 않은 버튼을 찾아낸 후 타겟으로 설정
    /// </summary>
    /// <returns>탐색 성공 여부</returns>
    public bool FindButton() {
        ButtonObject target = GameManager.Instance.GetNearest(transform, GameManager.Instance.Buttons, 5f);
        if (!target) return false;
        Vector3 dest = target.transform.position;
        pet.SetDestination(dest);
        return true;
    }
}
