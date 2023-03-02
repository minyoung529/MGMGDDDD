using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLight : MonoBehaviour
{
    [SerializeField]
    private PathFollower follower;

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        follower.Depart();
    }
}
