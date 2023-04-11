using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialTrain : MonoBehaviour
{
    [SerializeField] float driveTime = 3.0f;
    [SerializeField] float warningTime = 2.0f;
    [SerializeField] float minDriveCoolTime = 3.0f;
    [SerializeField] float maxDriveCoolTime = 5.0f;

    private Vector3 startPos = new Vector3(0.015f, 1f, 0.25f);
    private float endPosZ = 0.7f;

    private void Start()
    {
        StartCoroutine(Drive());
    }

    private void DriveTrain()
    {
        transform.localPosition = startPos;
        transform.DOLocalMoveZ(endPosZ, driveTime).OnComplete(() =>
        {
            StartCoroutine(Drive());
        });
    }

    IEnumerator Drive()
    {
        float randomCoolTime = Random.Range(minDriveCoolTime, maxDriveCoolTime);
        yield return new WaitForSeconds(randomCoolTime);
        StartCoroutine(Warning());
    }
    
    IEnumerator Warning()
    {
        // Sound & Light
        yield return new WaitForSeconds(warningTime);
        DriveTrain();
    }
}
