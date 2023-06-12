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
    OilTutorial = 3,
    StickyTutorial = 4,

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
    private SaveData save = null;

    private Chapter curChapter;
    private Chapter maxClearChapter;

    public Chapter CurChapter { get { return curChapter; } }
    public Chapter CurMaxChapter { get { return curChapter; } }

    public ChapterSO GetCurChapter { get { return chapters[(int)curChapter]; } }
    public ChapterSO GetChapterSO(Chapter chapter) { return chapters[(int)chapter]; }

    protected override void Awake()
    {
        InitChapter();
    }
    private void Start()
    {
        LoadChapter();
    }

    [ContextMenu("Reset")]
    public void ResetData()
    {
        save = null;
        SaveSystem.ResetData();
    }

    #region Set

    // Chapter 갱신
    public void SetSavePoint(SavePoint point)
    {
        curChapter = point.Chapter;
        GetCurChapter.savePoint = point.transform.position;
        if(maxClearChapter < curChapter) maxClearChapter = curChapter;
    }
    public void SetCurChapter(Chapter chapter)
    {
        curChapter = chapter;
        if (maxClearChapter < curChapter) maxClearChapter = curChapter;
    }
    #endregion

    #region Save
    public void SetLoadGame()
    {
        if (save == null) return;

        EventParam eventParam = new();
        if (save.pets != null) eventParam["pets"] = save.pets;
        eventParam["position"] = GetCurChapter.savePoint;
        EventManager.TriggerEvent(EventName.LoadChapter, eventParam);

        SceneController.StopListeningEnter(SetLoadGame);
    }

    // Data 챕터 가져오기
    public void LoadChapter()
    {
        SaveData loadData = SaveSystem.Load();
        if (loadData != null)
        {
            save = loadData;
            curChapter = save.curChapter;
            maxClearChapter = save.maxChapter;
        }
        else
        {
            save = new SaveData(0, 0, new List<PetType>());
        }
    }

    // Data 챕터 저장하기
    public void SaveChapter()
    {
        if (save == null) return;

        SaveSystem.Load();
        save = new SaveData(curChapter, maxClearChapter, GetPetTypeList());
        SaveSystem.Save(save);
    }
 
    #endregion

    #region Get
    private List<PetType> GetPetTypeList()
    {
        List<PetType> typeList = new List<PetType>();
        if(PetManager.Instance == null)
        {
            return save.pets;
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
    }
    #endregion

    private void OnApplicationQuit()
    {
        SaveChapter();
    }
}
