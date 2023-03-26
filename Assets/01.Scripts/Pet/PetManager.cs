using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoSingleton<PetManager>
{
    private List<Image> petImages = new List<Image>();
    private List<Image> petInvens = new List<Image>();
    private List<Pet> pets = new List<Pet>();

    private int selectIndex = -1;
    private float switchDelay = 0.2f;

    private bool isSwitching = false;

    private Vector3 scaleUp = new Vector3(1.25f, 1.25f, 1.25f);
    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);

    #region Get
    public int PetCount { get { return pets.Count; } }
    #endregion 

    protected override void Awake()
    {
        base.Awake();

        StartListen();
        CutSceneManager.AddStartCutscene(InactivePetCanvas);
        CutSceneManager.AddEndCutscene(ActivePetCanvas);
    }

    private void Start()
    {
        ResetPetManager();
    }

    private void Update()
    {
        for (int i = 0; i < pets.Count; i++)
        {
            pets[i].OnUpdate();
        }
    }

    public bool IsGet(Pet p)
    {
        return pets.Contains(p);
    }


    private void OnDestroy()
    {
        CutSceneManager.RemoveStartCutscene(InactivePetCanvas);
        CutSceneManager.RemoveEndCutscene(ActivePetCanvas);
        StopListen();
    }

    #region Listen

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Up_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Down_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Select_First_Pet, SelectPet);
        InputManager.StartListeningInput(InputAction.Select_Second_Pet, SelectPet);
        InputManager.StartListeningInput(InputAction.Select_Third_Pet, SelectPet);

        InputManager.StartListeningInput(InputAction.Pet_Skill, OnSkill);
        InputManager.StartListeningInput(InputAction.Pet_Move, OnClickMove);
        InputManager.StartListeningInput(InputAction.Pet_Follow, OnWithdraw);
        InputManager.StartListeningInput(InputAction.Pet_Skill_Up, OnSkillUp);
    }

    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Up_Pet, SwitchPet);
        InputManager.StopListeningInput(InputAction.Down_Pet, SwitchPet);
        InputManager.StopListeningInput(InputAction.Select_First_Pet, SelectPet);
        InputManager.StopListeningInput(InputAction.Select_Second_Pet, SelectPet);
        InputManager.StopListeningInput(InputAction.Select_Third_Pet, SelectPet);

        InputManager.StopListeningInput(InputAction.Pet_Skill, OnSkill);
        InputManager.StopListeningInput(InputAction.Pet_Move, OnClickMove);
        InputManager.StopListeningInput(InputAction.Pet_Follow, OnWithdraw);
        InputManager.StopListeningInput(InputAction.Pet_Skill_Up, OnSkillUp);
    }

    private void OnSkillUp(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        pets[selectIndex].SkillUp();
    }

    private void OnWithdraw(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        pets[selectIndex].Withdraw();
    }

    private void OnClickMove(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        pets[selectIndex].MovePoint();
    }

    private void OnSkill(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        pets[selectIndex].Skill();
    }

    #endregion

    #region SwitchPet

    private void SwitchPet(InputAction input, float addIndex)
    {
        if (PetCount <= 0) return;

        addIndex = 0;

        if (input == InputAction.Up_Pet && !isSwitching) addIndex = -1;
        else if (input == InputAction.Down_Pet && !isSwitching) addIndex = 1;
        else return;

        SwitchPetIndex((int)addIndex);
    }
    private void SwitchPetIndex(int addIndex)
    {
        selectIndex += addIndex;

        if (selectIndex >= pets.Count) selectIndex = 0;
        else if (selectIndex < 0) selectIndex = pets.Count - 1;

        StartCoroutine(SwitchDelay(selectIndex));
    }
    private IEnumerator SwitchDelay(int newIndex)
    {
        isSwitching = true;

        if (newIndex <= 0)
        {
            SelectPet(0);
        }
        else
        {
            SelectPet(newIndex);
        }

        yield return new WaitForSeconds(switchDelay);
        isSwitching = false;
    }

    #endregion

    #region SELECT

    public void SelectPet(InputAction input, float index)
    {
        if (pets.Count <= 0) return;
        switch (input)
        {
            case InputAction.Select_First_Pet:
                {
                    selectIndex = 0;
                }
                break;
            case InputAction.Select_Second_Pet:
                {
                    if (pets.Count == 1) return;
                    selectIndex = 1;
                }
                break;
            case InputAction.Select_Third_Pet:
                {
                    if (pets.Count == 2) return;
                    selectIndex = 2;
                }
                break;
        }
        OnSelectPetUI(selectIndex);
    }

    public void SelectPet(int index)
    {
        selectIndex = index;
        OnSelectPetUI(selectIndex);
    }

    public void NotSelectPet()
    {
        selectIndex = -1;
        OffSelectPetUI();
    }

    #endregion

    #region Set
    private void ResetPetManager()
    {
        pets.Clear();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            GameObject inven = transform.GetChild(0).GetChild(i).gameObject;
            petInvens.Add(inven.GetComponent<Image>());
            petImages.Add(inven.transform.GetChild(0).gameObject.GetComponent<Image>());
        }

        selectIndex = -1;
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
        selectIndex = pets.Count - 1;

        ActivePetUI(selectIndex);
        SelectPet(selectIndex);
    }
    public void DeletePet(Pet p)
    {
        pets.Remove(p);

        OffSelectPetUI();
        DisablePetUI(--selectIndex);
    }

    #endregion

    #region PetUI

    private void OnSelectPetUI(int index)
    {
        for (int i = 0; i < pets.Count; i++)
        {
            petInvens[i].DOFade(0.2f, 0.5f);
            petImages[i].DOFade(0.2f, 0.5f);
            petInvens[i].transform.DOScale(defaultScale, 0.5f);
        }
        petInvens[index].DOFade(1f, 0.5f);
        petImages[index].DOFade(1f, 0.5f);
        petInvens[index].transform.DOScale(scaleUp, 0.5f).SetEase(Ease.Flash);
    }
    private void OffSelectPetUI()
    {
        for (int i = 0; i < pets.Count; i++)
        {
            petInvens[i].transform.DOScale(defaultScale, 0.5f);
        }
    }

    private void ActivePetUI(int index)
    {
        petImages[index].sprite = pets[index].petSprite;
        petInvens[index].gameObject.SetActive(true);
    }
    private void DisablePetUI(int index)
    {
        petInvens[index].gameObject.SetActive(false);
    }

    private void ActivePetCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void InactivePetCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion
}