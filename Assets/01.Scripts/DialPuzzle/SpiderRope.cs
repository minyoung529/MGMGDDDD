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

    // �ð��� �� ���� �� �� �Ʒ�
    // ���� ������ �ð����� ++
}
