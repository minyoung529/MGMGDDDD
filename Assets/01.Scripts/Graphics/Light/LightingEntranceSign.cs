using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChangeEmissionGroup
{
    [SerializeField]
    private List<ChangeEmission> group;

    public ChangeEmission this[int index]
    {
        get => group[index];
        set => group[index] = value;
    }

    public ChangeEmissionGroup()
    {
        group = new List<ChangeEmission>();
    }

    public void Add(ChangeEmission emission)
    {
        group.Add(emission);
    }

    public void Change()
    {
        group.ForEach(x => x.Change());
    }

    public void Back()
    {
        group.ForEach(x => x.BackToOriginalColor());
    }
}

public class LightingEntranceSign : MonoBehaviour
{
    [SerializeField]
    private bool playOnAwake = false;

    [SerializeField]
    private float changeInterval = 1f;

    [SerializeField]
    private int groupCount = 2;
    private int curIdx = 0;

    private List<ChangeEmissionGroup> changeGroups = new List<ChangeEmissionGroup>();

    private void Awake()
    {

        for (int i = 0; i < groupCount; i++)
        {
            changeGroups.Add(new ChangeEmissionGroup());
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            ChangeEmission changeEmission = transform.GetChild(i).GetComponent<ChangeEmission>();

            int curIdx = i % groupCount;
            changeGroups[curIdx].Add(changeEmission);
        }
    }

    private void Start()
    {
        if (playOnAwake)
            StartLighting();
    }

    void StartLighting()
    {
        StartCoroutine(LoopLighting());
    }

    public void StopLighting()
    {
        StopAllCoroutines();
    }

    private IEnumerator LoopLighting()
    {
        bool isEvenActive = true;

        while (true)
        {
            OnAndOffLightings((curIdx++) % groupCount);
            yield return new WaitForSeconds(changeInterval);
            isEvenActive = !isEvenActive;
        }
    }

    private void OnAndOffLightings(int curIdx)
    {
        int prev = (curIdx + groupCount - 1) % groupCount;

        changeGroups[prev].Back();
        changeGroups[curIdx].Change();
    }
}
