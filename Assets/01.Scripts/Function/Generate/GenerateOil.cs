using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOil : MonoBehaviour
{
    [SerializeField] private GameObject oilObject;
    [SerializeField] private Material oilMat;

    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private Vector3 scale;

    private void Awake()
    {
        meshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            if (meshRenderers[i].materials.Length == 1)
            {
                //List<Material> matList = new List<Material>(meshRenderers[i].materials);
                meshRenderers[i].materials = new Material[2] { meshRenderers[i].material, null };
            }
        }
    }

    private void Start()
    {
        scale = oilObject.transform.localScale;
        oilObject.gameObject.SetActive(false);
    }

    public void Generate()
    {
        Debug.Log("GENERATE");

        //     oilObject.gameObject.SetActive(true);
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].materials[1] = oilMat;
            meshRenderers[i].sharedMaterials[1].CopyPropertiesFromMaterial(oilMat);
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
