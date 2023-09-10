using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 펫 묶어놓는 스크립트
/// </summary>
public class CatchingPet : MonoBehaviour
{
    [SerializeField]
    private Transform petTransform;

    private List<Pet> catchedPets = new List<Pet>();
    private List<GameObject> catchedPetGameObjects = new List<GameObject>();

    private int index = 0;

    private float fixedY = 0f;

    [SerializeField]
    private UnityEvent onCatchPet;

    private void Start()
    {
        fixedY = petTransform.position.y;
    }

    private void Update()
    {
        if (catchedPets.Count > 0)
        {
            for (int i = 0; i < catchedPets.Count; i++)
            {
                if(!catchedPets[i].gameObject.activeSelf)
                {
                    return;
                }

                if (catchedPets[i].Agent.enabled && !catchedPets[i].Agent.isStopped)
                {
                    catchedPets[i].Agent.SetDestination(transform.position);
                    catchedPets[i].Agent.stoppingDistance = i * 1f + 5f;
                }
            }
        }
    }

    public void EquipPet(Pet pet, Action onEquipEnd)
    {
        if (catchedPets.Contains(pet)) return;

        pet.State.ChangeState((int)PetStateName.Idle);  // 펫 동작 끄기
        pet.SetNavIsStopped(true);
        pet.SetNavEnabled(false);
        pet.Rigid.velocity = Vector3.zero;

        // 펫 없애기
        PetManager.Instance.DeletePet(pet.GetPetType);  // DeletePet을 하면 inactive가 되기 때문에
        pet.gameObject.SetActive(true);                 // 다시 Active를 켜줌

        catchedPets.Add(pet);
        catchedPetGameObjects.Add(pet.gameObject);

        onCatchPet?.Invoke();
        StartCoroutine(EquipAnimation(pet, onEquipEnd));
        index++;
    }

    private IEnumerator EquipAnimation(Pet pet, Action onEquipEnd)
    {
        yield return new WaitForSeconds(0.6f);

        Vector3 start = pet.transform.position;
        Vector3 end = petTransform.transform.position;
        float dist = Vector3.Distance(start, end);
        Vector3 mid = start + (end - start).normalized * dist * 0.5f;

        // Y 동일해 물리 버그 나지 않게
        start.y = end.y = fixedY;
        mid.y = start.y + 4f;

        float duration = 1f;
        pet.transform.position = start;

        yield return null;

        pet.transform.DOPath(new Vector3[] { start, mid, end }, duration, PathType.CatmullRom);

        yield return new WaitForSeconds(duration + 0.5f);

        pet.Rigid.velocity = Vector3.zero;
        pet.SetNavIsStopped(false);
        pet.SetNavEnabled(true);
        pet.SetTarget(transform);

        Debug.Log("On Equip ENd");
        onEquipEnd?.Invoke();
        index++;
    }

    [ContextMenu("UnEquipAllPets")]
    public void UnEquipAllPets()
    {
        foreach (Pet pet in catchedPets)
        {
            // 다시 펫 얻기
            pet.GetPet(GameManager.Instance.PlayerController.transform);

            pet.State.ChangeState((int)PetStateName.Idle);
            pet.Event.TriggerEvent((int)PetEventName.OnRecallKeyPress);

            index--;
        }

        index = 0;
        catchedPets.Clear();
        catchedPetGameObjects.Clear();
    }

    public bool IsContain(Pet pet)
    {
        return catchedPets.Contains(pet);
    }

    public bool IsContain(GameObject pet)
    {
        return catchedPetGameObjects.Contains(pet);
    }
}