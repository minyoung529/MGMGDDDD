using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    private DetectObject detectObj;

    private void Awake()
    {
        detectObj= GetComponent<DetectObject>();
    }

}
