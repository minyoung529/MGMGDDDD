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
    [SerializeField]
    private LineRope lineRopePrefab;

    WireController startPetRope;
    private List<LineRope> petRopes = new List<LineRope>();
    private List<ConnectedObject> connectedObjs = new List<ConnectedObject>();
    private List<Pet> pets = new List<Pet>();

    [SerializeField]
    private Rigidbody petRopeRigid;

    void Start()
    {
        startPetRope = Instantiate(wirePrefab);
        startPetRope.ConnectStartPoint(petRopeRigid);
    }

    public void UnConnect()
    {
        connectedObjs.ForEach(x => x.UnConnect());
        pets.ForEach(x => x.IsSelected = false);

        connectedObjs.Clear();
        pets.Clear();

        for (int i = 1; i < petRopes.Count; i++)
        {
            Destroy(petRopes[i].gameObject);
        }

        if (petRopes.Count > 1)
        {
            petRopes.RemoveRange(1, petRopes.Count - 1);
        }
        startPetRope.Active(true);
    }

    private LineRope CreateRope(Transform t1, Transform t2)
    {
        LineRope wire = Instantiate(lineRopePrefab);
        wire.SetTarget(t1, t2);

        petRopes.Add(wire);

        return wire;
    }

    public void Connect(ConnectedObject connectedObj)
    {
        if (connectedObjs.Find(x => x == connectedObj) == null)  // 같은 펫이 아니면
        {
            if (connectedObjs.Count > 0)
            {
                CreateRope(connectedObjs.Last().transform, connectedObj.transform);
                connectedObj.Connect(null, connectedObjs.Last().Rigid);
            }
            else
            {
                CreateRope(petRopeRigid.transform, connectedObj.transform);
                connectedObj.Connect(null, petRopeRigid);
            }

            pets.Add(connectedObj.GetComponent<Pet>());
            pets.Last().IsSelected = true;
            startPetRope.Active(false);
            connectedObjs.Add(connectedObj);
        }
    }
}
