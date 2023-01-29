using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTest : MonoBehaviour
{
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        if(transform.position.y < -100f)
        {
            SavePoint();
        }
    }

    [ContextMenu("BACK")]
    private void SavePoint()
    {
        transform.position = originalPos;
        GetComponent<Rigidbody>().velocity= Vector3.zero;
    }
}
