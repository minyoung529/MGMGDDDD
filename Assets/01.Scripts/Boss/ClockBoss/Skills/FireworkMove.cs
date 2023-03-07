using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireworkMove : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    [SerializeField] private float range = 5;
    [SerializeField] private float time = 3;
    [SerializeField] private float damage = 15;
    [SerializeField] private int pathCount = 5;

    private Vector3 target;
    private Sequence seq;
    private Rigidbody rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    public void Enable() {
        particle.SetActive(true);
        seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(2, 1));
        seq.Join(transform.DOLookAt(target, 1));
        seq.Append(transform.DOMove(target, time));
        /*
        Vector3[] path = new Vector3[pathCount];

        Vector3 dir = (target - transform.position).normalized;
        Vector3 axis = new Vector3(dir.z, 0, dir.x);
        axis = Quaternion.AngleAxis(90, axis) * dir;

        for(int i = 0; i < pathCount; i++) {
            path[i] = Vector3.Lerp(transform.position, target, i + 1 / pathCount);
            float factor;
            if (i + 1 < pathCount) {
                factor = Mathf.Lerp(0, 1, (i + 1) * (pathCount * 0.5f));
            }
            else {
                factor = Mathf.Lerp(1, 0, ((i + 1) - pathCount * 0.5f) / (pathCount * 0.5f));
            }
            path[i] += axis * (range * factor);
            axis = Quaternion.AngleAxis(360 / pathCount, dir) * axis;
        }

        transform.DOPath(path, pathCount, PathType.CatmullRom);
        */
    }

    public void SetTarget(Vector3 target) {
        this.target = target;   
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("FirePet")) {
            Enable();
        }
        if (collision.transform.CompareTag("Boss")) {
            rigid.AddForce((transform.position - collision.GetContact(0).point).normalized * 100f, ForceMode.Impulse);
            BossScript boss = collision.transform.GetComponent<BossScript>();
            boss.GetDamage(damage);
            particle.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
