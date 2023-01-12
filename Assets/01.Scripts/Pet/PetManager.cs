using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoBehaviour
{
    public static PetManager instance;

    public List<Image> petInvens = new List<Image>();
    private List<Pet> pets = new List<Pet>();

    private Color selectDefaultColor = Color.white;

    private int petIndex = 0; // Æê °³¼ö

    private bool isSelect = false;

    private int selectIndex = 0;
    private bool isSwitching = false;

    private void Awake()
    {
        instance = this;
        ResetPetSetting();
    }

    private void ResetPetSetting()
    {
        pets.Clear();
        petIndex = 0;
        isSelect= false;
        for (int i = 0; i < 3; i++)
        {
            petInvens[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (pets.Count == 0) return;
       
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
        {
            selectIndex++;
            if (selectIndex >= pets.Count) selectIndex = 0;
            StartCoroutine(SwitchDelay(selectIndex));
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            selectIndex--;
            if (selectIndex < 0) selectIndex = pets.Count - 1;
            StartCoroutine(SwitchDelay(selectIndex));
        }
    }

    private IEnumerator SwitchDelay(int newIndex)
    {
        isSwitching = true;

        if (!isSelect)
        {
            SelectPet(0);
            yield return new WaitForSeconds(0.3f);
        }
        SelectPet(newIndex);
        yield return new WaitForSeconds(0.3f);
        isSwitching = false;
    }

    private void SelectPet(int selectIndex)
    {
        Debug.Log(selectIndex);   
        OnSelect(true, selectIndex);
        for(int i=0;i<pets.Count;i++)
        {
            pets[i].OnSelected(false);
        }
        pets[selectIndex].OnSelected(true);
    }

    public void OnSelect(bool isOn, int index)
    {
        isSelect = isOn;
        if(isOn) SelectedPetUI(true, index);
    }
    public void OnSelect(bool isOn)
    {
        isSelect = isOn;
        if(!isOn) SelectedPetUI(false, 0);
    }

    private void SelectedPetUI(bool isOn, int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (petInvens[i].gameObject.activeSelf) petInvens[i].color = selectDefaultColor;
        }
        if(isOn)  petInvens[index].color = pets[index].selectColor;
    }


    public int GetPetIndex(Pet p)
    {
        return pets.FindIndex(e => e == p);
    }

    public void AddPet(Pet p)
    {
        pets.Add(p);
        ++petIndex;
        petInvens[petIndex - 1].gameObject.SetActive(true);
    }

    public void DeletePet(Pet p)
    {
        pets.Remove(p);
        petInvens[--petIndex].gameObject.SetActive(false);
    }


}
