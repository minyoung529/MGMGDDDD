using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    #region 키 매핑
    [SerializeField] private List<InputSave> defaultSave = new List<InputSave>();
    private static string SAVE_PATH;
    private const string SAVE_FILE = "/KeyMapping";
    private static Dictionary<InputAction, InputCode> keyDictionary = new Dictionary<InputAction, InputCode>();
    #endregion

    #region 이벤트
    private static Dictionary<KeyCode, Dictionary<InputType, Action<float>>> keyEventDictionary
        = new Dictionary<KeyCode, Dictionary<InputType, Action<float>>>();
    private static Dictionary<InputType, Action<float>> scrollEventDictionary
        = new Dictionary<InputType, Action<float>>();
    #endregion

    private void Awake() {
        SAVE_PATH = string.Concat(Application.persistentDataPath, "/Control");
        LoadKeyMapping();
        SaveKeyMapping(keyDictionary);
    }

    private void Update() {
        CheckKeyInput();
        CheckScrollInput();
    }

    private void LoadKeyMapping() {
        if(!File.Exists(string.Concat(SAVE_PATH, SAVE_FILE))) {
            foreach(InputSave save in defaultSave) {
                keyDictionary.Add(save.Action, save.Code);
            }
            return;
        }

        string jsonSave = File.ReadAllText(string.Concat(SAVE_PATH, SAVE_FILE));
        List<InputSave> inputSave = JsonConvert.DeserializeObject<List<InputSave>>(jsonSave);
        keyDictionary.Clear();
        foreach(InputSave save in inputSave) {
            keyDictionary.Add(save.Action, save.Code);
        }
    }

    private void SaveKeyMapping(Dictionary<InputAction, InputCode> keyDictionary) {
        if (!Directory.Exists(SAVE_PATH)) {
            Directory.CreateDirectory(SAVE_PATH);
        }
        List<InputSave> save = new List<InputSave>();
        foreach (KeyValuePair<InputAction, InputCode> pair in keyDictionary) {
            save.Add(new InputSave(pair.Key, pair.Value));
        }
        string jsonSave = JsonConvert.SerializeObject(save.ToArray(), Formatting.Indented);
        File.WriteAllText(string.Concat(SAVE_PATH, SAVE_FILE), jsonSave);
    }

    public void ChangeKeyMapping(InputAction action, InputCode inputCode) {
        if(inputCode.keyCode != KeyCode.None) {
            keyDictionary[action] = inputCode;
        }
        SaveKeyMapping(keyDictionary);
    }

    //딕셔너리 돌면서 키 인풋 받기
    private void CheckKeyInput() {
        foreach (KeyCode keyCode in keyEventDictionary.Keys) {
            foreach (InputType inputType in keyEventDictionary[keyCode].Keys) {
                switch (inputType) {
                    case InputType.GetKeyDown:
                        if (Input.GetKeyDown(keyCode))
                            keyEventDictionary[keyCode][inputType].Invoke(0);
                            break;
                    case InputType.GetKey:
                        if (Input.GetKey(keyCode))
                            keyEventDictionary[keyCode][inputType].Invoke(0);
                            break;
                    case InputType.GetKeyUp:
                        if (Input.GetKeyUp(keyCode))
                            keyEventDictionary[keyCode][inputType].Invoke(0);
                            break;
                }
            }
        }
    }

    //딕셔너리 돌면서 스크롤 인풋 받기
    private void CheckScrollInput() {
        float delta = Input.mouseScrollDelta.y;
        if (delta > 0) {
            scrollEventDictionary[InputType.ScrollUp].Invoke(delta);
        }
        else if (delta < 0) {
            scrollEventDictionary[InputType.ScrollDown].Invoke(delta);
        }
    }

    static public void StartListeningInput(InputAction action, InputType type, Action<float> listener) {
        InputCode input = keyDictionary[action];
        if (input.keyCode != KeyCode.None) {
            KeyCode key = input.keyCode;
            if (!keyEventDictionary.ContainsKey(key)) {
                keyEventDictionary.Add(key, new Dictionary<InputType, Action<float>>());
                if (!keyEventDictionary[key].ContainsKey(type)) {
                    keyEventDictionary[key].Add(type, listener);
                    return;
                }
            }
            keyEventDictionary[key][type] += listener;
        }
        else if (input.scroll) {
            if (!scrollEventDictionary.ContainsKey(type)) {
                scrollEventDictionary.Add(type, listener);
                return;
            }
            scrollEventDictionary[type] += listener;
        }
        else {
            Debug.LogError($"Action : {action} did not mapped any key!");
        }
    }

    static public void StopListeningInput(InputAction action, InputType type, Action<float> listener) {
        InputCode input = keyDictionary[action];
        if (input.keyCode != KeyCode.None) {
            KeyCode key = input.keyCode;
            if (!keyEventDictionary.ContainsKey(key) || !keyEventDictionary[key].ContainsKey(type)) {
                Debug.LogWarning("You are trying to remove Action that does not existent");
                return;
            }
            keyEventDictionary[key][type] -= listener;
        }
        else if (input.scroll) {
            if (!scrollEventDictionary.ContainsKey(type)) {
                Debug.LogWarning("You are trying to remove Action that does not existent");
                return;
            }
            scrollEventDictionary[type] -= listener;
        }
        else {
            Debug.LogError($"Action : {action} did not mapped any key!");
        }
    }

    /// <param name="pos">마우스 포인터의 레이가 충돌한 지점</param>
    /// <returns>마우스 포인터의 레이가 충돌 감지 시 true 아니면 false</returns>
    static public bool GetMouseRayPos(out Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane, Define.BOTTOM_LAYER)) {
            pos = hit.point;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }
}