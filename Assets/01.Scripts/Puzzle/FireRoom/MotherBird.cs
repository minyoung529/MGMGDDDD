using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotherBird : MonoBehaviour
{
    [SerializeField] private int birdCount = 3;
    [SerializeField] private ToggleScale toggleScale;
    [SerializeField] private UnityEvent onClear;
    [SerializeField] private UnityEvent onFirstClear;
    private int solveCount = 0;
    private Vector3 originScale;

    private void Start() {
        originScale = toggleScale.transform.localScale;
    }

    public void Clear() {
        solveCount++;
        if (solveCount == 1) onFirstClear.Invoke();
        Vector3 scale = originScale;
        scale.y = originScale.y * ((birdCount - solveCount) / (float)birdCount);
        toggleScale.SetTargetScale(scale);
        toggleScale.Size();
        onClear?.Invoke();
    }
}
