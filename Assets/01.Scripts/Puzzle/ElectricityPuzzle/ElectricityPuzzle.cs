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

    [SerializeField]
    private List<ElectricityEffect> electricityEffects;

    private bool[] isVisited;

    private int curIdx = 0;

    private int MaxCount => checkPoints.Length;

    private bool isTesting = false;

    [SerializeField]
    private UnityEvent onClear;

    [SerializeField]
    private UnityEvent onFail;

    [SerializeField]
    private UnityEvent onStart;


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
    public void StartElectricityFollow()
    {
        if (isTesting) return;

        onStart?.Invoke();
        isTesting = true;
        electricityEffects[0].Play();
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

        onFail?.Invoke();
        pathFollwers.ForEach(x => x.Stop());
        electricityEffects.ForEach(x => x.Fail());
    }

    private void Success()
    {
        if (++curIdx == MaxCount)
        {
            // 클리어
            Debug.Log("CLEAR");
            electricityEffects.ForEach(x => x.Success());
            onClear?.Invoke();
        }

        Split();
    }

    private void Split()
    {
        if (curIdx == MaxCount - 1)
        {
            electricityEffects[1].Play();
            pathFollwers[1].Depart();
        }
    }

    public void CheckVisited(int index)
    {
        isVisited[index] = true;
    }
}
