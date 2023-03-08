using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetPosAuto : MonoBehaviour
{
    [SerializeField] private List<Transform> targetTransform;
    [SerializeField] private float distance = 1f;
    [SerializeField] private float moveTime = 1f;
    [SerializeField] private CubePuzzleController puzzleController;
    private bool isSet = false;

    private void Update()
    {
        if (isSet) return;
        float min = 0;
        Transform target = null;
        for (int i = 0; i < targetTransform.Count; i++)
        {
            if (puzzleController.IsVisited(i)) return;
            float distance = (targetTransform[i].position - transform.position).sqrMagnitude;
            if (distance <= Mathf.Pow(this.distance, 2))
            {
                if (min == 0 || distance < min)
                {
                    target = targetTransform[i];
                }
            }
        }
        if (!target) return;
        transform.DOMove(target.position, moveTime);
        transform.DORotate(target.rotation.eulerAngles, moveTime);
        isSet = true;
    }

    public void Reset()
    {
        isSet = false;
    }
}
