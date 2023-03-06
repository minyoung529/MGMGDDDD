using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private bool isUnmove;
    private Vector3 inPoint;
    private Vector3 outPoint;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, inPoint) <= range)
        {
            transform.position = outPoint;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Boss")) {
            rigid.AddForce((transform.position - collision.GetContact(0).point).normalized * 100f, ForceMode.Impulse);
            BossScript boss = collision.transform.GetComponent<BossScript>();
            boss.GetDamage(damage);
            GetComponent<Collider>().enabled = false;
        }
        if (!isUnmove) return;
        if (collision.transform.CompareTag("Floor")) {
            GetComponent<UnMovedObject>().enabled = true;
        }
    }

    public void SetPoint(Vector3 inPoint, Vector3 outPoint) {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
    }

    public void Destroy()
    {
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        float time = 0;
        while (time <= 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, time / 1);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
