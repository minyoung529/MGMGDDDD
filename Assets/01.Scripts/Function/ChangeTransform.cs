using UnityEngine;
using DG.Tweening;

public class ChangeTransform : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 scale;

    public void Move() {
        targetTransform.DOMove(position, duration);
    }

    public void Rotate() {
        targetTransform.DORotate(rotation, duration);
    }

    public void Size( ) {
        targetTransform.DOScale(scale, duration);
    }
}
