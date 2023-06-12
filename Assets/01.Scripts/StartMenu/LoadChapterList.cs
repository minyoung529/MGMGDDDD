using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadChapterList : MonoBehaviour
{
    [SerializeField] private Image titleImage;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform content;
    private Chapter selectChapter = Chapter.BasicTutorial;

  private List<Button> chapterButtons = new List<Button>();

    private void Start()
    {
        SettingChapterButton();
    }

    private void GoChapterScene(Chapter change)
    {
        SceneController.ChangeScene(ChapterManager.Instance.GetChapterSO(change).scene, true);
    }
    private void GoCurChapterScene()
    {
        SceneController.ChangeScene(ChapterManager.Instance.GetCurChapter.scene, true);
    }

    public void SettingChapterButton()
    {
        for(int i=0; i<content.childCount;i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        chapterButtons.Clear();

        for (int i = 0; i < (int)ChapterManager.Instance.CurChapter; i++)
        {
            Chapter chapter = (Chapter)i;
            Button button = Instantiate(buttonPrefab, content).GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                LoadChapterData(button, chapter);
            });
            chapterButtons.Add(button);

            TextMeshProUGUI chapterNameText = button.GetComponentInChildren<TextMeshProUGUI>();
            chapterNameText.SetText(chapter.ToString());
            button.gameObject.SetActive(true);
        }
    }

    private void LoadChapterData(Button button, Chapter chapter)
    {
        selectChapter = chapter;
        titleImage.sprite = ChapterManager.Instance.GetChapterSO(chapter).chapterTitleImage;
    }

    public void PlayChapter()
    {
    //    SceneController.ListeningEnter(SceneType.LivingRoom, ChapterManager.Instance.SaveChapter);
        SceneController.ListeningEnter(SceneType.LivingRoom, ChapterManager.Instance.SetLoadGame);
        SceneController.ListeningEnter(SceneType.Lobby_FirstFloor, ChapterManager.Instance.SetLoadGame);
        SceneController.ListeningEnter(SceneType.Clock_Lobby, ChapterManager.Instance.SetLoadGame);
     
        ChapterManager.Instance.SetCurChapter(selectChapter);
        GoChapterScene(selectChapter);
    }

    public void LoadGame()
    {
        SceneController.ListeningEnter(SceneType.LivingRoom, ChapterManager.Instance.SetLoadGame);
        SceneController.ListeningEnter(SceneType.Lobby_FirstFloor, ChapterManager.Instance.SetLoadGame);
        SceneController.ListeningEnter(SceneType.Clock_Lobby, ChapterManager.Instance.SetLoadGame);

        GoCurChapterScene();
    }
}
