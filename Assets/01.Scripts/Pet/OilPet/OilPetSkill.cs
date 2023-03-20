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


    public void Init(PaintingObject painting, LineRenderer line, NavMeshAgent pathAgent, NavMeshAgent player)
    {
        this.painting = painting;
        this.pathAgent = pathAgent;
        agent = player;
        lineRenderer = line;

        pathAgent.transform.SetParent(null);
    }

    public void OnClickSkill()
    {
        painting.ResetData();
        agent.isStopped = true;

        pathAgent.gameObject.SetActive(true);
        pathAgent.transform.position = prevPosition = oilStartPos = GameManager.Instance.GetCameraHit();

        points.Clear();
        points.Add(oilStartPos);
    }

    public void ResetSkill()
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
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    public void StartSpreadOil(Action onStartPath, Action onEndPath)
    {
        if (!painting.IsPainting)
        {
            painting.IsPainting = true;
            onStartPath?.Invoke();
            agent.isStopped = true;

            for (int i = 0; i < points.Count; i++)
            {
                points[i] += Vector3.up * 0.5f;
            }

            float speed = skillDistance / agent.speed * 0.7f;

            // 나중에 끊기
            agent.transform.DOPath(points.ToArray(), speed).OnComplete(() =>
            {
                onEndPath?.Invoke();
                agent.isStopped = false;
                ResetSkill();
            }).OnKill(() =>
            {
                onEndPath?.Invoke();
                agent.isStopped = false;
                ResetSkill();
            });
        }
    }

    public void Update(bool isSkilling, bool isDragging)
    {
        if (isDragging)
        {
            if (skillDistance > MAX_OIL_DIST)
            {
                return;
            }

            Vector3 cameraHit = GameManager.Instance.GetCameraHit();

            if (!pathAgent.gameObject.activeSelf)
            {
                pathAgent.gameObject.SetActive(true);
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
            pathAgent.transform.position = GameManager.Instance.GetCameraHit();
        }
    }
}
