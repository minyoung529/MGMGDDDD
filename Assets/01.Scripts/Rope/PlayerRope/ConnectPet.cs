using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;

public class ConnectPet : MonoBehaviour
{
    [SerializeField]
    private WireController wirePrefab;

    private List<WireController> petRopes = new List<WireController>();
    private List<ConnectedObject> pets = new List<ConnectedObject>();

    [SerializeField]
    private Rigidbody petRopeRigid;

    void Start()
    {
        CreateRope(petRopeRigid);
    }

    public void UnConnect()
    {
        pets.ForEach(x => x.UnConnect());
        pets.Clear();

        for (int i = 1; i < petRopes.Count; i++)
        {
            Destroy(petRopes[i].gameObject);
        }

        petRopes.RemoveRange(1, petRopes.Count - 1);
    }

    private WireController CreateRope(Rigidbody target = null)
    {
        WireController wire = Instantiate(wirePrefab);

        if (target)
        {
            wire.ConnectStartPoint(target);
        }

        petRopes.Add(wire);

        return wire;
    }

    public void Connect(ConnectedObject connectedObj)
    {
        if (pets.Find(x => x == connectedObj) == null)  // 같은 펫이 아니면
        {
            if (pets.Count > 0)
            {
                CreateRope(pets.Last().Rigid);
            }

            connectedObj.Connect(petRopes.Last());
            pets.Add(connectedObj);
        }
    }
}
