using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidingObject : MonoBehaviour
{
    private Rigidbody rigid;
    private OilRoot oilRoot;

    private int enterCount = 0;
    private int slidingCount = 0;

    private bool canSliding = true;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG) && enabled)
        {
            enterCount++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG) && enabled)
        {
            if (oilRoot == null)
            {
                oilRoot = other.GetComponentInParent<OilRoot>();
                oilRoot.OnDryOil += CanSlidingActive;
            }

            if (oilRoot.Complete)
            {
                Sliding();
            }
        }
    }

    public void Sliding()
    {
        if (slidingCount == enterCount) return;
        if (!canSliding) return;

        Vector3[] points = oilRoot.Points;
        Vector3[] wayPoints;
        int closeIdx = GetCloseIdx(points);
        int pointLength = points.Length;

        // closeIdx ~ Length
        if (closeIdx < pointLength - closeIdx)
        {
            wayPoints = SplitArray(points, closeIdx, pointLength - closeIdx);
        }
        else // closeIdx -> 0
        {
            wayPoints = SplitArray(points, 0, closeIdx + 1);
            Array.Reverse(wayPoints);
        }

        canSliding = false;
        rigid.DOPath(wayPoints, wayPoints.Length * 0.3f, PathType.CatmullRom, PathMode.Full3D, 10, Color.red).SetEase(Ease.OutCirc);

        slidingCount = enterCount;
    }

    public static T[] SplitArray<T>(T[] array, int startIndex, int length)
    {
        T[] result = new T[length];
        Array.Copy(array, startIndex, result, 0, length);
        return result;
    }

    public void CanSlidingActive()
    {
        canSliding = true;
    }

    private int GetCloseIdx(Vector3[] points)
    {
        float min = (points[0] - transform.position).sqrMagnitude;
        int minIdx = 0;

        for (int i = 1; i < points.Length; i++)
        {
            float sqrMagnitude = (points[i] - transform.position).sqrMagnitude;

            if (sqrMagnitude < min)
            {
                minIdx = i;
                min = sqrMagnitude;
            }
        }

        return minIdx;
    }

    void OnDestroy()
    {
        if (oilRoot)
        {
            oilRoot.OnDryOil -= CanSlidingActive;
        }
    }
}
