using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sticky : MonoBehaviour
{
    [SerializeField] private bool canMove = false;
    [SerializeField] private bool applyRotatationOffset = true;
    private bool isSticky = false;
    private Action notSticky;

    [SerializeField] private Transform movableRoot;

    private NavMeshObstacle obstacle;

    #region Property
    public bool IsSticky { get { return isSticky; } set { isSticky = value; } }
    public bool CanMove { get { return canMove; } }
    public Transform MovableRoot
    {
        get
        {
            if (movableRoot == null)
                movableRoot = transform;
            return movableRoot;
        }
    }
    public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField]
    public bool ApplyOffset { get; set; } = true;
    #endregion

    private void Awake()
    {
        if (!movableRoot)
        {
            movableRoot = transform;
        }

        Rigidbody = movableRoot.GetComponentInChildren<Rigidbody>();
        obstacle = movableRoot.GetComponentInChildren<NavMeshObstacle>();
    }

    public void StartListeningNotSticky(Action action)
    {
        notSticky = action;
    }

    public void NotSticky()
    {
        notSticky?.Invoke();
        notSticky = null;

        if (obstacle)
            obstacle.enabled = true;
    }

    public void OnSticky()
    {
        if (obstacle)
            obstacle.enabled = false;
    }

    //public void SetSticky()
    //{
    //    if(isSticky) return;

    //    isSticky = true;
    //}

    //public void NotSticky()
    //{
    //    if(!isSticky) return;

    //    isSticky = false;
    //}
}
