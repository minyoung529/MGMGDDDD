using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;

public enum Chapter
{
    // First scene
    BasicTutorial = 0,
    FireTutorial = 1,
    OilTutorial = 2,
    StickyTutorial = 3,

    // Clock_Lobby Scene
    TicketBox = 4,
    Cube = 5,
    Cinema = 6,
    Cannon = 7,
    Balance = 8,
    Line = 9,
    Dropper = 10,

    // Second Scene
    Maze = 11,

    Count = 12
}
public class ChapterManager : MonoSingleton<ChapterManager>
{

    [SerializeField] List<ChapterSO> chapters;

    private Chapter curChapter;
    private Chapter maxClearChapter;

    public ChapterSO GetCurChapter { get { return chapters[(int)curChapter]; } }

    private SaveData save;
    private const string saveFileName = "Save";

    public Chapter CurChapter { get { return curChapter; } }
    public Chapter CurMaxChapter { get { return curChapter; } }

    private void Start()
    {
        InitChapter();
        LoadChapter();
    }

    // Chapter 갱신
    public void SetCurChapter(Chapter _saveCurChapter)
    {
        curChapter = _saveCurChapter;
        SaveChapter();
    }

    public void SetMaxChapter(Chapter _saveMaxChapter, Vector3 _savePosition)
    {
        maxClearChapter = _saveMaxChapter;
        chapters[(int)maxClearChapter].savePoint = _savePosition;

        SaveChapter();
    }

    #region Save
    // Data 챕터 가져오기
    private void LoadChapter()
    {
        SaveData loadData = SaveSystem.Load();
        if (loadData != null)
        {
            save = loadData;
            curChapter = save.curChapter;
            maxClearChapter = save.maxChapter;

            EventParam eventParam = new();
            eventParam["position"] = chapters[(int)curChapter].savePoint;
            EventManager.TriggerEvent(EventName.PlayerRespawn, eventParam);
            eventParam["pets"] = loadData.pets;
            EventManager.TriggerEvent(EventName.LoadChapter, eventParam);
        }
        else
        {
            save = new SaveData(Chapter.BasicTutorial, Chapter.BasicTutorial, PetManager.Instance.GetPetList);
        }
    }
    // Data 챕터 저장하기
    private void SaveChapter()
    {
        save = new SaveData(curChapter, maxClearChapter, PetManager.Instance.GetPetList);
        SaveSystem.Save(save);
    }
    #endregion

    #region Init Chapter
    int compare(ChapterSO a, ChapterSO b)
    {
        return (int)a.chapterName < (int)b.chapterName ? -1 : 1;
    }
    private void InitChapter()
    {
        chapters.Sort(compare);
    }
    #endregion

    #region Quit
    private void OnApplicationQuit()
    {
        SaveChapter();
    }

    #endregion
}
