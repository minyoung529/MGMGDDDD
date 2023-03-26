using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour, IFindable {
    bool IFindable.IsFindable { get => !isButtonOn; }

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform cap;
    [SerializeField] private UnityEvent onPress;
    [SerializeField] private UnityEvent onRise;
    [SerializeField] bool isRerise = false;

    private bool isButtonOn = false;

    private void OnTriggerEnter(Collider other) {
        if (((1 << other.gameObject.layer) & layerMask) != 0) {
            if (isButtonOn) return;
            Press(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (((1 << other.gameObject.layer) & layerMask) != 0) {
            if (!isButtonOn || !isRerise) return;
            Press(false);
        }
    }

    private void Press(bool value) {
        OnButtonAnimation(value);
        (value ? onPress : onRise)?.Invoke();
        isButtonOn = value;
    }

    private void OnButtonAnimation(bool value) {
        cap.DOLocalMoveY(value ? -0.2f : 0, 0.3f);
    }
}
