using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFromTo : MonoBehaviour
{
    [SerializeField] private Transform to;
    [SerializeField] private Transform from;

    void Update()
    {
        // POS
        Vector3 mid = (to.position + from.position) * 0.5f;
        transform.position = mid;

        // SCALE
        Vector3 scale = transform.localScale;
        scale.y = Vector3.Distance(to.position, from.position) * 0.5f;
        transform.localScale = scale;

        // ROT
        transform.up = (to.position - from.position).normalized;
    }
}
