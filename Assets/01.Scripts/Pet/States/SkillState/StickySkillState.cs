using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class StickySkillState : PetState
{
    public override PetStateName StateName => PetStateName.Skill;

    [SerializeField] private Transform explosion;
    [SerializeField] private UnityEvent OnBillow;
    [SerializeField] private UnityEvent OnExitBillow;

    [SerializeField]
    private SkillVisual enterVisual;
    [SerializeField]
    private SkillVisual exitVisual;

    private Vector3 smallDirection;


    public override void OnEnter()
    {
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);

        Billow();
    }

    public override void OnExit()
    {
        exitVisual.Trigger();

        explosion.gameObject.SetActive(false);
        OnExitBillow?.Invoke();

        pet.Event.StopListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }

    public override void OnUpdate()
    {
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
    private void Billow()
    {
        BillowAction();
        enterVisual.Trigger();
        OnBillow?.Invoke();
        explosion.gameObject.SetActive(true);
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
    #endregion

}
