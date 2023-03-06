using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController : MonoBehaviour
{
    [SerializeField] private Transform[] airPlanes;
    private Vector3[] positions;

    private void Start()
    {
        positions = new Vector3[airPlanes.Length];

        for (int i = 0; i < airPlanes.Length; i++)
        {
            positions[i]= airPlanes[i].position;    
        }
    }

    public void ChangePlanePos(Destination destination)
    {
        int idx = (int)destination;
        int prev = idx - 1;

        if (prev >= 0 && idx >= 0 && idx < airPlanes.Length)
        {
            Vector3 targetPos = positions[idx];

            airPlanes[idx].DOMove(airPlanes[prev].position, 0f).OnComplete(() =>
            {
                airPlanes[prev].gameObject.SetActive(false);
                airPlanes[idx].gameObject.SetActive(true);
                airPlanes[idx].transform.DOMove(targetPos, 1f);
            });
        }
    }
}
