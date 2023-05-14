using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.PlayerController.transform;
    }

    public void LookAt()
    {
        if(player == null)
        {
            Debug.Log("SDf");
            return;
        }
    
        transform.LookAt(player.transform, Vector3.up);
    }
}
