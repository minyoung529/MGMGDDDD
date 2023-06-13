using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Chapter", menuName = "ScriptableObjects/Chapter")]
public class ChapterSO : ScriptableObject
{
    public string chapterNumber;
    public SceneType scene;
    public Chapter chapterName;
    public Sprite chapterTitleImage;
    public Vector3 savePoint = Vector3.zero;

}
