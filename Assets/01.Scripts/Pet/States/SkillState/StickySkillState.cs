using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class StickySkillState : PetState
{
    public override PetStateName StateName => PetStateName.Skill;

    [SerializeField] private Transform scaleObject;
    [SerializeField] private Transform explosion;
    [SerializeField] private UnityEvent OnBillow;
    [SerializeField] private UnityEvent OnExitBillow;

    private Vector3 smallDirection;
    private Vector3 bigScale = new Vector3(3f, 3f, 3f);


    public override void OnEnter()
    {
        Billow();
    }

    public override void OnExit()
    {
        scaleObject.DOKill();
        scaleObject.DOScale(Vector3.one, 0.5f);

        explosion.gameObject.SetActive(false);
        OnExitBillow?.Invoke();
    }

    public override void OnUpdate()
    {

    }

    private void Awake()
    {
        explosion.gameObject.SetActive(false);
        smallDirection = transform.forward;
    }

    private void Billow()
    {
        transform.DOKill();
        BillowAction();
        OnBillow?.Invoke();
        explosion.gameObject.SetActive(true);
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

    private void OnTriggerEnter(Collider other)
    {
        Sticky stickyObject = other.GetComponent<Sticky>();
        if (stickyObject != null)
        {
            SetBillow(other.transform.forward);
        }
    }

}
