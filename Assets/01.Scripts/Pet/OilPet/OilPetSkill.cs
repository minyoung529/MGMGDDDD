using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 인스펙터 확인용
[System.Serializable]
public class OilPetSkill
{
    private NavMeshAgent agent;
    private NavMeshAgent pathAgent;

    #region SKILL_VARIABLES
    private Vector3 oilStartPos;
    public Vector3 StartPoint => oilStartPos;
    private PaintingObject painting;
    public PaintingObject Painting => painting;
    private LineRenderer lineRenderer;

    private List<Vector3> points = new List<Vector3>();
    private Vector3 prevPosition;
    private float oilDistance;
    private float skillDistance;

    private readonly float MIN_PATH_DIST = 0.25f;
    private readonly float MAX_OIL_DIST = 25f;
    #endregion

    public static bool IsCrosshair { get; set; } = true;

    public Action OnEndSpread_Once { get; set; }
    public bool IsCheckDistance = true;

    public static Action OnClearOil;

    public void Init(PaintingObject painting, LineRenderer line, NavMeshAgent pathAgent, NavMeshAgent player)
    {
        this.painting = painting;
        this.pathAgent = pathAgent;
        agent = player;
        lineRenderer = line;

        pathAgent.transform.SetParent(null);
    }

    public void ClearOil()
    {
        OnClearOil?.Invoke();
        painting.ResetData();
    }

    public void OnClickSkill()
    {
        ClearOil();
        pathAgent.gameObject.SetActive(true);

        pathAgent.enabled = false;

        if (IsCrosshair)
        {
            pathAgent.transform.position = prevPosition = oilStartPos = GameManager.Instance.GetCameraHit();
        }
        else
        {
            pathAgent.transform.position = prevPosition = oilStartPos = GameManager.Instance.GetMousePos();
        }

        points.Clear();
    }

    private void ResetSkill()
    {
        skillDistance = oilDistance = 0f;
        painting.IsPainting = false;
        pathAgent.gameObject.SetActive(false);
        points.Clear();
    }

    public void ShowPath()
    {
        if (points.Count == 0) return;

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(0, oilStartPos);

        if (points.Count < 2)
        {
            return;
        }

        for (int i = 1; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i - 1]);
        }
    }

    public void StartSpreadOil(Action onStartPath, Action onEndPath)
    {
        if (!painting.IsPainting)
        {
            painting.IsPainting = true;
            onStartPath?.Invoke();
            pathAgent.enabled = false;

            for (int i = 0; i < points.Count; i++)
            {
                points[i] += Vector3.up * 0.5f;
            }

            float duration = skillDistance / agent.speed * 0.7f;

            // 나중에 끊기
            agent.transform.DOPath(points.ToArray(), duration).OnComplete(() =>
            {
                onEndPath?.Invoke();
                KillSkill();
            }).OnKill(() =>
            {
                onEndPath?.Invoke();
                KillSkill();
            });
        }
    }

    private void KillSkill()
    {
        OnEndSpread_Once?.Invoke();
        OnEndSpread_Once = null;
        pathAgent.enabled = true;
        ResetSkill();
    }

    public void Update(bool isSkilling, bool isDragging)
    {
        if (isDragging)
        {
            if (IsCheckDistance && skillDistance > MAX_OIL_DIST)
            {
                return;
            }

            Vector3 cameraHit;

            if (IsCrosshair)
            {
                cameraHit = GameManager.Instance.GetCameraHit();
            }
            else
            {
                cameraHit = GameManager.Instance.GetMousePos();
            }

            if (!pathAgent.gameObject.activeSelf || !pathAgent.enabled)
            {
                pathAgent.gameObject.SetActive(true);
                pathAgent.enabled = true;
                return;
            }

            pathAgent.SetDestination(cameraHit);

            float dist = Vector3.Distance(prevPosition, pathAgent.transform.position);
            oilDistance += dist;
            skillDistance += dist;

            if (oilDistance > MIN_PATH_DIST)
            {
                oilDistance -= MIN_PATH_DIST;
                points.Add(pathAgent.transform.position);
            }

            prevPosition = pathAgent.transform.position;

            ShowPath();
        }

        if (!isSkilling)
        {
            lineRenderer.positionCount = 0;

            if (IsCrosshair)
            {
                pathAgent.transform.position = GameManager.Instance.GetCameraHit();
            }
            else
            {
                pathAgent.transform.position = GameManager.Instance.GetMousePos();
            }
        }
    }
}
