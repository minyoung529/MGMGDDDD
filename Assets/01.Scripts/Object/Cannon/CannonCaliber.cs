using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonCaliber : MonoBehaviour
{
    [SerializeField] private LayerMask bumpLayer;
    [SerializeField] private float radius;
    [SerializeField] private float bumpPow = 250f;
    [SerializeField] private float spinSpeed;

    private Pet[] pets;
    private bool isFire = false;
    private Rigidbody rigid;
    private CannonScript cannon;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    public void Fire(CannonScript cannon, Pet[] pets, Vector3 dir) {
        this.cannon = cannon;
        this.pets = pets;
        Vector3 pos = Vector3.up;
        foreach (Pet item in pets) {
            item.transform.position = transform.position + pos * radius;
            pos = Quaternion.AngleAxis(360 / pets.Length, transform.forward) * pos;
            item.transform.SetParent(transform);
        }
        rigid.AddForce(dir, ForceMode.Impulse);
        isFire = true;
    }

    private void Update() {
        if (!isFire) return;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x + spinSpeed * Time.deltaTime, 0, 0));
        Collider[] others = Physics.OverlapSphere(transform.position, radius, bumpLayer);
        if (others.Length > 0 && Vector3.Distance(transform.position, cannon.transform.position) > 5f) {
            Vector3 hitPoint = others[0].ClosestPoint(transform.position);
            Pop(pets, hitPoint);
            pets = null;
            Destroy(gameObject);
        }
    }

    public void Pop(Pet[] pets, Vector3 hitPoint) {
        isFire = false;
        foreach (Pet item in pets) {
            item.transform.SetParent(null);
            Vector3 dir = (item.transform.position - hitPoint).normalized;
            float angle = Mathf.Abs(Vector3.Angle(Vector3.up, dir));
            if (angle > 10) {
                dir = Vector3.RotateTowards(dir, Vector3.up, Mathf.Deg2Rad * (angle - 10) , 0).normalized;
            }
            item.Throw(dir * bumpPow);
        }
    }
}