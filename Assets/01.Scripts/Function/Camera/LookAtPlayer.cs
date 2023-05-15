using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField]
    private float duration = 0f;
    private float timer = 0f;
    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.MainCam.transform;
        timer = duration;
    }

    private  void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            transform.forward = -player.transform.forward;
        }
    }

    public void LookAt()
    {
        timer = 0f;
    }
}
