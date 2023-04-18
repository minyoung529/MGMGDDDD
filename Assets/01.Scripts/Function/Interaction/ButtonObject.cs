using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour, IFindable
{
    bool IFindable.IsFindable { get => !isButtonOn; }

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform cap;
    [SerializeField] private UnityEvent onPress;
    [SerializeField] private UnityEvent onRise;
    [SerializeField] private bool isRerise = false;

    private GameObject obj = null;
    public GameObject EnterObject => obj;
    private bool isButtonOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isButtonOn) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            obj = other.gameObject;
            Press(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            if (!isButtonOn || !isRerise || obj != other.gameObject) return;
            obj = null;
            Press(false);
        }
    }

    [ContextMenu("Press")]
    public virtual void Press(bool value)
    {
        DoButtonAnimation(value);
        (value ? onPress : onRise)?.Invoke();
        isButtonOn = value;
    }

    private void DoButtonAnimation(bool enable)
    {
        cap.DOLocalMoveY(enable ? -0.2f : 0, 0.3f);
    }
}
