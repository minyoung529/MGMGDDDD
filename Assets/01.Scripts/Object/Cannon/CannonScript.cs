using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CannonScript : MonoBehaviour
{
    [Header("초기 설정")]
    [SerializeField] private Transform barrel;
    [SerializeField] private float firePow = 500;
    [SerializeField] private float radius = 1f;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private UnityEvent shotEvent;

    private Collider[] colls;
    private Collider petColl;

    public Pet InPet { get { return pet; } }

    #region 인 게임 변수
    private Pet pet;
    private bool isPlay = false;
    private Sequence seq;
    #endregion

    private void Awake()
    {
        colls = GetComponentsInChildren<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!pet && collision.gameObject.layer != Define.PET_LAYER) return;

        petColl = collision.collider;
        StartCoroutine(SetIgnore(0f, true));
        GetInCannon(collision.transform.GetComponent<Pet>());
    }

    public void GetInCannon(Pet pet)
    {
        if (this.pet != null) return;

        this.pet = pet;
        pet.Event.TriggerEvent((int)PetEventName.OnHold);
        seq = DOTween.Sequence();
        seq.Append(barrel.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f));
        seq.Join(pet.transform.DOMove(barrel.position, 0.1f));
        seq.Append(barrel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
    }

    [ContextMenu("Trigger")]
    public void TriggerCannon()
    {
        if (isPlay) return;
        isPlay = true;

        shotEvent?.Invoke();
        seq = DOTween.Sequence();
        seq.Append(barrel.transform.DOScale(new Vector3(1.2f, 0.6f, 1.2f), 0.5f));
        seq.AppendInterval(0.1f);
        seq.AppendCallback(FireCannon);
        seq.Append(barrel.transform.DOScale(new Vector3(0.9f, 1.3f, 0.9f), 0.2f));
        seq.Append(barrel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f));
        seq.AppendCallback(() => isPlay = false);
    }

    private void FireCannon()
    {
        if (!pet)
        {
            smoke.Play();
            return;
        }
        explosion.Play();

        pet.Throw(barrel.up * firePow);
        StartCoroutine(SetIgnore(0.5f, false));
        pet = null;
    }

    public void GetOutPet() {
        if (!pet) return;
        pet.Throw(barrel.up * 500);
        StartCoroutine(SetIgnore(0.5f, false));
        pet = null;
    }

    private IEnumerator SetIgnore(float time, bool value)
    {
        yield return new WaitForSeconds(time);
        foreach (Collider coll in colls)
        {
            Physics.IgnoreCollision(coll, petColl, value);
        }
    }

    private void OnDisable()
    {
        seq.Kill();
    }
}