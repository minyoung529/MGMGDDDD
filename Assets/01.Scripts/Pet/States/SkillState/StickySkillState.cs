using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class StickySkillState : PetState
{
    public override PetStateName StateName => PetStateName.Skill;

    [SerializeField] private Transform explosion;
    [SerializeField] private UnityEvent onBillow;
    [SerializeField] private UnityEvent onExitBillow;

    [SerializeField]
    private SkillVisual enterVisual;
    [SerializeField]
    private SkillVisual exitVisual;

    private Vector3 smallDirection;

    public override void OnEnter()
    {
        pet.Skilling = true;

        pet.Event.StartListening((int)PetEventName.OnSkillCancel, OffBillow);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.State.BlockState((int)PetStateName.Move);

        OnBillow();
    }

    public override void OnExit()
    {
        pet.Skilling = false;
        exitVisual.Trigger();
        pet.State.UnBlockState((int)PetStateName.Move);

        explosion.gameObject.SetActive(false);
        onExitBillow?.Invoke();

        pet.Event.StopListening((int)PetEventName.OnSkillCancel, OffBillow);
        pet.Event.StopListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }


    private void Awake()
    {
        explosion.gameObject.SetActive(false);
        smallDirection = transform.forward;
    }

    #region Listen

    private void OnRecall()
    {
        pet.State.ChangeState((int)PetStateName.Recall);
    }

    #endregion

    #region Skill
    private void OnBillow()
    {
        BillowAction();
        enterVisual.Trigger();
        onBillow?.Invoke();
        explosion.gameObject.SetActive(true);
    }

    private void OffBillow()
    {
        pet.State.ChangeState((int)PetStateName.Idle);
    }

    private void BillowAction()
    {
        transform.forward = smallDirection;
        smallDirection = Vector3.zero;
    }

    private void SetBillow(Vector3 dir)
    {
        smallDirection = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        Sticky stickyObject = other.GetComponent<Sticky>();
        if (stickyObject != null)
        {
            SetBillow(other.transform.forward);
        }
    }

    public override void OnUpdate()
    {
    }
    #endregion

}
