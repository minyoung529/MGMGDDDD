using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isSelected = false;
    private bool isFirstSet = false;

    [SerializeField]
    private ChangeEmission emission;
    [SerializeField]
    private Color originalEmissionColor;
    [SerializeField]
    private Color wrongEmissionColor;
    [SerializeField]
    private Color successEmissionColor;

    [SerializeField]
    private LayerMask layer;

    new private Renderer renderer;

    public Func<bool> IsLock { get; set; }
    public Action<PressurePlate> OnSelectedAction { get; set; }
    public Action<PressurePlate> OnUnSelectedAction { get; set; }

    #region PROPERTY
    public bool IsSelected => isSelected;
    #endregion

    private void Awake()
    {
        emission.SetColor(originalEmissionColor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFirstSet) return;

        if (((1 << collision.gameObject.layer) & layer) != 0)
        {
            isFirstSet = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isSelected || isFirstSet) return;

        if (((1 << collision.gameObject.layer) & layer) != 0)
        {
            Selected();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isSelected) return;

        if (((1 << collision.gameObject.layer) & layer) != 0)
        {
            UnSelected();
        }
    }

    [ContextMenu("Selected")]
    public void Selected()
    {
        // 잠긴 상태가 아니면

        if (!IsLock.Invoke())
        {
            isSelected = true;
            OnSelected();
            emission?.Change();
            OnSelectedAction.Invoke(this);
        }
    }

    public void UnSelected()
    {
        emission?.BackToOriginalColor();
        emission.SetColor(originalEmissionColor);
        OnUnSelectedAction?.Invoke(this);
        isSelected = false;
    }

    public void WrongResult()
    {
        emission.SetIsMaintain(false);
        emission.SetColor(wrongEmissionColor);
        emission?.Change();
    }

    public void CorrectResult()
    {
        emission.SetIsMaintain(false);
        emission.SetColor(successEmissionColor);
        emission?.Change();
    }

    public virtual void OnSelected() { }

    public void ResetPuzzle()
    {
        UnSelected();
        emission.SetIsMaintain(true);
        OnSetValue();

        OnReset();
    }

    public virtual void OnReset() { }

    protected void OnSetValue()
    {
        isFirstSet = true;
    }
}
