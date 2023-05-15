using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonScript : MonoBehaviour
{
    [Header("�ʱ� ����")]
    [SerializeField] private CannonCaliber caliberPref;
    [SerializeField] private Transform barrel; 
    [SerializeField] private Transform destination;
    [SerializeField] private float firePow = 500;
    [SerializeField] private float radius = 1f;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private ParticleSystem smoke;

    #region �� ���� ����
    private Pet inPet;
    private bool isPlay = false;
    private Sequence seq;
    #endregion

    private void Update() {
        Collider[] petColls = Physics.OverlapSphere(transform.position, radius, 1 << Define.PET_LAYER);
        foreach (Collider item in petColls) {
            Pet pet = item.GetComponent<Pet>();
            if(!pet) {
                continue;
            }
            GetInCannon(pet);
        }
    }

    public void GetInCannon(Pet pet) {
        inPet = pet;
        pet.SetNavEnabled(false);
        pet.Coll.enabled = false;
        pet.Rigid.isKinematic = true;
        seq = DOTween.Sequence();
        seq.Append(barrel.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f));
        seq.Join(pet.transform.DOMove(barrel.position, 0.1f));
        seq.Append(barrel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
    }

    [ContextMenu("Trigger")]
    public void TriggerCannon() {
        if (isPlay) return;

        isPlay = true;
        seq = DOTween.Sequence();
        seq.Append(barrel.transform.DOScale(new Vector3(1.2f, 0.6f, 1.2f), 0.5f));
        seq.AppendInterval(0.1f);
        seq.AppendCallback(FireCannon);
        seq.Append(barrel.transform.DOScale(new Vector3(0.9f, 1.3f, 0.9f), 0.2f));
        seq.Append(barrel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f));
        seq.AppendCallback(() => isPlay = false);
    }

    private void FireCannon() {
        if (inPet) {
            smoke.Play();
            return;
        }
        explosion.Play();

        //CannonCaliber caliber = Instantiate(caliberPref, barrel);
        //caliber.transform.SetParent(null);
        //caliber.transform.localScale = Vector3.one;
        //caliber.Fire(this, inPet, barrel.up * firePow);

        inPet.PetThrow.Throw(barrel.up * firePow);
        inPet = null;
    }

    private void OnDisable() {
        seq.Kill();
    }
}