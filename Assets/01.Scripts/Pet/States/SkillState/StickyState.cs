using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyState : MonoBehaviour
{
    [SerializeField] private ParticleSystem skillEffect;
    [SerializeField] private Transform stickyParent;
    [SerializeField] private UnityEvent stickyEvent;

    private Transform originalParent = null;
    private Quaternion origianalRotation;

    private StickyPet pet;
    private Sticky sticky;

    public Sticky StickyObject => sticky;

    private void Start()
    {
        pet = transform.parent.GetComponent<StickyPet>();
    }

    public void OnSticky()
    {
        if (SelectedObject.CurInteractObject == null)
        {
            pet.State.ChangeState((int)PetStateName.Idle);
            return;
        }

        sticky = SelectedObject.CurInteractObject.GetComponent<Sticky>();
        if (sticky == null)
        {
            pet.State.ChangeState((int)PetStateName.Idle);
            return;
        }

        skillEffect.Play();
        stickyEvent?.Invoke();

        sticky.StartListeningNotSticky(OffSticky);
        sticky.OnSticky(pet);
        StartCoroutine(DelayParent());
    }

    private void OffSticky()
    {
        if (sticky == null) return;

        pet.SetInteractNull();
        pet.Rigid.isKinematic = false;
        pet.Rigid.useGravity = true;

        sticky.MovableRoot.SetParent(originalParent);

        sticky = null;
    }

    private IEnumerator DelayParent()
    {
        yield return null;

        originalParent = sticky.MovableRoot.parent;
        origianalRotation = sticky.MovableRoot.rotation;
        sticky.MovableRoot.SetParent(stickyParent);
        sticky.MovableRoot.DOLocalMove(new Vector3(0f, 1f, 0f), 1f);
    }

}
