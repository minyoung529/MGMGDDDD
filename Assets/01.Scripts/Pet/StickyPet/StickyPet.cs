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
    [SerializeField] private Transform scaleObject;

    private StickyState state = StickyState.Idle;
    private Vector3 bigScale = new Vector3(3f, 3f, 3f);
    private Vector3 smallDirection;

    private float moveSpeed = 1f;

    [SerializeField]
    private UnityEvent OnBillow;
    [SerializeField]
    private UnityEvent OnExitBillow;

    [SerializeField]
    private Transform stickyParent;

    private Sticky stickyObject = null;
    private Vector3 stickyOffset;
    private Quaternion origianalRotation;
    private Transform originalParent = null;
    private bool stickyKinematic = false;


    protected override void Awake()
    {
        base.Awake();
        smallDirection = transform.forward;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (stickyObject && stickyObject.ApplyOffset) // 오프셋 맞추기
        {
            stickyObject.transform.position = stickyParent.position + stickyOffset;
            stickyObject.MovableRoot.rotation = origianalRotation;
        }
    }

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        NotSticky();
        skillEffect.Play();
        stickyObject = null;
        ChangeState(StickyState.Idle);

        scaleObject.DOScale(Vector3.one, 0.5f);

        OnExitBillow?.Invoke();
    }

    private void ChangeState(StickyState setState)
    {
        state = setState;
    }

    #endregion

    #region Skill

    // Active Skill
    public override void Skill()
    {
        if (IsCoolTime) return;
        base.Skill();

        Billow();
    }

    private void Billow()
    {
        // ǳ��ó�� ��Ǫ�� �ൿ�� �����ϴ� �Լ�
        if (state == StickyState.Billow) return;
        ChangeState(StickyState.Billow);

        SetTarget(null);

        transform.DOKill();
        SetNavIsStopped(true);

        BillowAction();
        OnBillow?.Invoke();
    }

    public override void InteractionPoint()
    {
        base.InteractionPoint();
        ReadySticky();
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
        if (state == StickyState.Billow || state == StickyState.Sticky) return;
        ChangeState(StickyState.ReadySticky);

        Vector3 hit = GameManager.Instance.GetCameraHit();
        if (hit != Vector3.zero)
        {
            SetTarget(null);

            transform.DOMoveX(hit.x, moveSpeed);
            transform.DOMoveY(hit.y, moveSpeed);
            transform.DOMoveZ(hit.z, moveSpeed);
        }
    }

    private void Sticky(Sticky sticky)
    {
        if (state == StickyState.Sticky) return;
        ChangeState(StickyState.Sticky);

        //if (!sticky.CanSticky) return;

        stickyObject = sticky;
        skillEffect.Play();

        if (sticky.CanMove)
        {
            SetTarget(null);
            Rigid.isKinematic = false;
        }
        else
        {
            //SetNavIsStopped(true);
            SetNavEnabled(false);
            Rigid.isKinematic = true;
        }

        if (stickyObject.Rigidbody)
        {
            stickyKinematic = stickyObject.Rigidbody.isKinematic;
        }

        if (stickyKinematic || stickyObject.Rigidbody == null)
        {
            originalParent = stickyObject.MovableRoot.parent;
            stickyObject.MovableRoot.SetParent(stickyParent);

            stickyOffset = stickyObject.MovableRoot.position - stickyParent.position;
            origianalRotation = stickyObject.MovableRoot.rotation;
        }
        else
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = stickyObject.Rigidbody;
        }

        stickyObject.OnSticky();

        sticky.StartListeningNotSticky(NotSticky);
        sticky.StartListeningChangeCanMove(CanMove);
    }

    public void CanMove(bool canMove)
    {
        if (!stickyObject) return;

        if (canMove)
        {
            SetNavEnabled(true);
            SetTarget(null);
        }
        else
        {
            SetNavEnabled(false);
        }
    }

    private void NotSticky()
    {
        ChangeState(StickyState.Idle);

        //SetNavIsStopped(false);
        SetNavEnabled(true);
        if (stickyKinematic && stickyObject)
        {
            stickyObject.MovableRoot.SetParent(originalParent);
        }
        else
        {
            FixedJoint[] joints = GetComponents<FixedJoint>();
            for (int i = 0; i < joints.Length; i++)
            {
                Destroy(joints[i]);
            }
        }

        skillEffect.Play();
        Rigid.isKinematic = false;
        Rigid.useGravity = true;
        stickyObject = null;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        if (state == StickyState.ReadySticky)
        {
            Sticky stickyObject = other.GetComponent<Sticky>();
            if (stickyObject != null)
            {
                SetBillow(other.transform.forward);

                Sticky(stickyObject);
            }
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

    public void SetAnimation(bool value)
    {
    }

    #endregion

}