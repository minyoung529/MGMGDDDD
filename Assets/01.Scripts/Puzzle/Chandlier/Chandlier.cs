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
    [SerializeField] private List<ChandlierListner> oilGroupTransforms;
    [SerializeField] private List<ChandlierListner> groupTransforms;
    private int curIdx = 0;

    private ChandlierListner[] listners;

    [SerializeField]
    private UnityEvent onClearPuzzle;

    [Header("Rendering")]
    [SerializeField] private Transform rendererRoot;

    private ChandlierLight[] lights;

    private void Awake()
    {
        listners = fireBallRoot.GetComponentsInChildren<ChandlierListner>();
        lights = transform.GetComponentsInChildren<ChandlierLight>();

        foreach (ChandlierListner c in listners)
        {
            c.ListeningOnLighting(OnLighting);
        }

        oilGroupTransforms.ForEach(x => x.TouchedTime = 2f);
        groupTransforms.ForEach(x => x.TouchedTime = 1f);
    }

    private int CheckOilGroup(ChandlierListner listner)
    {
        if (oilGroupTransforms.Contains(listner))    // group
        {
            foreach (ChandlierListner element in oilGroupTransforms)
            {
                if (!element.IsOilContact || !element.IsTouched)
                {
                    Debug.Log(element.name + " : " + element.IsOilContact + ", " + element.IsTouched);
                    listner.BlockLighting();
                    oilGroupTransforms.ForEach(x => x.StopFire());
                    return -1;
                }
            }

            oilGroupTransforms.ForEach(x => x.Fire());
            return oilGroupTransforms.Count;
        }

        return 0;
    }

    private int CheckGroup(ChandlierListner listner)
    {
        if (groupTransforms.Contains(listner))    // group
        {
            foreach (ChandlierListner element in groupTransforms)
            {
                if (!element.IsTouched)
                {
                    listner.BlockLighting();
                    groupTransforms.ForEach(x => x.StopFire());
                    return -1;
                }
            }

            groupTransforms.ForEach(x => x.Fire());
            return groupTransforms.Count;
        }

        return 0;
    }

    private void OnLighting(ChandlierListner listner)
    {
        if (listner.IsSuccess) return;

        int check = CheckOilGroup(listner);

        if (check == 0) // 그룹에 안 되어있으면 다시 검사
            check = CheckGroup(listner);

        if (check == 0) // 그룹에 안 되어있으면 다시 검사
        {
            check = 1;
            listner.Fire();
            Debug.Log("FIRE");
        }

        if (check == -1) return;

        for (int i = 0; i < check; i++)
        {
            lights[curIdx].Lighting();

            if (++curIdx >= maxLightCount)
            {
                onClearPuzzle?.Invoke();
            }
        }
    }
}
