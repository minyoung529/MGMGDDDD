using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.PackageManager;

public class RopeController : MonoBehaviour
{
    [SerializeField]
    private WireController wirePrefab;

    private List<WireController> wires = new List<WireController>();
    private List<ConnectedObject> pets = new List<ConnectedObject>();
    private List<ConnectedObject> connectedObjs = new List<ConnectedObject>();
    private ConnectedObject playerConnect;

    private float distance = 1000f;

    void Start()
    {
        playerConnect = Utils.GetOrAddComponent<ConnectedObject>(gameObject);

        WireController wire = InstantiateRope();
        wire.ConnectStartPoint(playerConnect.RopePosition);
        wires.Add(wire);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ConnectTarget();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetRopes();
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)(int)KeyCode.Alpha1 + i))
            {
                UnConnect(i);
            }
        }
    }

    private void ConnectTarget()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.farClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 dir = (mousePos - Camera.main.transform.position).normalized;
        Ray ray = new Ray(Camera.main.transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, LayerMask.GetMask("Player")))
        {
            if (connectedObjs.Count > 0)
            {
                WireController newWire = InstantiateRope();
                newWire.ConnectStartPoint(connectedObjs.Last().transform);
                wires.Add(newWire);
            }

            ConnectObject(hitInfo.transform, wires.Last());
        }
    }

    private WireController InstantiateRope()
    {
        WireController newWire = Instantiate(wirePrefab);
        return newWire;
    }

    private void ConnectObject(Transform target, WireController wire)
    {
        ConnectedObject connectedObj = target.GetComponent<ConnectedObject>();

        if (pets.Find(x => x == connectedObj) == null)
        {
            pets.Add(connectedObj);
        }

        connectedObjs.Add(connectedObj);
        connectedObj?.Connect(wire, false);
    }

    private void ResetRopes()
    {
        int count = connectedObjs.Count;
        for (int i = 0; i < count; i++)
        {
            UnConnect(connectedObjs.Count - 1);
        }
    }

    private void UnConnect(int index)
    {
        // 플레이어의 줄은 지우지 않음
        if (index != 0)
        {
            Destroy(wires[index].gameObject);

            for (int i = index; i < wires.Count; i++)
            {
                wires.RemoveAt(i);
            }
        }

        connectedObjs[index].UnConnect();
        connectedObjs.RemoveAt(index);
    }
}
