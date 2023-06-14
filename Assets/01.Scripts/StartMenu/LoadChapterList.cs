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

    private List<PetType> EnumToListPetType(PetType flag)
    {
        List<PetType> petList = new List<PetType>();

        if ((flag & PetType.FirePet) != 0)
        {
            petList.Add(PetType.FirePet);
        }
        if ((flag & PetType.OilPet) != 0)
        {
            petList.Add(PetType.OilPet);
        }
        if ((flag & PetType.StickyPet) != 0)
        {
            petList.Add(PetType.StickyPet);
        }

        return petList;
    }

    private void Awake()
    {
        titleName = titleParent.Find("TitleNameText").GetComponent<TextMeshProUGUI>();
        titleImage = titleParent.Find("TitleImage").GetComponent<Image>();
    }
    private void Start()
    {
        InitChapterButton();
    }

    private void GoChapterScene(Chapter change)
    {
        SceneController.ChangeScene(ChapterManager.Instance.GetChapterSO(change).scene, true);
    }
    private void GoCurChapterScene()
    {
        SceneController.ChangeScene(ChapterManager.Instance.GetCurChapter.scene, true);
    }

    private void InitChapterButton()
    {
        for (int i = 0; i < (int)Chapter.Count; i++)
        {
            ChapterButton chapterButton = Instantiate(buttonPrefab, content);
            Chapter chapter = (Chapter)i;
            chapterButton.Init(chapter, () => LoadChapterData(chapterButton, chapter));

            chapterButtons.Add(chapterButton);
            chapters.Add(chapterButton.transform.parent.gameObject);
        }

        SceneController.ListeningEnter(SceneType.StartScene, ()=>SaveSystem.Load());

        SceneController.ListeningEnter(SceneType.LivingRoom, ChapterManager.Instance.SetLoadGame);
        SceneController.ListningExit(SceneType.LivingRoom, () => SaveSystem.Save(SaveSystem.CurSaveData));
        SceneController.ListeningEnter(SceneType.Lobby_FirstFloor, ChapterManager.Instance.SetLoadGame);
        SceneController.ListningExit(SceneType.Lobby_FirstFloor, () => SaveSystem.Save(SaveSystem.CurSaveData));
        SceneController.ListeningEnter(SceneType.Clock_Lobby, ChapterManager.Instance.SetLoadGame);
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

    private void LoadChapterData(ChapterButton button, Chapter chapter)
    {
        selectChapter = chapter;
        curSelectButton?.Tween.UnSelect();
        curSelectButton = button;
        curSelectButton?.Tween.Select();
        // titleImage.preserveAspect = true;
        titleName.SetText(chapter.ToString());
        titleImage.sprite = ChapterManager.Instance.GetChapterSO(chapter).chapterTitleImage;
    }

    public void PlayChapter()
    {
        SaveSystem.CurSaveData.pets = EnumToListPetType(ChapterManager.Instance.GetChapterSO(selectChapter).pets);
        ChapterManager.Instance.SetCurChapter(selectChapter);
        SaveSystem.Save(SaveSystem.CurSaveData);

        GoChapterScene(selectChapter);
    }

    public void LoadGame()
    {
        GoCurChapterScene();
        ChapterManager.Instance.SaveChapter();
    }
}
