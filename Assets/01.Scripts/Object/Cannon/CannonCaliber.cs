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

    public void Fire(Pet[] pets, Vector3[] path) {
        isFire = true;
        this.pets = pets;
        Vector3 dir = Vector3.up;
        foreach(Pet item in pets) {
            item.transform.position = transform.position + dir * radius;
            dir = Quaternion.AngleAxis(360 / pets.Length, transform.forward) * dir;
        }
        transform.DOPath(path, 2f, PathType.CubicBezier);
    }

    private void Update() {
        if (!isFire) return;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x + spinSpeed * Time.deltaTime, 0, 0));
        Collider[] others = Physics.OverlapSphere(transform.position, radius + 0.1f, bumpLayer);
        if (others.Length > 0) {
            Vector3 hitPoint = others[0].ClosestPoint(transform.position);
            Pop(pets, hitPoint);
            pets = null;
        }
    }

    public void Pop(Pet[] pets, Vector3 hitPoint) {
        foreach (Pet item in pets) {
            item.transform.SetParent(null);
            item.Rigid.velocity = Vector3.zero;
            item.Rigid.AddForce((transform.position - hitPoint).normalized * bumpPow, ForceMode.Impulse);
        }
    }
}
