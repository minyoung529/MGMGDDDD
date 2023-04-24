using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDayAndNight : MonoBehaviour
{
    public ChangeBloom[] postProcessings;
    public ChangeRenderSettings[] renderSettings;
    public GameObject[] activeList;

    [SerializeField]
    private bool onAwake = false;

    [SerializeField]
    private int testIdx = 0;

    private void Awake()
    {
        DialPuzzleController dialPuzzle = FindObjectOfType<DialPuzzleController>();
        if (dialPuzzle != null)
        {
            dialPuzzle.OnTimeChange += (TimeType type) => Change((int)type);
        }
    }

    private void Start()
    {
        if (onAwake)
        {
            Change(0);
        }
    }

    public void Change(int index)
    {
        //if (renderSettings.Length > 0)
        //    renderSettings[index]?.Change();

        if (postProcessings.Length > 0)
            postProcessings[index]?.Change();

        if (activeList.Length > 0)
        {
            AllInactive();

            if (activeList[index] != null)
                activeList[index]?.SetActive(true);
        }
    }

    private void AllInactive()
    {
        foreach (var item in activeList)
        {
            if (item != null)
                item.SetActive(false);
        }
    }

    [ContextMenu("TEST CHANGE")]
    public void TestChange()
    {
        Change(testIdx);
    }
}
