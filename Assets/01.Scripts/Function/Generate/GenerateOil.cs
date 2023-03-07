using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOil : MonoBehaviour
{
    [SerializeField] private GameObject oilObject;
    [SerializeField] private Material oilMat;

    public List<MeshRenderer> meshRenderer = new List<MeshRenderer>();
    private Material[][] mats = new Material[5][];
    private Vector3 scale;

    private void Awake()
    {

        for (int i = 0; i < meshRenderer.Count; i++)
        {
            mats[i] = meshRenderer[i].materials;
        }
    }

    private void Start()
    {
        scale = oilObject.transform.localScale;
        oilObject.gameObject.SetActive(false);
    }

    public void Generate()
    {
        //     oilObject.gameObject.SetActive(true);
        for (int i = 0; i < meshRenderer.Count; i++)
        {
            if (mats.Length == 1) continue;

            mats[i][1] = oilMat;
            meshRenderer[i].materials = mats[i];
        }
        //    oilObject.transform.localScale = Vector3.zero;
        //   oilObject.transform.DOScale(scale, 1f);

        SpringJoint joint = oilObject.GetComponent<SpringJoint>();

        if (joint)
        {
            joint.connectedBody = GetComponent<Rigidbody>();
        }
    }

}
