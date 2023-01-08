using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자 Input을 담당하는 함수
/// </summary>
public class InputManager
{
    // 행동 - 키코드가 연결된 딕셔너리
    private static Dictionary<InputAction, KeyCode> keyDict = new Dictionary<InputAction, KeyCode>();

    // 로드용 클래스
    public class InputKey
    {
        public InputAction inputAction;
        public KeyCode keycode;
    }

    public void OnStart()
    {
        // 로드 & 딕셔너리에 추가
        //List<InputKey> inputs = GameManager.Instance.SpreadData.GetDatas<InputKey>();

        //foreach (var pair in inputs)
        //{
        //    keyDict.Add(pair.inputAction, pair.keycode);
        //}
    }

    public void Update()
    {
        for (int i = 0; i < (int)InputAction.Length; i++)
        {
            InputAction action = (InputAction)i;

            // string => Enum Parse
            //if (GetKeyDown(action))
            //{
            //    EventManager<InputType>.TriggerEvent(i, InputType.GetKeyDown);
            //    EventManager<InputType, InputAction>.TriggerEvent(i, InputType.GetKeyDown, action);
            //}
            //else if (GetKey(action))
            //{
            //    EventManager<InputType>.TriggerEvent(i, InputType.Getkey);
            //    EventManager<InputType, InputAction>.TriggerEvent(i, InputType.Getkey, action);
            //}
            //else if (GetKeyUp(action))
            //{
            //    EventManager<InputType>.TriggerEvent(i, InputType.GetKeyUp);
            //    EventManager<InputType, InputAction>.TriggerEvent(i, InputType.GetKeyUp, action);
            //}

            // Else Axis => 
        }
    }

    // 키를 누르는 중
    public static bool GetKey(InputAction inputAction)
    {
        if (!keyDict.ContainsKey(inputAction))
            return false;

        return Input.GetKey(keyDict[inputAction]);
    }

    // 키를 눌렀을 때
    public static bool GetKeyDown(InputAction inputAction)
    {
        if (!keyDict.ContainsKey(inputAction))
            return false;

        return Input.GetKeyDown(keyDict[inputAction]);
    }

    // 키를 눌렀다 뗐을 때
    public static bool GetKeyUp(InputAction inputAction)
    {
        if (!keyDict.ContainsKey(inputAction))
            return false;

        return Input.GetKeyUp(keyDict[inputAction]);
    }

    // 행동에 연결되는 key를 바꿔주는 함수
    public static void ChangeKey(InputAction inputAction, KeyCode key)
    {
        keyDict[inputAction] = key;
    }
}
