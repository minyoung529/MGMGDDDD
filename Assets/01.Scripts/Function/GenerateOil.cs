using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOil : MonoBehaviour
{
    [SerializeField]
    private GameObject oilObject;
    private Vector3 scale;

    private void Start()
    {
        scale = oilObject.transform.localScale;
        oilObject.gameObject.SetActive(false);
    }

    public void Generate()
    {
        oilObject.gameObject.SetActive(true);
        oilObject.transform.localScale = Vector3.zero;
        oilObject.transform.DOScale(scale, 1f);

        SpringJoint joint = oilObject.GetComponent<SpringJoint>();
        joint.connectedBody = GetComponent<Rigidbody>();
    }
}
