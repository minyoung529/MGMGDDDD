using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialCar : MonoBehaviour
{
    [SerializeField] private float minSpeed = 2.0f;
    [SerializeField] private float maxSpeed = 10.0f;

    private float speed = 1f;

    private void Update()
    {
        ForwardMove();   
    }

    private void OnEnable()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void ForwardMove()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);
        if(transform.localPosition.z > 24f)
        {
            Destroy(gameObject);
        }
    }

    private void DeadPlayer()
    {
        Debug.Log("GameOver");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag(Define.PLAYER_TAG))
        {
            DeadPlayer();
        }
    }
}
