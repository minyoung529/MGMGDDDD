using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialPuzzleCarSpawner : MonoBehaviour
{

    public void DriveCar(GameObject car)
    {
        car.transform.DOLocalMoveZ(10f, 24f);
        car.transform.SetParent(transform);
    }
}
