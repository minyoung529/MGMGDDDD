using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TorchManager : MonoBehaviour
{
    // Clear Func
    public UnityEvent clearFunc;

    private int torchCnt = 0;
    private const int CHANGE_TORCH = 1;
    private const int MAX_TORCH = 5;
    public TorchPuzzle[] torchs = new TorchPuzzle[MAX_TORCH];

    // ���� 
    private int[,] puzzle =
        new int[MAX_TORCH, 2]
    {
        {0, 4},
        {1, 1},
        {2, 3},
        {3, 3},
        {4, 4},
    };
    // ???? ???? ???
    private bool isClear = false;
    private bool[] puzzleClear = new bool[MAX_TORCH] { false, false, false, false, false };

    private void Start()
    {
        torchs = transform.GetComponentsInChildren<TorchPuzzle>();
        ResetPuzzle();
    }

    private void ResetPuzzle()
    {
        isClear = false;
        torchCnt = 0;
        for (int i = 0; i < MAX_TORCH; i++)
        {
            puzzleClear[i] = false;
            torchs[i].OffLight();
        }
    }

    public void LightOn(int index)
    {
        for (int i = 0; i < CHANGE_TORCH; i++)
        {
            torchs[puzzle[index, i]].Lighting();

            puzzleClear[puzzle[index, i]] = !puzzleClear[puzzle[index, i]];
            if (puzzleClear[puzzle[index, i]]) OnLight();
            else OffLight();

        }
    }

    public void OnLight()
    {
        torchCnt++;
        if (torchCnt >= MAX_TORCH)
        {
            ClearPuzzle();
        }
    }
    public void OffLight()
    {
        torchCnt--;
        if (torchCnt >= MAX_TORCH)
        {
            ClearPuzzle();
        }
    }

    private void ClearPuzzle()
    {
        isClear = true;
        clearFunc.Invoke();
    }

    public void LeverCheck()
    {
        if (!isClear) return;
    }

    public void OpenDoor(GameObject obj)
    {
        Debug.Log("Clear");
        obj.SetActive(false);
    }
}
