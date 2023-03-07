using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetPosAuto : MonoBehaviour
{
    [SerializeField] private List<Transform> targetTransform;
    [SerializeField] private float distance = 1f;
    [SerializeField] private float moveTime = 1f;
    private bool isSet = false;

    private void Update() {
        if (isSet) return;
        float min = 0;
        Transform target = null;
        foreach (Transform item in targetTransform) {
            float distance = (item.position - transform.position).sqrMagnitude;
            if (distance <= Mathf.Pow(this.distance, 2)) {
                if (min == 0 || distance < min)
                    target = item;
            }
        }
        if (!target) return;
        transform.DOMove(target.position, moveTime);
        transform.DORotate(target.rotation.eulerAngles, moveTime);
        isSet = true;
    }

    public void Reset() {
        isSet = false;
    }
} 
