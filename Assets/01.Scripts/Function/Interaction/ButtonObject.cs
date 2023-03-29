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

    private GameObject obj = null;
    private bool isButtonOn = false;
    public GameObject EnterObject => obj;

    private void OnTriggerEnter(Collider other) {
        if (((1 << other.gameObject.layer) & layerMask) != 0) {
            if (isButtonOn) return;
            obj = other.gameObject;
            Press(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (((1 << other.gameObject.layer) & layerMask) != 0) {
            if (!isButtonOn || !isRerise || obj != other.gameObject) return;
            obj = null;
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

    public void PressButton(bool value) {
        Press(value);
    }

}
