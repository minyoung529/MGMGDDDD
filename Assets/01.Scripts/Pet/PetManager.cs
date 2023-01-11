using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    public static PetManager instance;

    List<Pet> pets = new List<Pet>();

    private int petIndex = 0;

    private void Awake()
    {
        instance = this; 
    }

    private void Update()
    {
        if (pets.Count == 0) return;
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput > 0)
        {
            // 휠을 밀어 돌렸을 때의 처리 ↑
            UpSelect();
            Debug.Log("UP: "+petIndex);
        }
        else if (wheelInput < 0)
        {
            // 휠을 당겨 올렸을 때의 처리 ↓
            DownSelect();
            Debug.Log("Down: "+petIndex);
        }
    }

    private void UpSelect()
    {
        pets[petIndex].OnSelected(false);
        petIndex++;
        if(petIndex == pets.Count)
        {
            petIndex = 0;
        }
        pets[petIndex].OnSelected(true);
    }
    private void DownSelect()
    {
        pets[petIndex].OnSelected(false);
        petIndex--;
        if(petIndex == -1)
        {
            petIndex = pets.Count-1;
        }
        pets[petIndex].OnSelected(true);
    }

    public int GetPetIndex(Pet p)
    {
        return pets.FindIndex(e => e == p);
    }

    public void AddPet(Pet p)
    {
        pets.Add(p);
        if(pets.Count==1) {
            pets[0].OnSelected(true);
        }
    }

    public void DeletePet(Pet p)
    {
        pets.Remove(p);
    }


}
