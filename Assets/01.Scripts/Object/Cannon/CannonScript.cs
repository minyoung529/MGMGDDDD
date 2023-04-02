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
    [SerializeField] private float radius = 1f;

    #region 인 게임 변수
    private List<Pet> pets;
    private Sequence seq;
    private bool isPlay = false;
    #endregion

    private void Update() {
        Collider[] petColls = Physics.OverlapSphere(transform.position, radius, 1 << Define.PET_LAYER);
        foreach (Collider item in petColls) {
            Pet pet = item.GetComponent<Pet>();
            if(!pet) {
                Debug.LogError($"오브젝트 {item.name}은 Pet의 레이어가 설정되어 있지만 펫 컴포넌트가 존재하지 않습니다!");
                continue;
            }
            GetInCannon(pet);
        }
    }

    public void GetInCannon(Pet pet) {
        pets.Add(pet);
        pet.Coll.enabled = false;
        pet.Rigid.isKinematic = true;
        seq = DOTween.Sequence();
        seq.Append(barrel.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f));
        seq.Join(pet.transform.DOMove(barrel.position, 0.1f));
        seq.Append(barrel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
    }

    [ContextMenu("Test1")]
    public void TriggerCannon() {
        if (isPlay) return;
        isPlay = true;
        seq = DOTween.Sequence();
        seq.Append(barrel.transform.DOScale(new Vector3(1.2f, 0.6f, 1.2f), 0.5f));
        seq.AppendInterval(0.1f);
        seq.Append(barrel.transform.DOScale(new Vector3(0.9f, 1.3f, 0.9f), 0.2f));
        seq.AppendCallback(FireCannon);
        seq.Append(barrel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f));
        isPlay = false;
    }

    [ContextMenu("Test2")]
    private void FireCannon() {
        if (pets.Count < 1) return;
        CannonCaliber caliber = Instantiate(caliberPref, barrel);
        caliber.transform.SetParent(null);
        Vector3[] path = new Vector3[4];
        path[0] = barrel.position;
        path[1] = Vector3.Lerp(transform.position, destination.position, 0.3f) + Vector3.up * 2f;
        path[2] = Vector3.Lerp(transform.position, destination.position, 0.6f) + Vector3.up;
        path[3] = destination.position;
        caliber.Fire(pets.ToArray(), path);
    }

    private void OnDisable() {
        seq.Kill();
    }
}