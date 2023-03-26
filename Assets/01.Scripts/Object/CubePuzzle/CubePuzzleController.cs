using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private bool[] visited;

    void Start()
    {
        visited = new bool[cubeCount];

        for (int i = 0; i < cubeCount; i++)
        {
            cubePuzzles.Add(transform.GetChild(i).GetComponent<CubePuzzle>());
            cubePuzzles[i].ListeningOnSuccess(OnChangeConnect);
        }
    }

    private void OnChangeConnect(int v)
    {
        if (cubeCount == cubePuzzles.Count(x => x.IsSuccess))
        {
            SolvePuzzle();
        }
    }

    [ContextMenu("ButtonTest")]
    public void CheckSuccess()
    {
        if (successCnt == cubeCount)
        {
            SolvePuzzle();
        }
        else
        {
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
        for (int i = 0; i < cubeCount; i++)
        {
            visited[i] = false;
        }

        successCnt = solvedPuzzleCount;
        foreach (CubePuzzle item in cubePuzzles)
        {
            item.ResetPuzzle();
            item.Respawn();
        }
    }

    public bool IsVisited(int v)
    {
        return visited[v];
    }
}
