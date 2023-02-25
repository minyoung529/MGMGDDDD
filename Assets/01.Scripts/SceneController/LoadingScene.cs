using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Text loadingText;

    private void Start()
    {
        
    }

    private void LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneController.CurrentScene.ToString());
    }
}
