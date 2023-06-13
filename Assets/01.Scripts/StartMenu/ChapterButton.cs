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

    public void ChangeState(ChapterProgressType type)
    {
        for (int i = 0; i < (int)ChapterProgressType.Count; i++)
        {
            bool isSelected = i == (int)type;
            chapterActiveList[i].SetActive(isSelected);
        }
    }

    public ChapterButton Init(Chapter chapter, Action onSelected)
    {
        button.onClick.AddListener(onSelected.Invoke);

        TextMeshProUGUI chapterNameText = button.transform.FindChild("ChapterNameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI chapterNumberText = button.transform.FindChild("ChapterNumberText").GetComponent<TextMeshProUGUI>();
        chapterNameText.SetText(chapter.ToString());
        chapterNumberText.SetText(ChapterManager.Instance.GetChapterSO(chapter).chapterNumber);
        button.gameObject.SetActive(true);

        return this;
    }

}
