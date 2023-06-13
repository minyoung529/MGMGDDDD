using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Drawing;

public enum Chapter
{
    // Living Room Scene
    LivingRoom = 0,

    // First scene
    BasicTutorial = 1,
    FireTutorial = 2,
    StickyTutorial = 3,
    OilTutorial = 4,

    // Clock_Lobby Scene
    TicketBox = 5,
    Cube = 6,
    Cinema = 7,
    Cannon = 8,
    Balance = 9,
    GameLand = 10,

    // Second Scene
    Maze = 11,

    Count = 12
}
public class ChapterManager : MonoSingleton<ChapterManager>
{
    [SerializeField] List<ChapterSO> chapters;
    private SaveData saveData;

    private Chapter curChapter;
    private Chapter maxClearChapter;

    public Chapter CurChapter { get { return curChapter; } }

    public ChapterSO GetCurChapter { get { return chapters[(int)curChapter]; } }
    public ChapterSO GetChapterSO(Chapter chapter) { return chapters[(int)chapter]; }

    protected override void Awake()
    {
        InitChapter();
    }

    [ContextMenu("Reset")]
    public void ResetData()
    {
        SaveSystem.ResetData();
    }

    #region Set

    // Chapter 갱신
    public void SetCurChapter(Chapter chapter)
    {
        curChapter = chapter;
        if (maxClearChapter < curChapter) maxClearChapter = curChapter;
        SaveChapter();
    }
    public void SetSavePoint(SavePoint point)
    {
        GetChapterSO(point.Chapter).savePoint = point.transform.position;
        SetCurChapter(point.Chapter);
    }
    #endregion


    #region Save
    public void SetLoadGame()
    {
        if (SaveSystem.CurSaveData == null) return;

        EventParam eventParam = new();
        if (SaveSystem.CurSaveData.pets != null)
        {
            if(SaveSystem.CurSaveData.curChapter != Chapter.LivingRoom)
            eventParam["pets"] = SaveSystem.CurSaveData.pets;
        }
            eventParam["position"] = GetCurChapter.savePoint;
        EventManager.TriggerEvent(EventName.LoadChapter, eventParam);

        SceneController.StopListeningEnter(SetLoadGame);
    }

    public void SaveChapter()
    {
        SaveSystem.CurSaveData.pets = GetPetTypeList();
        SaveSystem.CurSaveData.maxChapter = maxClearChapter;
        SaveSystem.CurSaveData.curChapter = curChapter;
    }

    // Data 챕터 가져오기
    public void LoadChapter()
    {
        SaveData loadData = SaveSystem.Load();
        if (loadData != null)
        {
            SaveSystem.CurSaveData = loadData;
            curChapter = SaveSystem.CurSaveData.curChapter;
            maxClearChapter = SaveSystem.CurSaveData.maxChapter;
        }
    }
 
    #endregion

    #region Get
    private List<PetType> GetPetTypeList()
    {
        List<PetType> typeList = new List<PetType>();
        if(PetManager.Instance == null)
        {
            return SaveSystem.CurSaveData.pets;
        }

        for (int i = 0; i < PetManager.Instance.GetPetList.Count; i++)
        {
            typeList.Add(PetManager.Instance.GetPetList[i].GetPetType);
        }
        return typeList;
    }

    int compare(ChapterSO a, ChapterSO b)
    {
        return (int)a.chapterName < (int)b.chapterName ? -1 : 1;
    }
    private void InitChapter()
    {
        chapters.Sort(compare);

        LoadChapter();
    }
    #endregion
}
