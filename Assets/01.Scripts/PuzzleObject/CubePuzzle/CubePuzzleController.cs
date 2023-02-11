using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePuzzleController : MonoBehaviour
{
    [SerializeField]
    private int cubeCount = 9;
    private int successCnt = 0;
    [SerializeField]
    private int solvedPuzzleCount = 0;

    List<CubePuzzle> cubePuzzles = new List<CubePuzzle>();

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

    }

    public void ResetPuzzle()
    {
        successCnt = solvedPuzzleCount;
    }
}
