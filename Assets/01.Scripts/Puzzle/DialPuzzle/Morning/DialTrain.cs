using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialTrain : MonoBehaviour
{
    [Header("TrafficLight")]
    [SerializeField] Light redLight;
    [SerializeField] Light greenLight;

    [Header("Value")]
    [SerializeField] float driveTime = 3.0f;
    [SerializeField] float warningTime = 2.0f;
    [SerializeField] float minDriveCoolTime = 3.0f;
    [SerializeField] float maxDriveCoolTime = 5.0f;

    private Vector3 startPos = new Vector3(0.015f, 1f, 0.25f);
    private float endPosZ = 0.7f;
    private bool stop = false;

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
        SwitchWarning(true);
        yield return new WaitForSeconds(randomCoolTime);
        StartCoroutine(Warning());
    }
    
    IEnumerator Warning()
    {
        // Sound
        yield return new WaitForSeconds(warningTime);
        SwitchWarning(false);
        DriveTrain();
    }

    private void SwitchWarning()
    {
        stop = !stop;
        if(stop)
        {
            redLight.gameObject.SetActive(true);
            greenLight.gameObject.SetActive(false);
        }
        else
        {
            redLight.gameObject.SetActive(false);
            greenLight.gameObject.SetActive(true);
        }
    }
    private void SwitchWarning(bool _stop)
    {
        stop = _stop;
        if(stop)
        {
            redLight.gameObject.SetActive(true);
            greenLight.gameObject.SetActive(false);
        }
        else
        {
            redLight.gameObject.SetActive(false);
            greenLight.gameObject.SetActive(true);
        }
    }
}
