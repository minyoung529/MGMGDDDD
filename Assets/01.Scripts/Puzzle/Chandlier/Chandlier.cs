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
    [SerializeField] private List<ChandlierListner> groupTransforms;
    private int curIdx = 0;

    private ChandlierListner[] listners;

    [SerializeField]
    private UnityEvent onClearPuzzle;

    [Header("Rendering")]
    [SerializeField] private Transform rendererRoot;
    private ChangeEmission[] emissionChangers;

    private void Awake()
    {
        listners = fireBallRoot.GetComponentsInChildren<ChandlierListner>();
        emissionChangers = rendererRoot.GetComponentsInChildren<ChangeEmission>();

        foreach (ChandlierListner c in listners)
        {
            c.ListeningOnLighting(OnLighting);
        }
    }

    private void OnLighting(ChandlierListner listner)
    {
        emissionChangers[curIdx].Change();

        if (groupTransforms.Contains(listner))    // group
        {
            foreach (ChandlierListner element in groupTransforms)
            {
                if (!element.IsOilContact)
                    return;
            }
        }

        if (++curIdx >= maxLightCount)
        {
            onClearPuzzle?.Invoke();
        }
    }
}
