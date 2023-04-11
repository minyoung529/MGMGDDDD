using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isSelected = false;

    [SerializeField]
    private ChangeEmission emission;

    [SerializeField]
    private LayerMask layer;

    new private Renderer renderer;

    public Func<bool> IsLock { get; set; }
    public Action<PressurePlate> OnSelectedAction { get; set; }

    #region PROPERTY
    public bool IsSelected => isSelected;
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & layer) != 0)
        {
            Selected();
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
        isSelected = false;
    }

    public virtual void OnSelected() { }

    public void ResetPuzzle()
    {
        UnSelected();
        OnReset();
    }

    public virtual void OnReset() { }
}
