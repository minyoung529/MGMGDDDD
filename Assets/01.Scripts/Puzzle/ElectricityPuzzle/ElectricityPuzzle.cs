using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder;

public class ElectricityPuzzle : MonoBehaviour
{
    [SerializeField]
    private Transform[] checkPoints;

    [SerializeField]
    private List<PathFollower> pathFollwers;

    private bool[] isVisited;

    private int curIdx = 0;

    private int MaxCount => checkPoints.Length;

    private bool isTesting = false;

    [SerializeField]
    private UnityEvent onClear;

    private void Start()
    {
        isVisited = new bool[checkPoints.Length];
        isVisited[isVisited.Length - 1] = true;
    }

    private void Update()
    {
        if (isTesting)
        {
            OnUpdateElectricity();
        }
    }

    [ContextMenu("Start!")]
    private void StartElectricityFollow()
    {
        if (isTesting) return;

        isTesting = true;
        pathFollwers[0].gameObject.SetActive(true);
        pathFollwers[0].Depart();
    }

    private void OnUpdateElectricity()
    {
        if (curIdx >= checkPoints.Length) return;

        if (Vector3.Distance(pathFollwers[0].transform.position, checkPoints[curIdx].position) < 1f)
        {
            if (isVisited[curIdx])
            {
                Success();
            }
            else
            {
                Fail();
            }
        }
    }

    private void Fail()
    {
        // 초기화 
        curIdx = 0;
        isTesting = false;

        pathFollwers.ForEach(x => x.gameObject.SetActive(false));
    }

    private void Success()
    {
        if (++curIdx == MaxCount)
        {
            // 클리어
            Debug.Log("CLEAR");
            onClear?.Invoke();
        }

        Split();
    }

    private void Split()
    {
        if (curIdx == MaxCount - 1)
        {
            // 하드코딩 죄송합니다.
            pathFollwers[1].Depart();
        }
    }

    public void CheckVisited(int index)
    {
        isVisited[index] = true;
    }
}
