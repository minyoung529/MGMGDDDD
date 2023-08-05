using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{
    private bool isEntered = false;

    private void Enter(GameObject obj)
    {
        //collision.gameObject 가져와서 조용히 만들기
        isEntered = true;
        Debug.Log("ENTER");
    }

    private void Exit(GameObject obj)
    {
        //collision.gameObject 가져와서 원래 상태로 만들기
        isEntered = false;
        Debug.Log("EXIT");
    }

    public void Toggle(GameObject obj)
    {
        if (isEntered)
            Exit(obj);
        else
            Enter(obj);
    }
}
