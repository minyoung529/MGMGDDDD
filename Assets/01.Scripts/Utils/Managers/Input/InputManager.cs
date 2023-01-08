using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� Input�� ����ϴ� �Լ�
/// </summary>
public class InputManager
{
    // �ൿ - Ű�ڵ尡 ����� ��ųʸ�
    private static Dictionary<InputAction, KeyCode> keyDict = new Dictionary<InputAction, KeyCode>();

    // �ε�� Ŭ����
    public class InputKey
    {
        public InputAction inputAction;
        public KeyCode keycode;
    }

    public void OnStart()
    {
        // �ε� & ��ųʸ��� �߰�
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

    // Ű�� ������ ��
    public static bool GetKey(InputAction inputAction)
    {
        if (!keyDict.ContainsKey(inputAction))
            return false;

        return Input.GetKey(keyDict[inputAction]);
    }

    // Ű�� ������ ��
    public static bool GetKeyDown(InputAction inputAction)
    {
        if (!keyDict.ContainsKey(inputAction))
            return false;

        return Input.GetKeyDown(keyDict[inputAction]);
    }

    // Ű�� ������ ���� ��
    public static bool GetKeyUp(InputAction inputAction)
    {
        if (!keyDict.ContainsKey(inputAction))
            return false;

        return Input.GetKeyUp(keyDict[inputAction]);
    }

    // �ൿ�� ����Ǵ� key�� �ٲ��ִ� �Լ�
    public static void ChangeKey(InputAction inputAction, KeyCode key)
    {
        keyDict[inputAction] = key;
    }
}
