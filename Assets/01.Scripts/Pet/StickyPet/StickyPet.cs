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
    private Vector3 bigScale = new Vector3(3f, 3f, 3f);
    private Vector3 smallDirection;

    [SerializeField]
    private UnityEvent OnBillow;
    [SerializeField]
    private UnityEvent OnExitBillow;

    [SerializeField]
    private Transform scaleObject;

    protected override void Awake()
    {
        base.Awake();
        smallDirection = transform.forward;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.X)) ReadySticky();
    }

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        NotSticky();
        skillEffect.Play();
        ChangeState(StickyState.Idle);

        scaleObject.DOScale(Vector3.one, 0.5f);

        OnExitBillow?.Invoke();
    }

    private void SetMove(bool canMove)
    {
        if (canMove)
        {
           //StartFollow();
            IsFollow = true;
        }
        else
        {
            IsFollow = false;
        }

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
        if (state == StickyState.Billow) return;
        ChangeState(StickyState.Billow);

        StopClickMove();
        StopFollow();

        transform.DOKill();
        SetMove(false);

        BillowAction();
        OnBillow?.Invoke();
    }

    private void BillowAction()
    {
        transform.forward = smallDirection;
        scaleObject.DOScale(bigScale, 0.5f);

        smallDirection = Vector3.zero;
    }

    private void SetBillow(Vector3 dir)
    {
        smallDirection = dir;
    }

    private void ReadySticky()
    {
        if(state == StickyState.Billow || state == StickyState.Sticky) return;
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

    private void Sticky(Sticky stickyObject)
    {
        if (state == StickyState.Sticky) return;
        ChangeState(StickyState.Sticky);

        skillEffect.Play();
        Rigid.isKinematic = true;
        SetMove(stickyObject.CanMove);

        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = stickyObject.GetComponent<Rigidbody>();
    }
    private void NotSticky()
    {
        ChangeState(StickyState.Idle);

        FixedJoint[] joints = GetComponents<FixedJoint>();
        for (int i = 0; i < joints.Length; i++)
        {
            Destroy(joints[i]);
        }
        SetMove(true);

        skillEffect.Play();
        Rigid.isKinematic = false;
        Rigid.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == StickyState.ReadySticky)
        {
            Sticky stickyObject = collision.collider.GetComponent<Sticky>();
            if (stickyObject != null)
            {
                SetBillow(collision.transform.forward);

                Sticky(stickyObject);
            }
        }
        else if (state == StickyState.Billow)
        {
            Vector3 point = (collision.contacts[0].point - transform.position).normalized;
            //MoveScale(point);
        }
    }

    private void MoveScale(Vector3 point)
    {
        Vector3 remaining = bigScale - transform.localScale;
        Debug.Log(point);

        point.x *= remaining.x;
        point.y *= remaining.y;
        point.z *= remaining.z;
        Vector3 targetPosition = point;

        transform.DOMove(transform.position + targetPosition, 0.1f);
    }


    #endregion

}