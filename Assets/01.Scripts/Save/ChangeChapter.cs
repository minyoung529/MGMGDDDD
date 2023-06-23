using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChapter : MonoBehaviour
{
    [SerializeField] private Chapter chapter;

    public void Change()
    {
        ChapterManager.Instance.SaveChapter((int)chapter);
    }
}
