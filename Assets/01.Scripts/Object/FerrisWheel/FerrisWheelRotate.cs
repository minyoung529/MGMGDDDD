using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelRotate : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    
    private float eulerX = 0f;

    void Update()
    {
        eulerX += speed * Time.deltaTime;
        Vector3 euler = new Vector3(0, 0f, eulerX);

        transform.localRotation = Quaternion.Euler(euler);
    }
}
