using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSkill
{
    //�� ������ ����
    public abstract void PreDelay();

    //�� ������ ����
    public abstract void HitTime();

    //�� ������ ����
    public abstract void PostDelay();

    //�� ������ ����
    public abstract void Stay();

    //������ ������ ���� ��ų ��
    public abstract void StopAttack();
}
