using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

public enum Chapter
{
    // ���� ���� �����ұ� ��� ��

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
    GameLand = 9,

    // Second Scene
    Maze = 10,

    Count = 11
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

    public ChapterSO GetChapterSO(Chapter chapter) { return chapters[(int)chapter]; }

    protected override void Awake()
    {
        InitChapter();
    }
    private void Start()
    {
        LoadChapter();
    }

    public void ChangeChapter(Chapter change)
    {
        if (SceneController.CurrentScene != GetChapterSO(change).scene)
        {
            SceneController.ChangeScene(GetChapterSO(change).scene, true);
        }
        SetCurChapter(change);
    }

    // Chapter ����
    public void SetCurChapter(Chapter _saveCurChapter)
    {
        curChapter = _saveCurChapter;
        SaveChapter();
    }

    public void SetMaxChapter(Chapter _saveMaxChapter)
    {
        maxClearChapter = _saveMaxChapter;

        SaveChapter();
    }

    public void SetSavePoint(Vector3 _savePosition)
    {
       GetChapterSO(maxClearChapter).savePoint = _savePosition;
    }

    #region Save
    // Data é�� ��������
    private void LoadChapter()
    {
        SaveData loadData = SaveSystem.Load();
        if (loadData != null)
        {
            if(SceneController.CurrentScene != GetChapterSO(loadData.curChapter).scene)
            {
                SceneController.ChangeScene(GetChapterSO(loadData.curChapter).scene, false);
            }
            save = loadData;
            curChapter = save.curChapter;
            maxClearChapter = save.maxChapter;

            EventParam eventParam = new();
            eventParam["pets"] = save.pets;
            eventParam["position"] = GetCurChapter.savePoint;
            EventManager.TriggerEvent(EventName.LoadChapter, eventParam);
        }
        else
        {
            save = new SaveData(Chapter.BasicTutorial, Chapter.BasicTutorial, null);
        }
    }
    // Data é�� �����ϱ�
    private void SaveChapter()
    {
        save = new SaveData(curChapter, maxClearChapter, GetPetTypeList());
        SaveSystem.Save(save);
    }
    private List<PetType> GetPetTypeList()
    {
        List<PetType> typeList = new List<PetType>();
        for (int i = 0; i < PetManager.Instance.GetPetList.Count; i++)
        {
            typeList.Add(PetManager.Instance.GetPetList[i].GetPetType);
        }

        return typeList;
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
