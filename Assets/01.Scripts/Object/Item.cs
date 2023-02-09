using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] Transform attachPoint;

    public LayerMask playerLayer;
    private float nearRadius = 1f;
    private Rigidbody rigid;

    private bool isGet = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (NearPlayer())
            {
                GetItem();
            }
        }
    }

    #region Boolean
    // 상호작용 가능한 범위인가 체크하는 함수
    private bool NearPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, nearRadius, playerLayer);

        if (colliders.Length > 0) return true;
        return false;
    }
    #endregion

    protected virtual void GetItem()
    {
        isGet = true;
        gameObject.SetActive(false);
    }
    protected virtual void UseItem()
    {
        isGet = false;
    }

    protected virtual void PutItem()
    {
        isGet = false;

        //rigid.isKinematic = false;
        //rigid.useGravity = true;
        //transform.SetParent(null);
    }
}
