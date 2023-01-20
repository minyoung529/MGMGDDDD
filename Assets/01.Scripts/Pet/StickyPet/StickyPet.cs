using DG.Tweening;
using UnityEngine;

public class StickyPet : Pet
{
    private float moveSpeed = 1f;

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        rigid.useGravity = true;
        rigid.isKinematic = false;
    }
    #endregion

    #region Skill
    // Active Skill
    protected override void ClickActive()
    {
        base.ClickActive();

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            StickySkill(hit);

            IsSkilling = false;
        }
    }
    private void StickySkill(RaycastHit hit)
    {
        IsStop = true;
        agent.enabled = false;
        rigid.useGravity= false;
        rigid.isKinematic = true;

        transform.DOKill();
        transform.DOMove(hit.point, moveSpeed);
    }

    // Passive Skill
    protected override void PassiveSkill()
    {
        base.PassiveSkill();


    }
    #endregion
}
