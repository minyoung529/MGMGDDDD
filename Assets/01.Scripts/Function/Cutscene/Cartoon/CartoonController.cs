using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class CartoonController : MonoBehaviour
{
    private CartoonPlayer[][] cartoonPlayers;
    private GameObject[] cartoons;

    [SerializeField]
    private bool playOnAwake = false;

    [SerializeField]
    private UnityEvent onClear;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private bool skipPlay = false;

    private void Awake()
    {
        cartoonPlayers = new CartoonPlayer[transform.childCount][];
        cartoons = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            cartoons[i] = transform.GetChild(i).gameObject;
            cartoons[i].gameObject.SetActive(true);
            cartoonPlayers[i] = cartoons[i].GetComponentsInChildren<CartoonPlayer>();
            cartoons[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        if (playOnAwake)
        {
            if (skipPlay)
            {
                onClear?.Invoke();
            }
            else
            {
                Play();
            }
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        CutSceneManager.Instance.EnterCutscene();

        for (int i = 0; i < cartoonPlayers.Length; i++)
        {
            cartoons[i].gameObject.SetActive(true);

            for (int j = 0; j < cartoonPlayers[i].Length; j++)
                cartoonPlayers[i][j].ReadyToPlay();

            for (int j = 0; j < cartoonPlayers[i].Length; j++)
            {
                cartoonPlayers[i][j].Play();
                yield return new WaitForSeconds(cartoonPlayers[i][j].Duration);
            }

            cartoons[i].gameObject.SetActive(false);
        }

        CutSceneManager.Instance.ExitCutscene();

        canvasGroup?.DOFade(0f, 1f).OnComplete(() =>
        {
            onClear?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
