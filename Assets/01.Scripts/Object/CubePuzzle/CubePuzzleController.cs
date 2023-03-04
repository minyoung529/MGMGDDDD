using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubePuzzleController : MonoBehaviour
{
    [SerializeField]
    private int cubeCount = 9;
    [SerializeField]
    private int successCnt = 0;
    [SerializeField]
    private int solvedPuzzleCount = 0;

    List<CubePuzzle> cubePuzzles = new List<CubePuzzle>();

    [SerializeField]
    private UnityEvent<int> OnSolvePuzzle;

    [SerializeField]
    private UnityEvent<int> OnPressButton;

    void Start()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            cubePuzzles.Add(transform.GetChild(i).GetComponent<CubePuzzle>());
            cubePuzzles[i].ListeningOnSuccess(OnSuccess);
        }
    }

    private void OnSuccess()
    {
        successCnt++;
    }

    [ContextMenu("ButtonTest")]
    public void CheckSuccess() {
        if(successCnt == cubeCount) {
            SolvePuzzle();
        }
        else {
            OnPressButton?.Invoke(successCnt);
            ResetPuzzle();
        }
    }

    [ContextMenu("SolvePuzzle")]
    private void SolvePuzzle()
    {
        OnSolvePuzzle?.Invoke(successCnt);
    }

    public void ResetPuzzle()
    {
        successCnt = solvedPuzzleCount;
        foreach(CubePuzzle item in cubePuzzles) {
            item.Respawn();
        }
    }
}
