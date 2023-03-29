using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropperController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onDropperStart;

    [SerializeField]
    private Transform playerSpawnPosition;

    private PlayerMove player;
    private JumpMotion jumpMotion = new();

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>();
        jumpMotion.targetPos = playerSpawnPosition.position;
    }

    public void StartDropper()
    {
        onDropperStart?.Invoke();
        jumpMotion.StartJump(player.transform, null, null, false, 2f);
    }

    public void StopDropper()
    {

    }

    public void EndDropper()
    {

    }
}
