using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuzeScript : MonoBehaviour
{
    [SerializeField] private UnityEvent onComplete;
    [SerializeField] private UnityEvent onRecycle;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private Transform fire;

    private Material material;
    private float amount = 0;
    private float distance = 0;
    private float fireSpeed = 3f;
    private bool canFire = true;

    private readonly int Fill = Shader.PropertyToID("_Fill");

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        distance = Vector3.Distance(startPos.position, endPos.position);
    }

    private void OnCollisionStay(Collision collision)
    {
        // 두 번째 로프에 불 연결이 안됨
        Fire fire = collision.transform.GetComponent<Fire>();
            Debug.Log(collision.collider.name);
        if(fire != null)
        {
            if (fire.IsBurn)
            {
                Fire();
            }
        }
    }

    [ContextMenu("Trigger")]
    public void Fire()
    {
        if (!canFire) return;
        canFire = false;
        fire.gameObject.SetActive(true);
        StartCoroutine(Burn());
    }

    private IEnumerator Burn()
    {
        while (amount < 1)
        {
            amount += fireSpeed * Time.deltaTime / distance;
            if (amount > 1) amount = 1;
            fire.position = Vector3.Lerp(startPos.position, endPos.position, amount);
            material.SetFloat(Fill, amount);
            yield return null;
        }
        onComplete?.Invoke();
        fire.gameObject.SetActive(false);
    }

    public void Recycle()
    {
        if (canFire) return;
        StartCoroutine(RecycleCor());
    }

    private IEnumerator RecycleCor()
    {
        while (amount > 0)
        {
            amount -= fireSpeed * Time.deltaTime / distance;
            if (amount < 0) amount = 0;
            fire.position = Vector3.Lerp(startPos.position, endPos.position, amount);
            material.SetFloat(Fill, amount);
            yield return null;
        }
        onRecycle?.Invoke();
        canFire = true;
    }
}
