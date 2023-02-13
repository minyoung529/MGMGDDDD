using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubePuzzleController : MonoBehaviour
{
    [SerializeField]
    private int cubeCount = 9;
    private int successCnt = 0;
    [SerializeField]
    private int solvedPuzzleCount = 0;

    List<CubePuzzle> cubePuzzles = new List<CubePuzzle>();

    [SerializeField]
    private UnityEvent OnSolvePuzzle;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cubePuzzles.Add(transform.GetChild(i).GetComponent<CubePuzzle>());
            cubePuzzles[i].ListeningOnSuccess(OnSuccess);
        }
    }

    private void OnSuccess()
    {
        if (++successCnt == cubeCount)
        {
            SolvePuzzle();
        }
    }

    private void SolvePuzzle()
    {
        OnSolvePuzzle?.Invoke();
    }

    public void ResetPuzzle()
    {
        successCnt = solvedPuzzleCount;
    }
}
