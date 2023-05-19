using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Chandlier : MonoBehaviour
{
    [SerializeField]
    private Transform fireBallRoot;

    [Header("Puzzle Setting")]
    [SerializeField] private int maxLightCount = 8;
    private int curLightCount = 0;
    [SerializeField] private List<ChandlierGroup> groups;

    private ChandlierListner[] listners;

    [SerializeField]
    private UnityEvent onClearPuzzle;

    [Header("Rendering")]
    [SerializeField] private Transform rendererRoot;

    private ChandlierLight[] lights;

    private const short FAIL_CODE = -999;

    private void Awake()
    {
        listners = fireBallRoot.GetComponentsInChildren<ChandlierListner>();
        lights = transform.GetComponentsInChildren<ChandlierLight>();

        foreach (ChandlierListner c in listners)
        {
            c.ListeningOnLighting(OnLighting);
        }

        groups[0].Group.ForEach(x => x.TouchedTime = 2f); // Oil Group
        groups[1].Group.ForEach(x => x.TouchedTime = 1f); // Fire Group
    }

    private int CheckGroup(ChandlierListner listner)
    {
        for (int i = 0; i < groups.Count; i++)
        {
            if (groups[i].Group.Contains(listner))    // group
            {
                foreach (ChandlierListner element in groups[i].Group)
                {
                    if (!element.IsTouched)
                    {
                        listner.BlockLighting();
                        groups[i].Group.ForEach(x => x.StopFire());
                        return FAIL_CODE;
                    }
                }

                groups[i].Group.ForEach(x => x.Fire());
                return i;
            }
        }
        return 0;
    }

    private void OnLighting(ChandlierListner listner)
    {
        if (listner.IsSuccess) return;

        // 그룹인지 체크
        int check = CheckGroup(listner);

        if (check == FAIL_CODE) return;

        lights[listner.LightIndex].Lighting();
        listner.Fire();

        if(++curLightCount == maxLightCount)
        {
            onClearPuzzle?.Invoke();
        }
    }
}

[System.Serializable]
public class ChandlierGroup
{
    [SerializeField]
    private List<ChandlierListner> list;

    public List<ChandlierListner> Group => list;

    public ChandlierListner this[int index]
    {
        get => list[index];
    }
}