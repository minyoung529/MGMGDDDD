using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrangePuzzle : MonoBehaviour
{
    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    private GameObject targetObject;

    [SerializeField] private UnityEvent OnInteraction;
    private bool isInteract = false;

    private void Start()
    {
        OnInteraction.AddListener(() => Arrange(targetObject));
    }

    void Update()
    {
        if (isInteract) return;

        Collider[] cols = Physics.OverlapSphere(transform.position, distance, layerMask);

        Debug.Log(cols.Length);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject == targetObject)
            {
                Debug.Log("INTERACT BIRD");
                isInteract = true;
                OnInteraction?.Invoke();
            }
        }
    }

    private void Arrange(GameObject obj)
    {
        obj.transform.DOMove(transform.position, 1f);
    }
}
