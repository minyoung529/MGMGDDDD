using DG.Tweening;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class StickyPet : Pet
{
    [SerializeField] private ParticleSystem skillEffect;
    [SerializeField] private GameObject jumpObject;

    private bool isSticky = false;
    private bool readySticky = false;
    private float moveSpeed = 1f;

    private Sticky sticky = null;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(Input.GetKeyDown(KeyCode.X)) ReadySticky();
    }

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        jumpObject.transform.localScale = new Vector3(1, 1, 1);
        jumpObject.SetActive(false);
        skillEffect.Play();
    }

    #endregion

    #region Skill

    // Active Skill
    protected override void Skill(InputAction inputAction, float value)
    {
        if (CheckSkillActive) return;
        base.Skill(inputAction, value);

        Billow();
    }

    private void Billow()
    {
        NotSticky();

        // 풍선처럼 부푸는 행동을 구현하는 함수
        jumpObject.SetActive(true);
        jumpObject.transform.DOScale(transform.localScale + new Vector3(3f, 3f, 3f), 0.5f);

        IsNotMove = true;

        SetJump();
    }

    private void SetJump()
    {
        // 점프대 점프할 수 있도록 설정하는 곳
        // 민영아 여기다가 하면 돼
    }

    private void ReadySticky()
    {
        readySticky = true;

        Vector3 hit = GameManager.Instance.GetCameraHit();
        if (hit != Vector3.zero)
        {
            StopClickMove();
            StopFollow();

            transform.DOMoveX(hit.x, moveSpeed);
            transform.DOMoveY(hit.y, moveSpeed);
            transform.DOMoveZ(hit.z, moveSpeed);
        }
    }

    private void NotSticky()
    {
        if (!isSticky) return;

        IsNotMove = false;
        Destroy(GetComponent<FixedJoint>());
        sticky.NotSticky();

        skillEffect.Play();
        Rigid.isKinematic = false;
        Rigid.useGravity = true;
        readySticky = false;

        isSticky = false;
        sticky = null;
    }

    private void Sticky(Sticky stickyObject)
    {
        isSticky = true;

        stickyObject.SetSticky();
        sticky = stickyObject;

        skillEffect.Play();
        Rigid.isKinematic = true;

        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = sticky.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (readySticky)
        {
            Sticky stickyObject = collision.collider.GetComponent<Sticky>();
            if (stickyObject != null) Sticky(stickyObject);
        }
    }

    #endregion

}