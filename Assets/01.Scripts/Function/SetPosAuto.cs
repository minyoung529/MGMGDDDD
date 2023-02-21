using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosAuto : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float distance;
    private bool isSet = false;

    private void Update() {
        if (isSet) return;
        if ((targetTransform.position - transform.position).sqrMagnitude <= Mathf.Pow(distance, 2)) {
            StartCoroutine(Move());
            isSet = true;
        }
    }

    private IEnumerator Move() {
        while((targetTransform.position - transform.position).sqrMagnitude <= 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, 1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetTransform.rotation, 1f);
            yield return null;
        }
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
} 
