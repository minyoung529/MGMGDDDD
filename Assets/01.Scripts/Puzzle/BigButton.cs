using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigButton : MonoBehaviour
{
    [SerializeField] private int targetCount = 3;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float targetHeight;
    private float originHeight;
    private List<Transform> objs = new List<Transform>();

    private void Awake() {
        originHeight = transform.localPosition.y;
    }

    private void OnCollisionEnter(Collision collision) {
        if ((layerMask.value | (1 << collision.gameObject.layer)) != layerMask.value) return;
        foreach (Transform item in objs)
            if (item == collision.transform) return;
        collision.rigidbody.velocity = Vector3.zero;
        objs.Add(collision.transform);
        Move();
    }

    private void Update() {
        List<Transform> items = new List<Transform>();
        foreach (Transform item in objs) {
            if (Vector3.Distance(item.position, transform.position) >= 3f) {
                items.Add(item);
            }
        }
        foreach (Transform item in items) {
            objs.Remove(item);
        }
        Move();
    }

    private void Move() {
        transform.DOKill();
        transform.DOLocalMoveY(Mathf.Lerp(originHeight, targetHeight, objs.Count / (float)targetCount), 0.5f);
    }
}
