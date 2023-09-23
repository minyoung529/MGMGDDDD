using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelRoom : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, -48f);
    }
}
