using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Funnel : MonoBehaviour
{
    [Header("Puzzle Setting")]
    [SerializeField]
    private int boltCount = 0;
    private int curBoltCount = 0;

    [SerializeField]
    private Transform boltRoot;
    private Bolt[] bolts;

    [Header("Clear")]
    [SerializeField]
    private UnityEvent onClear;

    private void Awake()
    {
        bolts = boltRoot.GetComponentsInChildren<Bolt>();

        foreach (Bolt bolt in bolts)
        {
            bolt.ListeningOnInserted(OnInsertBolt);
        }
    }

    private void OnInsertBolt()
    {
        curBoltCount++;

        if (curBoltCount == boltCount)
        {
            onClear?.Invoke();
        }
    }
}
