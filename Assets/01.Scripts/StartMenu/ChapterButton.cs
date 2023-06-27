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
    private PlaySound clickSound;
    private Chapter chapter;

    private void Awake()
    {
        clickSound = GetComponent<PlaySound>();
    }

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
        ChapterSO chapterSO = ChapterManager.Instance.GetChapterSO(chapter);

        chapterNameText.SetText(chapterSO.chapterKoreanName);
        chapterNumberText.SetText(chapterSO.chapterNumber);
        button.gameObject.SetActive(true);

        return this;
    }

    private void OnClick()
    {
        clickSound.Play();
        loadChapterList.LoadChapterData(this, chapter);
    }
}
