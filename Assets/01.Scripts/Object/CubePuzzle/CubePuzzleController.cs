using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CubePuzzleController : MonoBehaviour
{
    [SerializeField]
    private int cubeCount = 9;
    private int SuccessCnt
    {
        get
        {
            return cubePuzzles.Count(x => x.IsSuccess);
        }
    }
        
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
        if (cubeCount == SuccessCnt)
        {
            SolvePuzzle();
        }
    }

    [ContextMenu("SolvePuzzle")]
    private void SolvePuzzle()
    {
        OnSolvePuzzle?.Invoke(SuccessCnt);
    }

    public bool IsVisited(int v)
    {
        return visited[v];
    }
}
