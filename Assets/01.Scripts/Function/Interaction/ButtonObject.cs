using DG.Tweening;
using System;
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
    private bool isButtonOn = false;
    public bool IsButtonOn => isButtonOn;
    public GameObject EnterObject => obj;

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

    [ContextMenu("Press")]
    public void Press()
    {
        Press(true);
    }


    public void PressButton(bool value)
    {
        Press(value);
    }

    #region LISTENING
    public void ListeningOnPress(Action action)
    {
        onPress.AddListener(() => action.Invoke());
    }

    public void StopListeningOnPress(Action action)
    {
        onPress.RemoveListener(() => action.Invoke());
    }


    public void ListeningOnRise(Action action)
    {
        onRise.AddListener(() => action.Invoke());
    }


    public void StopListeningOnRise(Action action)
    {
        onRise.RemoveListener(() => action.Invoke());
    }

    #endregion
}
