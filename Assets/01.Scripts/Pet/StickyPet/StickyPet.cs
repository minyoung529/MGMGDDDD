using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public enum StickyState
{
    Idle,
    Move,
    ReadySticky,
    Sticky,
    Billow
}
public class StickyPet : Pet
{
    [SerializeField] private ParticleSystem skillEffect;

    private float moveSpeed = 1f;

    private StickyState state = StickyState.Idle;
    private Vector3 bigScale = new Vector3(3f, 1f, 3f);
    private Vector3 smallDirection;

    [SerializeField]
    private UnityEvent OnBillow;

    [SerializeField]
    private UnityEvent OnExitBillow;

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

        NotSticky();
        agent.enabled = true;
        skillEffect.Play();

        OnExitBillow?.Invoke();
    }

    private void SetMove(bool canMove)
    {
        CanMove = canMove;
        agent.enabled = canMove;
    }

    private void ChangeState(StickyState setState)
    {
        state = setState;
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
        // 풍선처럼 부푸는 행동을 구현하는 함수
        SetMove(false);

        OnBillow?.Invoke();

        BillowAction();
        SetJump();
    }

    private void BillowAction()
    {
        transform.forward = smallDirection;
        transform.DOScale(bigScale, 0.5f);

        smallDirection = Vector3.zero;
    }

    private void SetBillow(Vector3 dir)
    {
        smallDirection = dir;
    }
    private void SetJump()
    {
        // 점프대 점프할 수 있도록 설정하는 곳
        // 민영아 여기다가 하면 돼
    }

    private void ReadySticky()
    {
        if (state == StickyState.ReadySticky) return;
        ChangeState(StickyState.ReadySticky);

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
        if (state != StickyState.Sticky) return;

        CanMove = false;
        Destroy(GetComponent<FixedJoint>());

        skillEffect.Play();
        Rigid.isKinematic = false;
        Rigid.useGravity = true;

        ChangeState(StickyState.Idle);
    }

    private void Sticky(Sticky stickyObject)
    {
        if (state == StickyState.Sticky) return;

        skillEffect.Play();
        agent.enabled = stickyObject.CanMove;
        CanMove = !stickyObject.CanMove;

        ChangeState(StickyState.Sticky);

        Rigid.isKinematic = true;

        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = stickyObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == StickyState.ReadySticky)
        {
            Sticky stickyObject = collision.collider.GetComponent<Sticky>();
            if (stickyObject != null)
            {
                Vector3 dir = (collision.contacts[0].point - transform.position).normalized;
                SetBillow(collision.transform.forward);
                Sticky(stickyObject);
            }
        }

    }

    #endregion

}