using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneController.ChangeScene(ChapterManager.Instance.GetCurChapterSO.scene, true);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying= false;
#endif
    }
}
