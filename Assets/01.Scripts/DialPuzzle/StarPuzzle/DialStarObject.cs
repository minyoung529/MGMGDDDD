using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialStarObject : MonoBehaviour
{
    private int index;
    public int Index => index;

    [SerializeField]
    private bool isFullStar;

    private readonly string ATTACH_POSITION = "RailPosition";
    private Vector3 originalPosition = Vector3.zero;

    #region Action
    public Action<int> OnAttached { get; set; }
    public Action<int> OnReset { get; set; }
    public Action<int> OnContactFire { get; set; }
    #endregion

    private Material material;
    private BoxCollider boxCollider;
    private bool isAttached = false;
    private Transform attachTransform;

    #region PROPERTY
    private readonly int EMISSION_HASH = Shader.PropertyToID("_EmissionColor");
    private readonly Color STAR_COLOR = new Color(2.297397f, 1.202759f, 0f);
    #endregion

    private void Awake()
    {
        index = transform.GetSiblingIndex();
        originalPosition = transform.position;

        if (isFullStar)
        {
            Transform[] childs = transform.GetComponentsInChildren<Transform>();

            for (int i = 1; i <= transform.childCount; i++)
            {
                if (i - 1 != Index)
                {
                    Destroy(childs[i].gameObject);
                }
                else
                {
                    material = childs[i].GetComponent<Renderer>().material;
                    boxCollider = childs[i].GetComponent<BoxCollider>();
                }
            }
        }
    }

    private void Update()
    {
        if (isAttached)
        {
            transform.position = attachTransform.position + Vector3.one * 0.38f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.STICKY_PET_TAG))
        {
            Attach(other);
        }
    }

    public void Attach(Collider other)
    {
        if (other == null) return;
        if (isAttached) return;

        attachTransform = other.transform.Find(ATTACH_POSITION);

        if (attachTransform == null)
        {
            attachTransform = other.transform;
        }

        ChangeAttach();
        OnAttached.Invoke(Index);

        isAttached = true;
    }

    private void ChangeAttach()
    {
        //transform.localScale = Vector3.up * 0.38f;
        transform.localEulerAngles = Vector3.zero + Vector3.forward * 38f;

        boxCollider.size *= 2f;
    }

    public void ResetStar()
    {
        ResetPosition();
        isAttached = false;
    }

    private void ResetPosition()
    {
        transform.position = originalPosition;
        OnReset.Invoke(Index);
        material.DOColor(Color.black, EMISSION_HASH, 0.5f);;
        boxCollider.size *= 0.5f;

        attachTransform = null;
    }

    public void OnFire()
    {
        OnContactFire.Invoke(Index);
    }

    public void Success()
    {
        Debug.Log("SUCCESS");
        material.DOColor(STAR_COLOR, EMISSION_HASH, 0.5f);
        //material.SetFloat(EMISSION_HASH, 1f);
    }
}
