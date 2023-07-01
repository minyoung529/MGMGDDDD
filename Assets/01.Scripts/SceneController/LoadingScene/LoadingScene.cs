using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image fillBar;

    private AsyncOperation operation;
    private bool isChanging;

    private float timer = 0f;
    private float changeDuration = 5f;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public void ChangeScene()
    {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1f, 0.5f).OnComplete(LoadSceneAsync);
        SoundManager.Instance.MuteSound();
    }

    private void LoadSceneAsync()
    {
        operation = SceneManager.LoadSceneAsync(SceneController.CurrentScene.ToString()/*, LoadSceneMode.Additive*/);
        operation.allowSceneActivation = false;
        operation.completed += OnSceneChange;
        isChanging = true;
    }

    private void Update()
    {
        if (!isChanging) return;

        timer += Time.deltaTime;
        if (timer < changeDuration)
        {
            fillBar.fillAmount = Mathf.Lerp(0f, 0.9f, timer / changeDuration);
        }
        else
        {
            if (isChanging)
            {
                fillBar.fillAmount = 1f;
                operation.allowSceneActivation = true;

                isChanging = false;
                operation = null;
                timer = 0f;
            }
        }
    }

    private void OnSceneChange(AsyncOperation op)
    {
        SceneController.ChangeScene(op);
        SoundManager.Instance.LoadVolumeSmooth();

        //SceneManager.UnloadSceneAsync(SceneController.prevScene.ToString());
    }

    public void InactiveScene()
    {
        canvasGroup.DOFade(0f, 0.5f).OnComplete(ResetValue);
    }

    private void ResetValue()
    {
        SoundManager.Instance.LoadVolumeSmooth();
        fillBar.fillAmount = 0f;
        gameObject.SetActive(false);
    }
}
