using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private Transform inPoint;
    [SerializeField] private Transform outPoint;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, inPoint.position) <= range)
        {
            transform.position = outPoint.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Boss"))
        {
            rigid.AddForce(new Vector3(1, Random.Range(-1, 1), Random.Range(-1, 1)).normalized * 100, ForceMode.Impulse);
            BossScript boss = collision.transform.GetComponent<BossScript>();
            boss.GetDamage(damage);
        }
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
