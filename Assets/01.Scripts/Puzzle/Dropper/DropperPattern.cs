using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

[System.Serializable]
public struct PosAndRot
{
    public Vector3 position;
    public Vector3 rotation;

    public PosAndRot(Vector3 position, Vector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}

public class DropperPattern : MonoBehaviour
{
    [SerializeField]
    private List<PosAndRot> beginTransforms;

    [SerializeField]
    private List<PosAndRot> puzzleTransforms;

    [SerializeField]
    private UnityEvent onStartPattern;

    [SerializeField]
    private float duration = 0.3f;

    private Transform target = null;

    private float scale = 1f;
    private readonly float SCALE = 1.94f;

    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.position;
        //scale = transform.localScale.x / SCALE;
    }

    private void Start()
    {
        if (puzzleTransforms.Count == 0) return;

        for (int i = 0; i < puzzleTransforms.Count; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = beginTransforms[i].position;
            child.localEulerAngles = beginTransforms[i].rotation;
        }
    }

    public void Init(Transform target)
    {
        this.target = target;
    }

    [ContextMenu("Start Pattern")]
    public void PatternStart()
    {
        gameObject.SetActive(true);

        onStartPattern?.Invoke();
        EnterPatternAnimation();

        transform.DOMoveY(transform.position.y + Vector3.Distance(target.position, transform.position) + 25f, 2.65f);
    }

    private void EnterPatternAnimation()
    {
        ChangePositions(puzzleTransforms, duration);
    }

    public void ExitPatternAnimation()
    {
        ChangePositions(beginTransforms, duration, () => gameObject.SetActive(false));
    }

    private void ChangePositions(List<PosAndRot> posAndRots, float duration, Action onEnd = null)
    {
        if (beginTransforms == null || puzzleTransforms == null || beginTransforms.Count == 0 || puzzleTransforms.Count == 0)
        {
            onEnd?.Invoke();
            return;
        }

        for (int i = 0; i < beginTransforms.Count; i++)
        {
            transform.GetChild(i).DOLocalMove(posAndRots[i].position * scale, duration);
            transform.GetChild(i).DOLocalRotate(posAndRots[i].rotation, duration);
        }

        if (onEnd != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(duration);
            seq.AppendCallback(() => onEnd.Invoke());
        }
    }

    #region RECORD
    [ContextMenu("* Record Puzzle Trasform")]
    public void RecordPuzzleTransform()
    {
        puzzleTransforms = RecordTrasnform();
    }

    [ContextMenu("* Record Begin Trasform")]
    public void RecordBeginTransform()
    {
        beginTransforms = RecordTrasnform();
    }

    private List<PosAndRot> RecordTrasnform()
    {
        List<PosAndRot> list = new();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            list.Add(new PosAndRot(child.localPosition, child.localEulerAngles));
        }

        return list;
    }
    #endregion

    [ContextMenu("* Set Puzzle Position")]
    public void SetPuzzlePosition()
    {
        if (puzzleTransforms.Count == 0) return;

        for (int i = 0; i < puzzleTransforms.Count; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = puzzleTransforms[i].position;
            child.localEulerAngles = puzzleTransforms[i].rotation;
        }
    }

    public void ResetPattern()
    {
        transform.DOKill();

        for (int i = 0; i < puzzleTransforms.Count; i++)
        {
            transform.GetChild(i).DOKill();
        }

        transform.position = originalPos;
        gameObject.SetActive(false);

        if (puzzleTransforms.Count == 0) return;

        for (int i = 0; i < puzzleTransforms.Count; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = beginTransforms[i].position;
            child.localEulerAngles = beginTransforms[i].rotation;
        }
    }

    public void Pause()
    {
        transform.DOKill();
    }
}
