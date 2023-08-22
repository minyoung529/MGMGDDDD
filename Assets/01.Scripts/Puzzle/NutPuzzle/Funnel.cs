using System.Linq;
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
    private List<Bolt> bolts;

    [Header("Clear")]
    [SerializeField]
    private UnityEvent onEquip;
    [SerializeField]
    private UnityEvent onClear;

    private bool isClear = false;

    private void Awake()
    {
        bolts = boltRoot?.GetComponentsInChildren<Bolt>().ToList();

        if (bolts == null || bolts.Count == 0)
        {
            bolts = new List<Bolt>();

            for (int i = 0; i < boltRoot.childCount; i++)
            {
                bolts.Add(boltRoot.GetChild(i).GetComponent<Bolt>());
            }
        }

        foreach (Bolt bolt in bolts)
        {
            bolt.ListeningOnInserted(OnInsertBolt);
        }
    }

    private void OnInsertBolt()
    {
        curBoltCount++;
        onEquip?.Invoke();

        if (curBoltCount == boltCount)
        {
            if (isClear) return;
            isClear = true;
            onClear?.Invoke();
        }
    }
}
