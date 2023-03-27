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
    private Action<bool> onChangeCanMove;

    [SerializeField] private Transform movableRoot;

    private NavMeshObstacle obstacle;
    private Conditions stickyConditions;

    #region Property
    public bool IsSticky { get { return isSticky; } set { isSticky = value; } }
    public bool CanMove { get { return canMove; } set { canMove = value; OnMoveChange(value); } }
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
    public bool Condition
    {
        get
        {
            if (stickyConditions)
                return stickyConditions.Condition();

            return true;
        }
    }
    #endregion

    private void Awake()
    {
        if (!movableRoot)
        {
            movableRoot = transform;
        }

        Rigidbody = movableRoot.GetComponentInChildren<Rigidbody>();
        obstacle = movableRoot.GetComponentInChildren<NavMeshObstacle>();
        stickyConditions = GetComponent<Conditions>();
    }

    private void Update()
    {
        if (stickyConditions)
        {
            bool condition = stickyConditions.Condition();

            if (condition != canMove)
                CanMove = condition;
        }
    }

    public void StartListeningNotSticky(Action action)
    {
        notSticky = action;
    }

    public void StartListeningChangeCanMove(Action<bool> action)
    {
        onChangeCanMove = action;
    }

    public void NotSticky()
    {
        if (!IsSticky) return;

        isSticky = false;
        notSticky?.Invoke();
        notSticky = null;

        if (obstacle)
            obstacle.enabled = true;
    }

    public void OnSticky()
    {
        if (IsSticky) return;

        isSticky = true;
        if (obstacle)
            obstacle.enabled = false;
    }

    public void OnMoveChange(bool canMove)
    {
        onChangeCanMove?.Invoke(canMove);
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
