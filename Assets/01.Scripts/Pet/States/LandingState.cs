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
    /// �þ� ������ �����ϴ� Ȱ��ȭ ���� ���� ��ư�� ã�Ƴ� �� Ÿ������ ����
    /// </summary>
    /// <returns>Ž�� ���� ����</returns>
    public bool FindButton() {
        ButtonObject target = GameManager.Instance.GetNearest(transform, GameManager.Instance.Buttons, 5f);
        if (!target) return false;
        Vector3 dest = target.transform.position;
        pet.SetDestination(dest);
        return true;
    }
}
