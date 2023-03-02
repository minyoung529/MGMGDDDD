using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoSingleton<PetManager>
{
    public List<Image> petInvens = new List<Image>();
    private List<Pet> pets = new List<Pet>();

    private int petIndex = -1;
    private int selectIndex = 0;

    private bool isSelect = false;
    private bool isSwitching = false;

    private Vector3 scaleUp = new Vector3(1.33f, 1.33f, 1.33f);
    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);

    #region Get
    public int PetCount { get { return pets.Count; } }
    public bool IsSelected { get { return isSelect; } }
    #endregion 

    private void Awake()
    {
        ResetPetManager();
    }
    private void Start()
    {
        // StartListen();
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

    private void OnDestroy()
    {
        // StopListen();
    }

    #region Listen

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Next_Pet, SwitchPet);
    }
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Next_Pet, SwitchPet);
    }

    #endregion

    #region SwitchPet
    public bool IsPetSelected(int index)
    {
        return selectIndex == index;
    }
    private void SwitchPet(InputAction input, float addIndex)
    {
        if (isSwitching || PetCount <= 0) return;

        selectIndex += (int)addIndex;

        if (selectIndex >= pets.Count) selectIndex = 0;
        else if (selectIndex < 0) selectIndex = pets.Count - 1;

        StartCoroutine(SwitchDelay(selectIndex));
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

    public void SelectPet(int index)
    {
        isSelect = true;

        for (int i = 0; i < pets.Count; i++)
        {
            pets[i].Select(false);
        }
        pets[index].Select(true);
        OnSelectPetUI(index);
    }
    public void NotSelectPet()
    {
        isSelect = false;
        OffSelectPetUI();
        for (int i = 0; i < pets.Count; i++)
        {
            pets[i].Select(false);
        }
    }

    #endregion

    #region Set
    private void ResetPetManager()
    {
        pets.Clear();

        petIndex = -1;
        selectIndex = 0;
        isSelect = false;
        isSwitching = false;

        OffSelectPetUI();
        for (int i = 0; i < petInvens.Count; i++)
        {
            DisablePetUI(i);
        }
    }

    public void AddPet(Pet p)
    {
        pets.Add(p);

        ++petIndex;
        ActivePetUI(petIndex);
        SelectPet(petIndex);
    }
    public void DeletePet(Pet p)
    {
        pets.Remove(p);

        OffSelectPetUI();
        DisablePetUI(--petIndex);
    }

    #endregion

    #region PetUI

    private void OnSelectPetUI(int index)
    {
        OffSelectPetUI();
        petInvens[index].transform.DOScale(scaleUp, 1f);
    }
    private void OffSelectPetUI()
    {
        for (int i = 0; i < pets.Count; i++)
        {
             petInvens[i].transform.DOScale(defaultScale, 1f);
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