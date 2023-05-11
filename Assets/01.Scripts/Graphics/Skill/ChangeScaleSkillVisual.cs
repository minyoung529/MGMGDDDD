using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChangeScaleSkillVisual : SkillVisual
{
    [SerializeField]
    private Transform changedObject;

    [SerializeField]
    private Vector3 targetScale = new Vector3(3f, 3f, 3f);

    [SerializeField]
    private float duration = 0.5f;

    protected override void OnTrigger()
    {
        changedObject.DOKill();
        changedObject.DOScale(targetScale, duration);
    }
}
