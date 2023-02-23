using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectStair : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField] private GameObject stepRayUpper;
    [SerializeField] private GameObject stepRayLower;
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 2f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    private void FixedUpdate()
    {
        StepClimb();
    }

    void StepClimb()
    {
        Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward) * 0.2f, Color.red);
        Debug.DrawRay(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward) * 0.2f, Color.blue);

        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitLower, 0.2f))
        {
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitUpper, 0.2f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        //RaycastHit hitLower45;
        //if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        //{
        //    RaycastHit hitUpper45;
        //    if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
        //    {
        //        rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
        //    }
        //}

        //RaycastHit hitLowerMinus45;
        //if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        //{
        //    RaycastHit hitUpperMinus45;
        //    if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
        //    {
        //        rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
        //    }
        //}
    }
}
