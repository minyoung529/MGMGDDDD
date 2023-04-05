using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerRespawn : MonoBehaviour
{
    private static GameObject[] respawnTransforms;
    private static Transform player;

    [Header("Prefab")]
    private static CanvasGroup dieCanvas;

    private void Awake()
    {
        CanvasGroup canvasPrefab = Resources.Load<CanvasGroup>("DieCanvas");
        dieCanvas = Instantiate(canvasPrefab);
        respawnTransforms = GameObject.FindGameObjectsWithTag("SpawnPoint");
        player = transform;
    }

    public static void RespawnClosestPoint()
    {
        IOrderedEnumerable<GameObject> sorted = respawnTransforms.OrderBy(x => Vector3.Distance(x.transform.position, player.position));
        player.position = sorted.First().transform.position;
        FadeInOutAnimation();
    }

    public static void RespawnPoint(Vector3 point)
    {
        player.position = point;
        FadeInOutAnimation();
    }

    private static void FadeInOutAnimation()
    {
        Sequence seq = DOTween.Sequence();

        dieCanvas.gameObject.SetActive(true);

        //seq.timeScale = 1f / Time.timeScale;
        seq.Append(dieCanvas.DOFade(1f, 1f));

        seq.AppendInterval(0.8f);
        //seq.AppendCallback(() =>
        //{
        //    seq.timeScale = 1f;
        //});

        seq.Append(dieCanvas.DOFade(0f, 1f));
        seq.AppendCallback(() => dieCanvas.gameObject.SetActive(false));
    }
}
