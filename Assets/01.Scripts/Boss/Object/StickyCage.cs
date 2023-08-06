using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCage : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cages;

    int curIdx = 0;

    PuzzleClearCounter counter;

    private void Awake()
    {
        counter = GetComponent<PuzzleClearCounter>();
    }

    [ContextMenu("Open")]
    public void OpenCages()
    {
        cages[curIdx++].gameObject.SetActive(false);
        counter.CheckClear();
    }

    [ContextMenu("Reset")]
    public void ResetCages()
    {
        counter.ResetCount();
        curIdx = 0;

        foreach(GameObject cage in cages)
        {
            cage.gameObject.SetActive(true);
        }
    }
}
