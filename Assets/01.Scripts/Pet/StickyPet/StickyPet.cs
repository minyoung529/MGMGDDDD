using DG.Tweening;
using System.Collections;
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
    public StickyState State => state;
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


    protected override void Awake()
    {
        base.Awake();
        smallDirection = transform.forward;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //if (stickyObject && stickyObject.transform.parent == stickyParent && stickyObject.ApplyOffset) // 오프셋 맞추기
        //{
        //    //stickyObject.transform.position = stickyParent.position + stickyOffset;
        //    stickyObject.MovableRoot.rotation = origianalRotation;
        //}
    }

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        NotSticky();
        skillEffect.Play();
        stickyObject = null;
        ChangeState(StickyState.Idle);

        scaleObject.DOKill();
        scaleObject.DOScale(Vector3.one, 0.5f);
        IsMovePointLock = false;

        OnExitBillow?.Invoke();
    }

    public void ChangeState(StickyState setState)
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
        if (state == StickyState.Billow) return;
        ChangeState(StickyState.Billow);

        if(agent!= null)
        {
            SetNavIsStopped(true);
            SetTarget(null);
        }

        BillowAction(bigScale);

        IsMovePointLock = true;
        OnBillow?.Invoke();
    }

    public override void InteractionPoint()
    {
        base.InteractionPoint();
        ReadySticky();
    }

    private void BillowAction(Vector3 _scale)
    {
        transform.DOKill();

        transform.forward = smallDirection;
        scaleObject.DOScale(_scale, 0.5f);

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

            transform.DOMove(hit, moveSpeed);
        }
    }

    private void Sticky(Sticky sticky)
    {
        if (state == StickyState.Sticky) return;
        ChangeState(StickyState.Sticky);
        stickyObject = sticky;
        skillEffect.Play();
        transform.DOKill();

        if (sticky.CanMove)
        {
            SetTarget(null);

            SetNavEnabled(false);
            SetNavEnabled(true);
        }
        else
        {
            SetNavEnabled(false);
        }

            // SET ORIGINAL PARENT & PARENT
            StartCoroutine(DelayParent());

            // SET VARIABLE
            //originalParent = stickyObject.MovableRoot.parent;
            //stickyOffset = stickyObject.MovableRoot.position - stickyParent.position;
            //origianalRotation = stickyObject.MovableRoot.rotation;
        
        stickyObject.OnSticky(this);

        sticky.StartListeningNotSticky(NotSticky);
        sticky.StartListeningChangeCanMove(CanMove);
    }

    // TEST
    private IEnumerator DelayParent()
    {
        yield return null;

        originalParent = stickyObject.MovableRoot.parent;
        stickyOffset = stickyObject.MovableRoot.position - stickyParent.position;
        origianalRotation = stickyObject.MovableRoot.rotation;
        stickyObject.MovableRoot.SetParent(stickyParent);
        stickyObject.MovableRoot.DOLocalMove(new Vector3(0f, 1f, 0f), 1f);
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

    public void NotSticky()
    {
        ChangeState(StickyState.Idle);

        SetNavEnabled(true);
        if (stickyObject)
        {
            stickyObject.MovableRoot.SetParent(originalParent);
            stickyObject.NotSticky();
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