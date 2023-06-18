using Bitgem.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadChapterList : MonoBehaviour
{
    [SerializeField] private Transform titleParent;
    [SerializeField] private ChapterButton buttonPrefab;
    [SerializeField] private Transform content;
    private Chapter selectChapter = 0;

    private TextMeshProUGUI titleName;
    private Image titleImage;

    private List<ChapterButton> chapterButtons = new List<ChapterButton>();
    private List<GameObject> chapters = new List<GameObject>();

    private ChapterButton curSelectButton;

    private void Awake()
    {
        titleName = titleParent.Find("TitleNameText").GetComponent<TextMeshProUGUI>();
        titleImage = titleParent.Find("TitleImage").GetComponent<Image>();
    }

    private void Start()
    {
        InitChapterButton();
    }

    private void GoCurChapterScene()
    {
        SceneController.ChangeScene(ChapterManager.Instance.GetCurChapterSO.scene, true);
    }

    private void InitChapterButton()
    {
        for (int i = 0; i < (int)Chapter.Count; i++)
        {
            ChapterButton chapterButton = Instantiate(buttonPrefab, content);
            Chapter chapter = (Chapter)i;
            chapterButton.Init(chapter, this);

            chapterButtons.Add(chapterButton);
            chapters.Add(chapterButton.transform.parent.gameObject);
        }

        SceneController.ListeningEnter(SceneType.StartScene, () =>
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SaveSystem.Load();
            SaveSystem.CurSaveData.pets = new List<PetType>();
        });

        SceneController.ListeningEnter(SceneType.LivingRoom, ChapterManager.Instance.LoadGame);
        SceneController.ListningExit(SceneType.LivingRoom, () => SaveSystem.Save(SaveSystem.CurSaveData));
        SceneController.ListeningEnter(SceneType.Lobby_FirstFloor, () => { ChapterManager.Instance.LoadGame(); });
        SceneController.ListningExit(SceneType.Lobby_FirstFloor, () => SaveSystem.Save(SaveSystem.CurSaveData));
        SceneController.ListeningEnter(SceneType.Clock_Lobby, ChapterManager.Instance.LoadGame);
        SceneController.ListningExit(SceneType.Clock_Lobby, () => SaveSystem.Save(SaveSystem.CurSaveData));

        SettingChapterButton();
    }

    public void SettingChapterButton()
    {
        for (int i = 0; i < (int)Chapter.Count; i++)
        {
            if ((int)ChapterManager.Instance.MaxChapter < i) // lock
            {
                chapterButtons[i].ChangeState(ChapterProgressType.Lock);
            }
            else if ((int)ChapterManager.Instance.MaxChapter > i) // clear
            {
                chapterButtons[i].ChangeState(ChapterProgressType.Clear);
            }
            else // progress
            {
                chapterButtons[i].ChangeState(ChapterProgressType.Progress);
            }
        }

        LoadChapterData(chapterButtons[0], 0);
    }

    public void LoadChapterData(ChapterButton button, Chapter chapter)
    {
        selectChapter = chapter;
        curSelectButton?.Tween.UnSelect();
        curSelectButton = button;
        curSelectButton?.Tween.Select();

        ChapterSO chapterSO = ChapterManager.Instance.GetChapterSO(chapter);

        titleName.SetText(chapterSO.chapterKoreanName);
        titleImage.sprite = chapterSO.chapterTitleImage;
    }

    public void PlayChapter()
    {
        ChapterManager.Instance.CurChapter = selectChapter;
        ChapterManager.Instance.SaveEnumToListPet(selectChapter);

        SaveSystem.Save(SaveSystem.CurSaveData);
        GoCurChapterScene();
        selectChapter = Chapter.LivingRoom;
    }

    public void LoadGame()
    {
        GoCurChapterScene();
    }
}
