using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 펫 묶어놓는 스크립트
/// </summary>
public class CatchingPet : MonoBehaviour
{
    [SerializeField]
    private Transform[] petTransforms;

    private List<Pet> catchedPets = new List<Pet>();

    private int index = 0;

    private float fixedY = 0f;

    private void Start()
    {
        fixedY = petTransforms[0].position.y;
    }

    private void Update()
    {
        if (catchedPets.Count > 0)
        {
            for (int i = 0; i < catchedPets.Count; i++)
            {
                if (catchedPets[i].Agent.enabled)
                {
                    catchedPets[i].Agent.SetDestination(transform.position);
                    catchedPets[i].Agent.stoppingDistance = i * 1f + 5f;
                }
            }
        }
    }

    public void EquipPet(Pet pet, Action onEquipEnd)
    {
        if (index >= petTransforms.Length)
            return;

        if (catchedPets.Contains(pet)) return;

        pet.State.ChangeState((int)PetStateName.Idle);  // 펫 동작 끄기
        pet.SetNavIsStopped(true);
        pet.SetNavEnabled(false);
        pet.Rigid.velocity = Vector3.zero;

        // 펫 없애기
        PetManager.Instance.DeletePet(pet.GetPetType);

        catchedPets.Add(pet);

        StartCoroutine(EquipAnimation(pet, onEquipEnd));
        index++;
    }

    private IEnumerator EquipAnimation(Pet pet, Action onEquipEnd)
    {
        Vector3 start = pet.transform.position;
        Vector3 end = petTransforms[index].transform.position;
        float dist = Vector3.Distance(start, end);
        Vector3 mid = start + (end - start).normalized * dist * 0.5f;

        // Y 동일해 물리 버그 나지 않게
        start.y = end.y = fixedY;
        mid.y = start.y + 5f;

        float duration = 1f;
        pet.transform.position = start;

        yield return null;

        pet.transform.DOPath(new Vector3[] { start, mid, end }, duration, PathType.CatmullRom);

        yield return new WaitForSeconds(duration + 0.5f);

        pet.Rigid.velocity = Vector3.zero;
        pet.SetNavEnabled(true);
        pet.SetNavIsStopped(false);
        pet.SetTarget(transform);

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
            pet.SetTargetNull();

            index--;
        }

        index = 0;
        catchedPets.Clear();
    }

    public bool IsContain(Pet pet)
    {
        return catchedPets.Contains(pet);
    }
}