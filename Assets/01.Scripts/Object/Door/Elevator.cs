using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private Collider collider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        CloseElevator();
    }

    [ContextMenu("Open")]
    public void OpenElevator()
    {
        Debug.Log("Open");
        leftDoor.Open();
        rightDoor.Open();
    }
    [ContextMenu("Close")]
    public void CloseElevator()
    {
        Debug.Log("Close");
        leftDoor.Close();
        rightDoor.Close();
    }

    public void UpElevator()
    {
        if (floor >= 3) return;
        floor++;
        isMoving = true;
        CloseElevator();
        Debug.Log("UP");

        rigid.DOMoveY(transform.position.y + distance, duration).OnComplete(() => TriggerActive(false));
        StartCoroutine(DelayOpenMove());
    }

    public void DownElevator()
    {
        if (floor <= 1) return;
        Debug.Log("Down");
        floor--;
        isMoving = true;
        CloseElevator();

        rigid.DOMoveY(transform.position.y + (distance * -1), duration).OnComplete(() => TriggerActive(false));
        StartCoroutine(DelayOpenMove());
    }

    private IEnumerator DelayOpenMove()
    {
        yield return new WaitForSeconds(duration);
        OpenElevator();
        isMoving = false;
    }
    private IEnumerator DelayCloseMove()
    {
        yield return new WaitForSeconds(duration);
        CloseElevator();    
        isMoving = false;
    }

    public void TriggerActive(bool active)
    {
        collider.enabled = active;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Define.PLAYER_LAYER)
        {
            UpElevator();
        }
    }
}
