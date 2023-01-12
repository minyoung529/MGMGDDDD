using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveInput : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField]
    private float speed = 3f;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        rigid.velocity = transform.TransformDirection(new Vector3(x, rigid.velocity.y, z) * speed);
    }
}
