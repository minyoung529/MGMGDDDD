using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    #region 키 매핑
    private static string SAVE_PATH;
    private const string SAVE_FILE = "/KeyMapping";
    private static Dictionary<InputAction, InputInfo[]> keyBinding = new Dictionary<InputAction, InputInfo[]>();
    public static Dictionary<InputAction, InputInfo[]> KeyBinding => keyBinding;
    [SerializeField] private List<InputSave> inputSave = new List<InputSave>();
    #endregion

    #region 이벤트
    private static Dictionary<InputAction, Action<InputAction, float>> eventDictionary
        = new Dictionary<InputAction, Action<InputAction, float>>();
    #endregion

    protected override void Awake()
    {
        SAVE_PATH = string.Concat(Application.persistentDataPath, "/Control");

        if (Instance == this)
        {
            LoadKeyMapping();
        }
        base.Awake();
    }

    private void Update()
    {
        CheckInput();
    }

    private void OnApplicationQuit()
    {
        SaveKeyMapping(keyBinding);
    }

    private void LoadKeyMapping()
    {
        if (!File.Exists(string.Concat(SAVE_PATH, SAVE_FILE)))
        {
            string jsonSave = File.ReadAllText(string.Concat(SAVE_PATH, SAVE_FILE));
            inputSave = JsonConvert.DeserializeObject<List<InputSave>>(jsonSave);
            keyBinding.Clear();
        }
        foreach (InputSave save in inputSave)
        {
            keyBinding.Add(save.Action, save.Code);
        }
    }

    private void SaveKeyMapping(Dictionary<InputAction, InputInfo[]> keyDictionary)
    {
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        List<InputSave> save = new List<InputSave>();
        foreach (KeyValuePair<InputAction, InputInfo[]> pair in keyDictionary)
        {
            save.Add(new InputSave(pair.Key, pair.Value));
        }
        string jsonSave = JsonConvert.SerializeObject(save.ToArray(), Formatting.Indented);
        File.WriteAllText(string.Concat(SAVE_PATH, SAVE_FILE), jsonSave);
    }

    public void ChangeKeyMapping(InputAction action, InputInfo[] inputInfo)
    {
        keyBinding[action] = inputInfo;
        SaveKeyMapping(keyBinding);
    }

    private void CheckInput()
    {

        List<InputAction> keys = eventDictionary.Keys.ToList();

        for (int i = 0; i < keys.Count; i++)
        {
            InputAction action = keys[i];
            InputInfo[] infos = keyBinding[action];
            if (infos == null) continue;
            bool result = true;
            float param = 0;
            for (int keyIndex = 0; keyIndex < infos.Length; keyIndex++)
            {
                object value = CheckInputType(infos[keyIndex]);
                if (infos[keyIndex].inputType <= InputType.GetKeyUp)
                {
                    if (!(bool)value)
                    {
                        result = false;
                        break;
                    }
                }
                else if (infos[keyIndex].inputType <= InputType.ScrollDown)
                {
                    if ((float)value == 0)
                    {
                        result = false;
                        break;
                    }
                    param = (float)value;
                }
                else
                {
                    Debug.LogError("입력의 방식이 특정되지 않았습니다");
                    result = false;
                    break;
                }
            }
            if (result)
            {
                eventDictionary[action].Invoke(action, param);
            }
        }
    }

    private object CheckInputType(InputInfo inputInfo)
    {
        switch (inputInfo.inputType)
        {
            case InputType.GetKeyDown:
                return Input.GetKeyDown(inputInfo.keyCode);
            case InputType.GetKey:
                return Input.GetKey(inputInfo.keyCode);
            case InputType.GetKeyUp:
                return Input.GetKeyUp(inputInfo.keyCode);
            case InputType.ScrollUp:
                if (Input.mouseScrollDelta.y > 0)
                    return Input.mouseScrollDelta.y;
                return 0;
            case InputType.ScrollDown:
                if (Input.mouseScrollDelta.y < 0)
                    return Input.mouseScrollDelta.y;
                return 0;
            default:
                return 0;
        }
    }

    static public void StartListeningInput(InputAction action, Action<InputAction, float> listener)
    {
        if (eventDictionary.ContainsKey(action))
        {
            eventDictionary[action] += listener;
        }
        else
        {
            eventDictionary.Add(action, listener);
        }
    }

    static public void StopListeningInput(InputAction action, Action<InputAction, float> listener)
    {
        if (eventDictionary.ContainsKey(action))
        {
            eventDictionary[action] -= listener;
            if (eventDictionary[action] == null)
            {
                eventDictionary.Remove(action);
            }
        }
    }
}