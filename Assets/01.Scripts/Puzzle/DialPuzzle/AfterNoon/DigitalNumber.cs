using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalNumber : MonoBehaviour
{
    [System.Serializable]
    public class IntList
    {
        public List<int> list;
    }

    public IntList[] activeIndexes = new IntList[10];

    public int recordNumber = 0;

    private Transform[] childs;

    private void Awake()
    {
        childs = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i);
        }
    }

    public void SetNumber(int number)
    {
        if (number > 10) return;
        ActiveNumbers(number);
    }

    private void ActiveNumbers(int number)
    {
        foreach (Transform child in childs)
            child.gameObject.SetActive(false);

        foreach (int i in activeIndexes[number].list)
        {
            childs[i].gameObject.SetActive(true);
        }
    }

    [ContextMenu("Record Number Shape")]
    public void RecordNumber()
    {
        activeIndexes[recordNumber].list = new List<int>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                activeIndexes[recordNumber].list.Add(i);
            }
        }
    }
}
