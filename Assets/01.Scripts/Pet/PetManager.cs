using System;
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
    private int selectIndex = 0;

    private bool isSelect = false;
    private bool isSwitching = false;
    public bool IsSelecting() { return isSelect; }
    public bool IsSwitching() { return isSwitching; }

    private void Awake()
    {
        instance = this;

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
    #endregion

    #endregion

    #region Set
    private void ResetPetManager()
    {
        pets.Clear();
        petIndex = 0;
        isSelect = false;
        for (int i = 0; i < 3; i++)
        {
            petInvens[i].gameObject.SetActive(false);
        }
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
    #endregion
}
