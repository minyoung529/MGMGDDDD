using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ConnectedRope : MonoBehaviour
{
    private Pair<RopeCollider, RopeCollider> ropePair;
    [SerializeField]
    private Transform mid;

    private RopeDetect ropeDetect;

    private void Start()
    {
        RopeCollider[] ropeColliders = GetComponentsInChildren<RopeCollider>();
        ropeDetect = GetComponent<RopeDetect>();

        ropePair.first = ropeColliders[0];
        ropePair.second = ropeColliders[1];

        InputManager.StartListeningInput(InputAction.UnConnect, InputType.GetKeyDown, UnConnect);
    }

    public void Connect(Transform from, Transform to)
    {
        transform.position = Utils.GetMid(from.position, to.position);

        ropePair.first.Connect(from, mid, true);
        ropePair.second.Connect(to, mid , true);

        ropeDetect?.SetSize();
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
