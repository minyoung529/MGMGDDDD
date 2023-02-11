using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    private float dist = 0f;

    [SerializeField]
    private float targetDist = 2f;
    private Vector3 prevPosition;

    private void Awake()
    {
        prevPosition = transform.position;
    }

    private void Update()
    {
        if (dist > targetDist)
        {
            if (CheckValidate())
            {
                Generate();
                dist = 0f;
            }
        }
        else
        {
            dist += Vector3.Distance(prevPosition, transform.position);
        }

        prevPosition = transform.position;
    }

    private void Generate()
    {
        Debug.Log("GENERATE");
        ps.Emit(1);
        //ps.Play();
    }

    private bool CheckValidate()
    {
        return true;
    }
}
