using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.U2D;

public class PetManager : MonoSingleton<PetManager>
{
    private List<Pet> pets = new List<Pet>();
    private List<Type> petTypes = new List<Type>();

    private int beforeIndex = -1;
    private int selectIndex = -1;
    private float switchDelay = 0.2f;

    private bool isSwitching = false;

    #region Get
    public int PetCount { get { return pets.Count; } }
    public Pet GetSelectPet { get { return pets[selectIndex]; } }
    public List<Pet> GetPetList { get { return pets; } }
    #endregion 

    [SerializeField]
    private GameObject[] petPrefabs;

    private PetUI ui;
    public PetUI UI { get { return ui; } }

    private Dictionary<InputAction, Action<InputAction, float>> inputActions = new();

    public Pet GetPetByKind<T>() where T : Pet
    {
        Pet pet = GetMyPetByKind<T>();

        if (pet == null)
            pet = FindObjectOfType<T>();

        return pet;
    }

    public Pet GetMyPetByKind<T>() where T : Pet
    {
        Pet pet = null;

        foreach (Pet p in pets)
        {
            if (p)
            {
                pet ??= p.GetComponent<T>();
            }
        }

        return pet;
    }
    public Pet BindingPet(PetType type)
    {
        Pet pet = null;
        switch (type)
        {
            case PetType.None:
                break;
            case PetType.OilPet:
                pet = FindObjectOfType<OilPet>();
                break;
            case PetType.FirePet:
                pet = FindObjectOfType<FirePet>();
                break;
            case PetType.StickyPet:
                pet = FindObjectOfType<StickyPet>();
                break;
        }

        return pet;
    }
    public int GetIndexPetType(PetType type)
    {
        for (int i = 0; i < pets.Count; i++)
        {
            if (pets[i].GetPetType == type)
            {
                return i;
            }
        }
        return -1;
    }

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponent<PetUI>();
        inputActions.Add(InputAction.Up_Pet, SwitchPet);
        inputActions.Add(InputAction.Down_Pet, SwitchPet);
        inputActions.Add(InputAction.Select_First_Pet, SelectPet);
        inputActions.Add(InputAction.Select_Second_Pet, SelectPet);
        inputActions.Add(InputAction.Select_Third_Pet, SelectPet);
        inputActions.Add(InputAction.Pet_Skill, OnSkill);
        inputActions.Add(InputAction.Pet_Move, OnClickMove);
        inputActions.Add(InputAction.Pet_Skill_Up, OnSkillUp);
        inputActions.Add(InputAction.Pet_Follow, ReCall);
        inputActions.Add(InputAction.Pet_Interaction, OnPetInteraction);
        inputActions.Add(InputAction.Pet_Move_Up, OnMoveUp);

        StartAllListen();
        ResetPetManager();
        EventManager.StartListening(EventName.LoadChapter, LoadPet);
        EventManager.StartListening(EventName.StopPetAllListen, StopPetAllListen);
        EventManager.StartListening(EventName.StartPetAllListen, StartPetAllListen);

