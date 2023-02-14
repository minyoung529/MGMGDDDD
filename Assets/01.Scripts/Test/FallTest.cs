using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTest : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;

        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < -100f)
        {
            SavePoint();
        }
    }

    public void SavePoint()
    {
        transform.position = originalPos;
        transform.rotation = originalRot;
        
        if(rigid)
        {
            rigid.velocity = Vector3.zero;
        }
    }
}
