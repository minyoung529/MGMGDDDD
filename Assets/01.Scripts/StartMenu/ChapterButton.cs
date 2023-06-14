using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ChapterProgressType
{
    Lock,
    Clear,
    Progress,
    Count
}

public class ChapterButton : MonoBehaviour
{
    [SerializeField]
    private GameObject[] chapterActiveList;

    [SerializeField]
    private Button button;

    [SerializeField]
    private ChapterUITween tween;
    public ChapterUITween Tween => tween;

    private LoadChapterList loadChapterList;
    private Chapter chapter;

    public void ChangeState(ChapterProgressType type)
    {
        for (int i = 0; i < (int)ChapterProgressType.Count; i++)
        {
            bool isSelected = i == (int)type;
            chapterActiveList[i].SetActive(isSelected);
        }
    }

    public ChapterButton Init(Chapter chapter, LoadChapterList loadChapterList)
    {
        this.loadChapterList = loadChapterList;
        this.chapter = chapter;
        button.onClick.AddListener(OnClick);

        TextMeshProUGUI chapterNameText = button.transform.Find("ChapterNameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI chapterNumberText = button.transform.Find("ChapterNumberText").GetComponent<TextMeshProUGUI>();
        chapterNameText.SetText(chapter.ToString());
        chapterNumberText.SetText(ChapterManager.Instance.GetChapterSO(chapter).chapterNumber);
        button.gameObject.SetActive(true);

        return this;
    }

    private void OnClick()
    {
        loadChapterList.LoadChapterData(this, chapter);
    }
}
