using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image fillBar;

    private AsyncOperation operation;
    private bool isChanging;

    private float timer = 0f;
    private float changeDuration = 5f;

    public void ChangeScene()
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
        //SceneManager.UnloadSceneAsync(SceneController.prevScene.ToString());
    }
}
