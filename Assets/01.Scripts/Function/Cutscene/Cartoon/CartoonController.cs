using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Threading;

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

    IEnumerator Start()
    {
        InputManager.StartListeningInput(InputAction.Interaction, Clear);
        yield return null;
        
        if (playOnAwake)
        {
            if (skipPlay)
            {
                gameObject.SetActive(false);
                canvasGroup.gameObject.SetActive(false);
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
        SoundManager.Instance.LoadVolumeSmooth();

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

        Clear();
    }

    public void Clear(InputAction action = InputAction.Interaction, float value = 0f)
    {
        CutSceneManager.Instance.ExitCutscene();

        canvasGroup?.DOFade(0f, 1f).OnComplete(() =>
        {
            onClear?.Invoke();
            gameObject.SetActive(false);
        });

        InputManager.StopListeningInput(InputAction.Interaction, Clear);
    }
}
