using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ConnectedRope : MonoBehaviour
{
    private Pair<RopeCollider, RopeCollider> ropePair;

    private void Start()
    {
        RopeCollider[] ropeColliders = GetComponentsInChildren<RopeCollider>();

        ropePair.first = ropeColliders[0];
        ropePair.second = ropeColliders[1];

        InputManager.StartListeningInput(InputAction.UnConnect, InputType.GetKeyDown, UnConnect);
    }

    public void Connect(Transform from, Transform to)
    {
        float dist = Vector3.Distance(from.position, to.position);
        Vector3 mid = from.position + (to.position - from.position).normalized * 0.5f * dist;

        ropePair.first.Connect(from, mid, true);
        ropePair.second.Connect(to, mid , true);
    }

    public void UnConnect(InputAction action = InputAction.UnConnect, InputType type = InputType.GetKeyDown, float value = 0f)
    {
        ropePair.first?.UnConnect();
        ropePair.second?.UnConnect();
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.UnConnect, InputType.GetKeyDown, UnConnect);
    }
}
