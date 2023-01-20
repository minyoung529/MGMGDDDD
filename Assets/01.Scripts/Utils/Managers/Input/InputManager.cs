using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    #region 키 매핑
    private static string SAVE_PATH;
    private static string DEFAULT_PATH;
    private const string SAVE_FILE = "/KeyMapping";
    private static Dictionary<InputAction, InputCode> keyBinding = new Dictionary<InputAction, InputCode>();
    #endregion

    #region 이벤트
    private static Dictionary<KeyCode, Dictionary<InputType, Action<InputAction, InputType, float>>> keyEventDictionary
        = new Dictionary<KeyCode, Dictionary<InputType, Action<InputAction, InputType, float>>>();
    private static Dictionary<InputType, Action<InputAction, InputType, float>> scrollEventDictionary
        = new Dictionary<InputType, Action<InputAction, InputType, float>>();
    #endregion

    private void Awake() {
        SAVE_PATH = string.Concat(Application.persistentDataPath, "/Control");
        DEFAULT_PATH = string.Concat(Application.dataPath, "/KeyBinding/default.txt");
        LoadKeyMapping();
    }

    private void Update() {
        CheckInput();
    }

    private void OnApplicationQuit() {
        SaveKeyMapping(keyBinding);
    }

    private void LoadKeyMapping() {
        if(!File.Exists(string.Concat(SAVE_PATH, SAVE_FILE))) { //세이브 파일 없으면 디폴트 값 가져옴
            string json_Save = File.ReadAllText(string.Concat(DEFAULT_PATH));
            List<InputSave> input_Save = JsonConvert.DeserializeObject<List<InputSave>>(json_Save);
            foreach (InputSave save in input_Save) {
                keyBinding.Add(save.Action, save.Code);
            }
            return;
        }

        string jsonSave = File.ReadAllText(string.Concat(SAVE_PATH, SAVE_FILE));
        List<InputSave> inputSave = JsonConvert.DeserializeObject<List<InputSave>>(jsonSave);
        keyBinding.Clear();
        foreach(InputSave save in inputSave) {
            keyBinding.Add(save.Action, save.Code);
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
            keyBinding[action] = inputCode;
        }
        SaveKeyMapping(keyBinding);
    }

    private void CheckInput() {
        foreach (KeyValuePair<InputAction, InputCode> item in keyBinding) {
            if (item.Value.keyCode != KeyCode.None) {
                CheckKeycodeInput(item.Key, item.Value.keyCode);
                continue;
            }
            CheckScrollInput(item.Key, item.Value.scroll);
        }
    }

    private void CheckKeycodeInput(InputAction action, KeyCode keyCode) {
        if (!keyEventDictionary.ContainsKey(keyCode)) return;
        foreach (InputType inputType in keyEventDictionary[keyCode].Keys) {
            switch (inputType) {
                case InputType.GetKeyDown:
                    if (Input.GetKeyDown(keyCode))
                        keyEventDictionary[keyCode][inputType].Invoke(action, inputType, 0);
                    break;
                case InputType.GetKey:
                    if (Input.GetKey(keyCode))
                        keyEventDictionary[keyCode][inputType].Invoke(action, inputType, 0);
                    break;
                case InputType.GetKeyUp:
                    if (Input.GetKeyUp(keyCode))
                        keyEventDictionary[keyCode][inputType].Invoke(action, inputType, 0);
                    break;
            }
        }
    }

    private void CheckScrollInput(InputAction action, bool scroll) {
        InputType type = scroll ? InputType.ScrollUp : InputType.ScrollDown;
        if (!scrollEventDictionary.ContainsKey(type)) return;
        float delta = Input.mouseScrollDelta.y;
        if (type == InputType.ScrollUp && delta > 0) {
            scrollEventDictionary[type].Invoke(action, type, delta);

        }
        else if (type == InputType.ScrollDown && delta < 0) {
            scrollEventDictionary[type].Invoke(action, type, delta);
        }
    }

    static public void StartListeningInput(InputAction action, InputType type, Action<InputAction, InputType, float> listener) {
        InputCode input = keyBinding[action];
        if (input.keyCode != KeyCode.None) { //키코드 입력이 바인딩 된 경우
            KeyCode key = input.keyCode;
            if (!keyEventDictionary.ContainsKey(key)) {
                keyEventDictionary.Add(key, new Dictionary<InputType, Action<InputAction, InputType, float>>());
                if (!keyEventDictionary[key].ContainsKey(type)) {
                    keyEventDictionary[key].Add(type, listener);
                    return;
                }
            }
            keyEventDictionary[key][type] += listener;
            return;
        }
        if (input.scroll) { //스크롤 업 입력이 바인딩 된 경우
            if (type != InputType.ScrollUp) { // 키 바인딩과 입력 방식 동일성 검사
                Debug.LogError("You are trying to register ScrollEvent that different KeyBinding between Action");
                return;
            }
            if (!scrollEventDictionary.ContainsKey(type)) {
                scrollEventDictionary.Add(type, listener);
                return;
            }
            scrollEventDictionary[type] += listener;
        }
        else { //스크롤 다운 입력이 바인딩 된 경우
            if (type != InputType.ScrollDown) { // 키 바인딩과 입력 방식 동일성 검사
                Debug.LogError("You are trying to register ScrollEvent that different KeyBinding between Action");
                return;
            }
            if (!scrollEventDictionary.ContainsKey(type)) {
                scrollEventDictionary.Add(type, listener);
                return;
            }
            scrollEventDictionary[type] += listener;
        }
    }

    static public void StopListeningInput(InputAction action, InputType type, Action<InputAction, InputType, float> listener) {
        InputCode input = keyBinding[action];
        if (input.keyCode != KeyCode.None) { //키코드 입력이 바인딩 된 경우
            KeyCode key = input.keyCode;
            if (!keyEventDictionary.ContainsKey(key) || !keyEventDictionary[key].ContainsKey(type)) {
                Debug.LogWarning("You are trying to remove Action that does not existent");
                return;
            }
            keyEventDictionary[key][type] -= listener;
            return;
        }
        //이벤트 등록 과정에서 키 바인딩과 타입의 동일 여부를 이미 판단했기 때문에 등록 해제 과정에서는 해주지 않음
        if (input.scroll) { //스크롤 업 입력이 바인딩 된 경우
            if (!scrollEventDictionary.ContainsKey(type)) {
                Debug.LogWarning("You are trying to remove Action that does not existent");
                return;
            }
            scrollEventDictionary[type] -= listener;
        }
        else { //스크롤 다운 입력이 바인딩 된 경우
            if (!scrollEventDictionary.ContainsKey(type)) {
                Debug.LogWarning("You are trying to remove Action that does not existent");
                return;
            }
            scrollEventDictionary[type] -= listener;
        }
    }

    /// <returns>마우스 포인터의 레이가 충돌 감지 시 true 아니면 false</returns>
    static public bool GetMouseRayPos(Vector3 mousePos, out Vector3 hitPoint) {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane, Define.BOTTOM_LAYER)) {
            hitPoint = hit.point;
            return true;
        }
        hitPoint = Vector3.zero;
        return false;
    }
}