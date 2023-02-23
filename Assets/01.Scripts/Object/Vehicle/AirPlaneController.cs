using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController : MonoBehaviour
{
    [SerializeField] private Transform[] airPlanes;

    public void ChangePlanePos(Destination destination)
    {
        Debug.Log("sdfsdfs");

        int idx = (int)destination;
        int prev = idx - 1;

        if (prev >= 0 && idx >= 0 && idx < airPlanes.Length)
        {
            Vector3 targetPos = airPlanes[idx].position;

            airPlanes[idx].DOMove(airPlanes[prev].position, 0f).OnComplete(() =>
            {
                airPlanes[prev].gameObject.SetActive(false);
                airPlanes[idx].gameObject.SetActive(true);
                airPlanes[idx].transform.DOMove(targetPos, 1f);
            });
        }
    }
}
