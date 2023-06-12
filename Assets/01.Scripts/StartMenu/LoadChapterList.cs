using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadChapterList : MonoBehaviour
{
    [SerializeField] Image titleImage;
    private GameObject buttonPrefab;
    private Chapter selectChapter = Chapter.BasicTutorial;

    private void Awake()
    {
        buttonPrefab = transform.GetChild(0).gameObject;
        buttonPrefab?.gameObject.SetActive(false);
    }

    private void Start()
    {
        SettingChapterButton();
    }

    private void SettingChapterButton()
    {
        if (buttonPrefab == null)
        {
            Debug.LogError("ButtonPrefab을 찾을 수 없습니다.");
            return;
        }

        for (int i = 0; i < (int)ChapterManager.Instance.CurChapter; i++)
        {
            Chapter chapter = (Chapter)i;
            Button button = Instantiate(buttonPrefab, transform).GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                LoadChapterData(button, chapter);
            });

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
        ChapterManager.Instance.SetCurChapter(selectChapter);
        ChapterManager.Instance.GoChapterScene(selectChapter);
    }
}
