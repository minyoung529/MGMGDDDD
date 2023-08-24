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
    private List<Image> petImages = new List<Image>();
    private List<Image> petInvens = new List<Image>();
    private List<Pet> pets = new List<Pet>();
    private List<Type> petTypes = new List<Type>();

    private int beforeIndex = -1;
    private int selectIndex = -1;
    private float switchDelay = 0.2f;

    private bool isSwitching = false;

    private Vector3 scaleUp = new Vector3(1.25f, 1.25f, 1.25f);
    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);

    [SerializeField]
    private SpriteAtlas spriteAtlas;

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
                SceneController.ListeningEnter(SceneType.StartScene, InactivePetCanvas);
            }
            else
            {

                SceneController.ListeningEnter((SceneType)i, ActivePetCanvas);
            }
        }
    }

    private void Start()
    {
        CutSceneManager.Instance.AddStartCutscene(InactivePetCanvas);
        CutSceneManager.Instance.AddStartCutscene(StopAllListen);
        CutSceneManager.Instance.AddEndCutscene(ActivePetCanvas);
        CutSceneManager.Instance.AddEndCutscene(StartAllListen);
    }

    private void LoadPet(EventParam eventParam = null)
    {
        ResetPetManager();
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

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug_CreateAndGetPet();
#endif
    }

    public bool IsGet(Pet p)
    {
        return pets.Contains(p);
    }

    private void OnDestroy()
    {
        CutSceneManager.Instance?.RemoveStartCutscene(InactivePetCanvas);
        CutSceneManager.Instance?.RemoveEndCutscene(ActivePetCanvas);
        EventManager.StopListening(EventName.LoadChapter, LoadPet);
        EventManager.StopListening(EventName.StopPetAllListen, StopPetAllListen);
        StopAllListen();

        for (int i = 0; i < (int)SceneType.Count; i++)
        {
            if (i == (int)SceneType.StartScene)
            {
                SceneController.StopListeningEnter(SceneType.StartScene, InactivePetCanvas);
            }
            else
            {

                SceneController.StopListeningEnter((SceneType)i, ActivePetCanvas);
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

        InputManager.StopListeningInput(inputAction, inputActions[inputAction]);
        inputActions[inputAction] -= action;
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
        // ���� ���� ��
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

        if (pets[selectIndex].GetPetType == PetType.OilPet)
            pets[selectIndex].Event.TriggerEvent((int)PetEventName.OnSkillCancel);
        if (beforeIndex > -1) pets[beforeIndex].Event.TriggerEvent((int)PetEventName.OnOffPing);

        addIndex = 0;

        if (input == InputAction.Up_Pet && !isSwitching) addIndex = -1;
        else if (input == InputAction.Down_Pet && !isSwitching) addIndex = 1;
        else return;

        SwitchPetIndex(selectIndex + (int)addIndex);
    }
    private void SwitchPetIndex(int addIndex)
    {
        beforeIndex = selectIndex;
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
        OnSelectPetUI(selectIndex);
    }

    public void NotSelectPet()
    {
        beforeIndex = selectIndex;
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
    public void ResetPetManager()
    {
        pets.Clear();
        petTypes.Clear();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            GameObject inven = transform.GetChild(0).GetChild(i).gameObject;
            if (!inven.activeSelf) continue;
            petInvens.Add(inven.GetComponent<Image>());
            petImages.Add(inven.transform.GetChild(0).gameObject.GetComponent<Image>());
        }
        beforeIndex = -1;
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
        if (pets.Contains(p)) return;
        pets.Add(p);
        petTypes.Add(p.GetType());

        SelectPet(pets.Count - 1);
        ActivePetUI(selectIndex);
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

        DisablePetUI(index);

        petTypes.RemoveAt(index);
        pets.Remove(p);

        selectIndex = pets.Count - 1;

        SelectPet(0);
        p.gameObject.SetActive(false);
    }

    public void DeletePet(int index)
    {
        DisablePetUI(index);

        petTypes.RemoveAt(index);
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
        if (index < 0 || index >= petInvens.Count) return;

        petInvens[index].gameObject.SetActive(true);
        petImages[index].sprite = spriteAtlas.GetSprite(pets[index].petSprite.name);
    }
    private void DisablePetUI(int index)
    {
        if (index < 0 || index >= petInvens.Count) return;

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