        for (int i = 0; i < (int)SceneType.Count; i++)
        {
            if (i == (int)SceneType.StartScene)
            {
                SceneController.ListeningEnter(SceneType.StartScene, UI.InactivePetCanvas);
            }
            else
            {
                SceneController.ListeningEnter((SceneType)i, UI.ActivePetCanvas);
            }
        }
    }

    private void Start()
    {
        CutSceneManager.Instance.AddStartCutscene(UI.InactivePetCanvas);
        CutSceneManager.Instance.AddStartCutscene(StopAllListen);
        CutSceneManager.Instance.AddEndCutscene(UI.ActivePetCanvas);
        CutSceneManager.Instance.AddEndCutscene(StartAllListen);
    }

    private void LoadPet(EventParam eventParam = null)
    {
        if (!eventParam.Contain("pets")) return;
        List<PetType> petList = SaveSystem.CurSaveData.pets;

        for (int i = 0; i < petList.Count; i++)
        {
            Pet bindingPet = BindingPet(petList[i]);
            if (bindingPet == null) return;

            bindingPet.GetPet(GameManager.Instance.PlayerController.transform, true);

            Vector3 offset = Vector3.right * 3f * i;
            offset.x -= 2f;
            bindingPet.SetForcePosition(GameManager.Instance.PlayerController.transform.position + offset);
        }
    }

    private void Update()
    {
        Debug_CreateAndGetPet();

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

    }

    public bool IsGet(Pet p)
    {
        return pets.Contains(p);
    }

    private void OnDestroy()
    {
        CutSceneManager.Instance?.RemoveStartCutscene(UI.InactivePetCanvas);
        CutSceneManager.Instance?.RemoveEndCutscene(UI.ActivePetCanvas);
        EventManager.StopListening(EventName.LoadChapter, LoadPet);
        EventManager.StopListening(EventName.StopPetAllListen, StopPetAllListen);
        StopAllListen();

        for (int i = 0; i < (int)SceneType.Count; i++)
        {
            if (i == (int)SceneType.StartScene)
            {
                SceneController.StopListeningEnter(SceneType.StartScene, UI.InactivePetCanvas);
            }
            else
            {

                SceneController.StopListeningEnter((SceneType)i, UI.ActivePetCanvas);
            }
        }
    }

    #region Listen
    private void StopPetAllListen(EventParam eventParam = null)
    {
        StopAllListen();
    }
    private void StartPetAllListen(EventParam eventParam = null)
    {
        StartAllListen();
    }

    private void StartAllListen()
    {
        foreach (var pair in inputActions)
        {
            InputManager.StopListeningInput(pair.Key, pair.Value);
            InputManager.StartListeningInput(pair.Key, pair.Value);
        }
    }

    private void StopAllListen()
    {
        foreach (var pair in inputActions)
        {
            InputManager.StopListeningInput(pair.Key, pair.Value);
        }
    }

    public void StartListen(InputAction inputAction, Action<InputAction, float> action = null)
    {
        if (!inputActions.ContainsKey(inputAction))
        {
            if (action == null) return;
            inputActions.Add(inputAction, action);
        }
        else
        {
            inputActions[inputAction] += action;
        }

        InputManager.StopListeningInput(inputAction, inputActions[inputAction]);
        InputManager.StartListeningInput(inputAction, inputActions[inputAction]);
    }

    public void StopListen(InputAction inputAction, Action<InputAction, float> action = null)
    {
        if (!inputActions.ContainsKey(inputAction))
        {
            if (action == null) return;
            inputActions.Add(inputAction, action);
        }

        if (action == null)
        {
            InputManager.StopListeningInput(inputAction, inputActions[inputAction]);
        }
        else
        {
            InputManager.StopListeningInput(inputAction, action);
            inputActions[inputAction] -= action;
        }
    }

    private void OnSkillUp(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        //pets[selectIndex].SkillUp();
        pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillKeyUp);
    }
    private void OnMoveUp(InputAction input, float value)
    {
        if (selectIndex < 0 || selectIndex >= pets.Count) return;
        pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnOffPing);
    }
    private void OnClickMove(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        if (selectIndex >= pets.Count) return;

        pets[selectIndex].MovePoint();
    }

    private void OnPetInteraction(InputAction input, float value)
    {
        if (selectIndex < 0) return;
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        if (selectIndex >= pets.Count) return;

        if (SelectedObject.GetInteract() == null) return;
        pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnInputInteractAction);
    }

    private void OnSkill(InputAction input, float value)
    {
        // ∆Í¿Ã æ¯¿ª ∂ß
        if (selectIndex < 0) return;
        if (selectIndex >= pets.Count) return;

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
        if (selectIndex >= pets.Count) return;
        if (selectIndex < 0) return;

        pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnRecallKeyPress);
    }
    #endregion

    #region SwitchPet
    private void SwitchPet(InputAction input, float addIndex)
    {
        if (PetCount <= 0) return;
        if (selectIndex >= pets.Count) return;

        if (pets[selectIndex].GetPetType == PetType.OilPet)
            pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillCancel);
        if (beforeIndex > -1) pets[beforeIndex].Event.TriggerEvent((int)PetEventName.OnOffPing);

        if (input == InputAction.Up_Pet && !isSwitching) addIndex = -1;
        else if (input == InputAction.Down_Pet && !isSwitching) addIndex = 1;
        else return;

        if (selectIndex + addIndex >= PetCount && selectIndex + addIndex < 0)
            return;

        SwitchPetIndex(selectIndex + (int)addIndex);
    }
    private void SwitchPetIndex(int addIndex)
    {
        beforeIndex = selectIndex;
        selectIndex = addIndex;

        if (selectIndex >= pets.Count) selectIndex = 0;
        else if (selectIndex < 0) selectIndex = pets.Count - 1;

        if (selectIndex + addIndex >= PetCount && selectIndex + addIndex < 0)
            return;

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
        if (beforeIndex > -1) pets[beforeIndex].Event.TriggerEvent((int)PetEventName.OnOffPing);

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
        beforeIndex = selectIndex;
        selectIndex = index;
        UI.SelectPetUI(pets[selectIndex].GetPetType);
    }

    public Pet GetSelectedPet()
    {
        if (PetCount < 1 || pets.Count <= selectIndex) return null;
        return pets[selectIndex];
    }
    #endregion

    #region Set
    public void ResetPetManager()
    {
        for (int i = 0; i < pets.Count; i++)
        {
            DeletePet(i);
        }

        petTypes.Clear();
        pets.Clear();

        UI.ResetPetUI();

        beforeIndex = -1;
        selectIndex = -1;
        isSwitching = false;
    }

    public void AddPet(Pet p)
    {
        if (pets.Contains(p)) return;
        pets.Add(p);
        petTypes.Add(p.GetType());

        UI.Active(p.GetPetType);
        SelectPet(pets.Count - 1);
    }
    public void DeletePet(PetType type)
    {
        Pet p = null;
        switch (type)
        {
            case PetType.FirePet:
                p = GetPetByKind<FirePet>();
                break;
            case PetType.StickyPet:
                p = GetPetByKind<StickyPet>();
                break;
            case PetType.OilPet:
                p = GetPetByKind<OilPet>();
                break;
        }
        if (p == null) return;

        int index = pets.IndexOf(p);
        if (index < 0) return;

        UI.Inactive(p.GetPetType);

        petTypes.RemoveAt(index);
        pets.Remove(p);

        selectIndex = -1;
        if (pets.Count > 0)
        {
            selectIndex = 0;
            SelectPet(0);
        }

        p.gameObject.SetActive(false);
    }

    public void DeletePet(int index)
    {
        if (pets[index] == null) return;
        UI.Inactive(pets[index].GetPetType);

        Pet pet = pets[index];
        petTypes.RemoveAt(index);
        pets.RemoveAt(index);

        Destroy(pet.gameObject);

        selectIndex = -1;
        if (pets.Count > 0)
        {
            selectIndex = 0;
            SelectPet(0);
        }
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetKey(KeyCode.Keypad0))
            {
                ResetPetManager();
            }
            else if(Input.GetKey(KeyCode.Keypad1)) {
                if (GetIndexPetType(PetType.FirePet) != -1) return;
                Pet pet = Instantiate(petPrefabs[0], GameManager.Instance.PlayerController.transform.position, Quaternion.identity).GetComponent<Pet>();
                pet.GetPet(GameManager.Instance.PlayerController.transform);
            }
            else if(Input.GetKey(KeyCode.Keypad2)) {
                if (GetIndexPetType(PetType.OilPet) != -1) return;
                Pet pet = Instantiate(petPrefabs[1], GameManager.Instance.PlayerController.transform.position, Quaternion.identity).GetComponent<Pet>();
                pet.GetPet(GameManager.Instance.PlayerController.transform);
            }
            else if(Input.GetKey(KeyCode.Keypad3)) {
                if (GetIndexPetType(PetType.StickyPet) != -1) return;
                Pet pet = Instantiate(petPrefabs[2], GameManager.Instance.PlayerController.transform.position, Quaternion.identity).GetComponent<Pet>();
                pet.GetPet(GameManager.Instance.PlayerController.transform);
            }
        }
    }
    #endregion
}