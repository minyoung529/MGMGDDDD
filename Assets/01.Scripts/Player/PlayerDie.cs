using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerDie : MonoBehaviour
{
    private static GameObject[] respawnTransforms;
    private static Transform player;

    [Header("Prefab")]
    private CanvasGroup dieCanvas;

    private void Awake()
    {
        CanvasGroup canvasPrefab = Resources.Load<CanvasGroup>("DieCanvas");
        dieCanvas = Instantiate(canvasPrefab);
        respawnTransforms = GameObject.FindGameObjectsWithTag("SpawnPoint");
        player = transform;

        EventManager.StartListening(EventName.PlayerDie, OnDie);
    }

    private void OnDie(EventParam param = null)
    {
        if (param == null)
        {
            DieAnimation(SetRespawnClosestPoint);
        }
        else
        {
            DieAnimation(() => RespawnPoint((Vector3)param["position"]));
        }
    }

    public static Vector3 GetClosestRespawnPoint()
    {
        IOrderedEnumerable<GameObject> sorted = respawnTransforms.OrderBy(x => Vector3.Distance(x.transform.position, player.position));
        return sorted.First().transform.position;
    }

    public void SetRespawnClosestPoint()
    {
        player.position = GetClosestRespawnPoint();
    }

    public void RespawnPoint(Vector3 point)
    {
        player.position = point;
    }

    private void DieAnimation(Action onEndAnimation = null)
    {
        Sequence seq = DOTween.Sequence();

        dieCanvas.gameObject.SetActive(true);

        Time.timeScale = 0.1f;
        seq.timeScale = 1f / Time.timeScale;
        seq.Append(dieCanvas.DOFade(1f, 1f));
        seq.AppendInterval(0.8f);

        seq.AppendCallback(() => onEndAnimation?.Invoke());
        seq.AppendCallback(() => Time.timeScale = 1f);
        seq.Append(dieCanvas.DOFade(0f, 1f));
        seq.AppendCallback(() => dieCanvas.gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.PlayerDie, OnDie);
    }
}
