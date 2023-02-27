using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image fillBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneController.CurrentScene.ToString());
        operation.allowSceneActivation = false;

        float timer = 0f;
        operation.completed += SceneController.ChangeScene;

        while (!operation.isDone)
        {
            yield return null;

            if (operation.progress < 0.9f)
            {
                fillBar.fillAmount = operation.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                fillBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if (fillBar.fillAmount >= 0.99f)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
