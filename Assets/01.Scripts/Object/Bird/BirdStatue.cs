using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStatue : MonoBehaviour
{
    [SerializeField] private List<Light> lights = new List<Light>();
    [SerializeField] private Vector3 targetEuler;

    public void OnPuzzle()
    {
        foreach (Light light in lights)
        {
            light.DOColor(Color.green, 1f);
        }

        transform.DORotate(targetEuler, 3f);

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeAll;
    }
}
