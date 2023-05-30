using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetListenerTransform : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance.MainCam)
        {
            transform.rotation = GameManager.Instance.MainCam.transform.rotation;
        }
    }
}
