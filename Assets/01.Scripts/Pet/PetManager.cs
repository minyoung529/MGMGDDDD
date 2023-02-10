using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoSingleton<PetManager>
{
    public List<Image> petInvens = new List<Image>();
    private List<Pet> pets = new List<Pet>();

    private Color selectDefaultColor = Color.white;

    private int petIndex = 0; // ?? ????
    private int selectIndex = 0;

    private bool isSelect = false;
    private bool isSwitching = false;
    public bool IsSelecting{ get { return isSelect; } }
    public bool IsSwitching { get { return isSwitching; } }

    private void Awake()
    {
        ResetPetManager();
    }
    private void Update()
    {
        if (pets.Count == 0) return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
        {
            SwitchPet(1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            SwitchPet(-1);
        }
    }

    #region SwitchPet
    public int GetPetIndex(Pet p)
    {
        return pets.FindIndex(e => e == p);
    }
    private void SwitchPet(int addIndex)
    {
        selectIndex += addIndex;

        if (selectIndex >= pets.Count) selectIndex = 0;
        else if (selectIndex < 0) selectIndex = pets.Count - 1;

        StartCoroutine(SwitchDelay(selectIndex));
    }
    private IEnumerator SwitchDelay(int newIndex)
    {
        isSwitching = true;

        if (!isSelect)
        {
            SelectPet(0);
        }
        else
        {
            SelectPet(newIndex);
        }

        yield return new WaitForSeconds(0.3f);
        isSwitching = false;
    }
    #endregion

    #region SELECT

    public void SelectPet(int selectIndex)
    {
        isSelect = true;
        OnSelectPetUI(selectIndex);

        for(int i=0;i<pets.Count;i++)
        {
            pets[i].IsSelected=false;
        }
        pets[selectIndex].IsSelected=true;
    }
    public void NotSelectPet()
    {
        isSelect = false;
        OffSelectPetUI();

        for(int i=0;i<pets.Count;i++)
        {
            pets[i].IsSelected = false;
        }
    }

    #endregion

    #region Set
    private void ResetPetManager()
    {
        pets.Clear();

        selectIndex = 0;
        petIndex = 0;
        isSelect = false;
        isSwitching = false;

        for (int i = 0; i < 3; i++)
        {
            petInvens[i].gameObject.SetActive(false);
        }
    }

    public void AddPet(Pet p)
    {
        pets.Add(p);
        ++petIndex;
        ActivePetUI(petIndex - 1);
    }
    public void DeletePet(Pet p)
    {
        pets.Remove(p);
        DisablePetUI(--petIndex);
    }
    #endregion

    #region SelectUI
    private void OnSelectPetUI(int index)
    {
        OffSelectPetUI();
        petInvens[index].color = pets[index].selectColor;
    }
    private void OffSelectPetUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if (petInvens[i].gameObject.activeSelf) petInvens[i].color = selectDefaultColor;
        }
    }

    private void ActivePetUI(int index)
    {
        petInvens[index].gameObject.SetActive(true);
    }
    private void DisablePetUI(int index)
    {
        petInvens[index].gameObject.SetActive(false);
    }
    #endregion
}
