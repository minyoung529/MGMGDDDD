using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonScript : MonoBehaviour
{
    [Header("초기 설정")]
    [SerializeField] private CannonCaliber caliberPref;
    [SerializeField] private Transform barrel; 
    [SerializeField] private Transform destination;
    [SerializeField] private float firePow = 500;
    [SerializeField] private float radius = 1f;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private ParticleSystem smoke;

    #region 인 게임 변수
    private List<Pet> pets = new List<Pet>();
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
        pets.Add(pet);
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
        if (pets.Count < 1) {
            smoke.Play();
            return;
        }
        explosion.Play();
        CannonCaliber caliber = Instantiate(caliberPref, barrel);
        caliber.transform.SetParent(null);
        caliber.transform.localScale = Vector3.one;
        caliber.Fire(this, pets.ToArray(), barrel.up * firePow);
        pets.Clear();
    }

    private void OnDisable() {
        seq.Kill();
    }
}