using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class StickyPet : Pet
{
    private float moveSpeed = 1f;


    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        rigid.useGravity = false;
        rigid.isKinematic = false;
        agent.enabled= false;
    }

    public override void AppearPet()
    {
        base.AppearPet();

        rigid.useGravity = true;
    }

    #endregion

    #region Skill

    // Active Skill
    protected override void ClickActive()
    {
        base.ClickActive();
        if (!IsSkilling) return;

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            isSkilling = false;
            MoveActiveSkill(hit);
        }
    }
    private void MoveActiveSkill(RaycastHit hit)
    {
        isStop = true;
        agent.enabled = false;

        transform.DOKill();
        transform.DOScale(new Vector3(1.5f , 1.5f, 1.5f), 1f);
        transform.DOMove(hit.point, moveSpeed).OnComplete(()=>
        {
            StickySkill(hit.collider.gameObject);
            CoolTime(Define.ACTIVE_COOLTIME_TYPE);
        });
    }

    // Passive Skill
    protected override void PassiveSkill(Collision collision)
    {
        base.PassiveSkill(collision);

        if (IsPassiveCoolTime) return;
        collision.gameObject.GetComponent<Sticky>().SetSticky();

        StickySkill(collision.gameObject);
        CoolTime(Define.PASSIVE_COOLTIME_TYPE);
    }

    private void StickySkill(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null) obj.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.detectCollisions = true;

        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = rb;
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
