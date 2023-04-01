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
    [SerializeField] private float radius = 1f;

    #region �� ���� ����
    private List<Pet> pets;
    private Sequence seq;
    private bool isPlay = false;
    #endregion

    private void Awake() {
        seq = DOTween.Sequence();
    }

    private void Update() {
        Collider[] petColls = Physics.OverlapSphere(transform.position, radius, 1 << Define.PET_LAYER);
        foreach (Collider item in petColls) {
            Pet pet = item.GetComponent<Pet>();
            if(!pet) {
                Debug.LogError($"������Ʈ {item.name}�� Pet�� ���̾ �����Ǿ� ������ �� ������Ʈ�� �������� �ʽ��ϴ�!");
                continue;
            }
            GetInCannon(pet);
        }
    }

    public void GetInCannon(Pet pet) {
        pet.Coll.enabled = false;
        pet.Rigid.isKinematic = true;
        seq.Kill();
        seq.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f));
        seq.Join(pet.transform.DOMove(barrel.position, 0.5f));
        seq.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
    }

    public void TriggerCannon() {
        if (isPlay) return;
        isPlay = true;
        seq.Kill();
        seq.Append(transform.DOScale(new Vector3(1.1f, 0.7f, 1.1f), 2f));
        seq.Append(transform.DOScale(new Vector3(0.9f, 1.1f, 0.9f), 1f));
        seq.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
        seq.AppendCallback(FireCannon);
    }

    private void FireCannon() {
        isPlay = false;
        CannonCaliber caliber = Instantiate(caliberPref, barrel);
        caliber.transform.SetParent(null);
        Vector3[] path = new Vector3[4];
        path[0] = barrel.position;
        path[1] = Vector3.Lerp(transform.position, destination.position, 0.3f) + Vector3.up * 2f;
        path[2] = Vector3.Lerp(transform.position, destination.position, 0.6f) + Vector3.up;
        path[3] = destination.position;
        caliber.Fire(pets.ToArray(), path);
    }
}