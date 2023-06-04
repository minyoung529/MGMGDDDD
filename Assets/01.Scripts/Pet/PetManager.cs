using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PetManager : MonoSingleton<PetManager>
{
    private List<Image> petImages = new List<Image>();
    private List<Image> petInvens = new List<Image>();
    private List<Pet> pets = new List<Pet>();
    private List<Type> petTypes = new List<Type>();

    private int selectIndex = -1;
    private float switchDelay = 0.2f;

    private bool isSwitching = false;

    private Vector3 scaleUp = new Vector3(1.25f, 1.25f, 1.25f);
    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);

    #region Get
    public int PetCount { get { return pets.Count; } }
    public Pet GetSelectPet { get { return pets[selectIndex]; } }
    public List<Pet> GetPetList { get { return pets; } }
    #endregion 

    [SerializeField]
    private GameObject[] petPrefabs;

    private Dictionary<InputAction, Action<InputAction, float>> inputActions = new();

    public Pet GetPetByKind<T>() where T : Pet
    {
        Pet pet = null;

        foreach (Pet p in pets)
        {
            if (p)
            {
                pet ??= p.GetComponent<T>();
            }
        }

        if (pet == null)
            pet = FindObjectOfType<T>();

        return pet;
    }

    protected override void Awake()
    {
        base.Awake();

        inputActions.Add(InputAction.Up_Pet, SwitchPet);
        inputActions.Add(InputAction.Down_Pet, SwitchPet);
        inputActions.Add(InputAction.Select_First_Pet, SelectPet);
        inputActions.Add(InputAction.Select_Second_Pet, SelectPet);
        inputActions.Add(InputAction.Select_Third_Pet, SelectPet);
        inputActions.Add(InputAction.Pet_Skill, OnSkill);
        inputActions.Add(InputAction.Pet_Move, OnClickMove);
        inputActions.Add(InputAction.Pet_Skill_Up, OnSkillUp);
        inputActions.Add(InputAction.Pet_Follow, ReCall);

        StartAllListen();
    }

    private void Start()
    {
        ResetPetManager();

        CutSceneManager.Instance.AddStartCutscene(InactivePetCanvas);
        CutSceneManager.Instance.AddStartCutscene(StopAllListen);
        CutSceneManager.Instance.AddEndCutscene(ActivePetCanvas);
        CutSceneManager.Instance.AddEndCutscene(StartAllListen);
    }


    private void Update()
    {
        for (int i = 0; i < pets.Count; i++)
        {
            if (pets[i] == null)
            {
                pets[i] = FindObjectOfType(petTypes[i]) as Pet;

                if (pets[i] == null)
                {
                    // LATER: FIX
                    DeletePet(i);
                    break;
                }

                pets[i]?.SetPlayerTransform(GameManager.Instance.PlayerController.transform);
                pets[i]?.SetTargetPlayer();
            }

            pets[i]?.OnUpdate();
        }

        Debug_CreateAndGetPet();
    }

    public bool IsGet(Pet p)
    {
        return pets.Contains(p);
    }

    private void OnDestroy()
    {
        CutSceneManager.Instance?.RemoveStartCutscene(InactivePetCanvas);
        CutSceneManager.Instance?.RemoveEndCutscene(ActivePetCanvas);
        StopAllListen();
    }

    #region Listen
    private void StartAllListen()
    {
        foreach (var pair in inputActions)
        {
            StartListen(pair.Key, pair.Value);
        }
    }

    private void StopAllListen()
    {
        foreach (var pair in inputActions)
        {
            StopListen(pair.Key, pair.Value);
        }
    }

    public void StartListen(InputAction inputAction, Action<InputAction, float> action = null)
    {
        if (!inputActions.ContainsKey(inputAction))
        {
            if (action == null) return;
            inputActions.Add(inputAction, action);
        }

        InputManager.StartListeningInput(inputAction, inputActions[inputAction]);
    }

    public void StopListen(InputAction inputAction, Action<InputAction, float> action = null)
    {
        if (!inputActions.ContainsKey(inputAction))
        {
            if (action == null) return;
            inputActions.Add(inputAction, action);
        }

        InputManager.StopListeningInput(inputAction, inputActions[inputAction]);
    }

    private void OnSkillUp(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        //pets[selectIndex].SkillUp();
        pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillKeyUp);
    }

    private void OnClickMove(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

        pets[selectIndex].MovePoint();

        if (SelectedObject.GetInteract() == null) return;
        pets[selectIndex].InteractionPoint();
    }

    private void OnSkill(InputAction input, float value)
    {
        // ÆêÀÌ ¾øÀ» ¶§
        if (selectIndex < 0) return;

        if (pets[selectIndex].Skilling)
        {
            pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillCancel);
        }
        else
        {
            pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillKeyPress);
        }
    }

    private void ReCall(InputAction input, float value)
    {
        if (pets.Count == 0) return;

        pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnRecallKeyPress);
    }
    #endregion

    #region SwitchPet

    private void SwitchPet(InputAction input, float addIndex)
    {
        if (PetCount <= 0) return;

        if (pets[selectIndex].GetPetType == PetType.OilPet)
            pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillCancel);

        addIndex = 0;

        if (input == InputAction.Up_Pet && !isSwitching) addIndex = -1;
        else if (input == InputAction.Down_Pet && !isSwitching) addIndex = 1;
        else return;

        SwitchPetIndex(selectIndex + (int)addIndex);
    }
    private void SwitchPetIndex(int addIndex)
    {
        selectIndex = addIndex;

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
        if (pets[selectIndex].GetPetType == PetType.OilPet)
            pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillCancel);

        switch (input)
        {
            case InputAction.Select_First_Pet:
                {
                    SelectPet(0);
                }
                break;
            case InputAction.Select_Second_Pet:
                {
                    if (pets.Count <= 1) return;
                    SelectPet(1);
                }
                break;
            case InputAction.Select_Third_Pet:
                {
                    if (pets.Count <= 2) return;
                    SelectPet(2);
                }
                break;
        }
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

    public Pet GetSelectedPet()
    {
        if (PetCount < 1 || pets.Count <= selectIndex) return null;
        return pets[selectIndex];
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
        petTypes.Add(p.GetType());
        selectIndex = pets.Count - 1;

        ActivePetUI(selectIndex);
        SelectPet(selectIndex);
    }
    public void DeletePet(PetType type)
    {
        Pet p = GetPetByKind<StickyPet>();
        int index = pets.IndexOf(p);
        petTypes.RemoveAt(index);

        DisablePetUI(index);

        petInvens.Remove(petInvens[index]);
        petImages.Remove(petImages[index]);
        pets.Remove(p);

        SelectPet(0);
        p.gameObject.SetActive(false);
    }

    public void DeletePet(int index)
    {
        petTypes.RemoveAt(index);

        DisablePetUI(index);

        petInvens.Remove(petInvens[index]);
        petImages.Remove(petImages[index]);
        pets.RemoveAt(index);

        SelectPet(0);
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

    public void ActivePetCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void InactivePetCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion

    #region ACCESS
    public void AllPetActions(Action<Pet> action)
    {
        pets.ForEach(action);
    }

    public bool Contain(Pet pet)
    {
        return pets.Contains(pet);
    }
    #endregion

    #region Debug
    private void Debug_CreateAndGetPet()
    {
        for (int i = 0; i < petPrefabs.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha4 + i)))
            {
                Pet pet = Instantiate(petPrefabs[i], GameManager.Instance.PlayerController.transform.position, Quaternion.identity).GetComponent<Pet>();
                pet.GetPet(GameManager.Instance.PlayerController.transform);
            }
        }
    }
    #endregion
}