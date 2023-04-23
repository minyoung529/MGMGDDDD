using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRail : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    float timer;
    [SerializeField]
    Spline spline;

    private Transform railPosition;

    private void Awake()
    {
        railPosition = transform.Find("RailPosition");
    }

    private void Update()
    {
        transform.position = spline.GetLocationByTime(timer) - railPosition.localPosition;
    }
}
