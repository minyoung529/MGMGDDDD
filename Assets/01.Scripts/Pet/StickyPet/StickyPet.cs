using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class StickyPet : Pet
{
    private float moveSpeed = 1f;

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        rigid.useGravity = true;
        rigid.isKinematic = false;
        agent.enabled= false;
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
    protected override void PassiveSkill(Collision collision)
    {
        base.PassiveSkill(collision);

        if (IsCoolTime) return;
        collision.gameObject.GetComponent<Sticky>().SetSticky();
        collision.transform.SetParent(transform);
        CoolTime();
    }
    #endregion

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if(collision.gameObject.TryGetComponent(out Sticky s))
        {
            PassiveSkill(collision);
        }
    }
}
