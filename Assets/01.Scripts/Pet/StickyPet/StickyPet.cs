using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class StickyPet : Pet
{
    private float moveSpeed = 1f;
    private bool isStopGear = false;

    protected override void Awake()
    {
        base.Awake();

    }

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

        transform.DOKill();
        transform.DOScale(new Vector3(1.5f , 1.5f, 1.5f), 1f);
        transform.DOMove(hit.point, moveSpeed).OnComplete(()=>
        {
            //transform.SetParent(hit.transform);

            //rigid.useGravity= false;
            rigid.isKinematic = true;
            rigid.detectCollisions = true;
            
            HingeJoint joint = hit.collider.gameObject.AddComponent<HingeJoint>();
            hit.rigidbody.isKinematic = true;
            joint.connectedBody = rigid;
        });
        
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


    private void OnCollisionExit(Collision collision)
    {
    }
}
