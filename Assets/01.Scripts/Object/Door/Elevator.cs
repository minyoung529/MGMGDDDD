using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private ToggleRotation leftDoor;
    [SerializeField] private ToggleRotation rightDoor;

    private int floor = 1;
    private const int maxFloor = 3;

    private float distance = 18f;
    private float duration = 5f;

    private bool isMoving = false;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    [ContextMenu("Open")]
    public void OpenElevator()
    {
        leftDoor.Open();
        rightDoor.Open();
    }
    [ContextMenu("Close")]
    public void CloseElevator()
    {
        leftDoor.Close();
        rightDoor.Close();
    }

    public void UpElevator()
    {
        if (floor >= 3) return;
        floor++;
        isMoving = true;
        CloseElevator();

        rigid.DOMoveY(transform.position.y + distance, duration);
        StartCoroutine(DelayOpenMove());
    }

    public void DownElevator()
    {
        if (floor <= 1) return;
        floor--;
        isMoving = true;
        CloseElevator();

        rigid.DOMoveY(transform.position.y + (distance * -1), duration);
        StartCoroutine(DelayOpenMove());
    }

    private IEnumerator DelayOpenMove()
    {
        yield return new WaitForSeconds(2f);
        OpenElevator();
        isMoving = false;
    }
    private IEnumerator DelayCloseMove()
    {
        yield return new WaitForSeconds(2f);
        CloseElevator();    
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Define.PLAYER_LAYER)
        {
            UpElevator();
        }
    }
}
