using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRope : MonoBehaviour
{
     private DialPuzzleController dialController;
    private float length => dialController.RemainTime;

    private void Awake()
    {
        dialController = GetComponent<DialPuzzleController>();
    }

    // 시간이 다 됐을 때 맨 아래
    // 길이 나누기 시간으로 ++
}
