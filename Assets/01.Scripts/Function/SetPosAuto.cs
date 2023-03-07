using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosAuto : MonoBehaviour
{
    [SerializeField] private List<Transform> targetTransform;
    [SerializeField] private float distance = 1f;
    [SerializeField] private float moveSpeed = 1f;
    private bool isSet = false;

    private void Update() {
        if (isSet) return;
        foreach (Transform item in targetTransform)
        {
            if ((item.position - transform.position).sqrMagnitude <= Mathf.Pow(distance, 2))
            {
                StartCoroutine(Move(item));
                isSet = true;
            }
        }
    }

    private IEnumerator Move(Transform pos) {
        while((pos.position - transform.position).sqrMagnitude <= 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position, pos.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, pos.rotation, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = pos.position;
        transform.rotation = pos.rotation;
    }

    public void Reset()
    {
        isSet = false;
    }
} 
