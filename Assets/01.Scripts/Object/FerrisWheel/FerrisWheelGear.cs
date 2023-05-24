using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelGear : MonoBehaviour
{
    private Quaternion originalEulerAngles;
    [Range(0f, 1f)]
    [SerializeField]
    private float t = 0.5f;

    private void Start()
    {
        originalEulerAngles = transform.rotation;
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, originalEulerAngles, t);
    }
}