using DG.Tweening;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolatedCubePuzzle : MonoBehaviour
{
    [SerializeField]
    private ButtonObject[] buttonObjects;

    [SerializeField]
    private PathCreator pathCreator;

    private bool isActiveAll = false;

    [SerializeField]
    private float followDuration = 2f;
    private float curRatio = 0f;
    private bool isStartFollow = false;

    [SerializeField]
    private Transform cubeTransform;
    private Quaternion originalRotation;

    private void Awake()
    {
        foreach (var obj in buttonObjects)
        {
            obj.ListeningOnPress(SetIsAllButtonActive);
        }

        originalRotation = cubeTransform.rotation;
    }

    private void Update()
    {
        if (!isStartFollow) return;

        curRatio += Time.deltaTime / followDuration;

        if (curRatio > 1f)
        {
            OnEnd();
        }
        else
        {
            cubeTransform.position = pathCreator.path.GetPointAtDistance(curRatio * pathCreator.path.length);
            cubeTransform.rotation = pathCreator.path.GetRotationAtDistance(curRatio * pathCreator.path.length);
        }
    }

    private void SetIsAllButtonActive()
    {
        if (isActiveAll) return;

        foreach (var obj in buttonObjects)
        {
            if (!obj.IsButtonOn)
                return;
        }

        isActiveAll = true;
        StartFollow();
    }

    [ContextMenu("Start Follow")]
    public void StartFollow()
    {
        isStartFollow = true;
    }

    private void OnEnd()
    {
        isStartFollow = false;
            cubeTransform.GetComponent<Rigidbody>().freezeRotation = true;

        cubeTransform.localEulerAngles = Vector3.up * 90f; 
    }
}
