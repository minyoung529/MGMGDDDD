using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

public enum Chapter
{
    // 씬을 어케 구분할까 고민 중

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
    private SaveData save = null;

    private Chapter curChapter;
    private Chapter maxClearChapter;

    public Chapter CurChapter { get { return curChapter; } }
    public Chapter CurMaxChapter { get { return curChapter; } }

    public ChapterSO GetCurChapter { get { return chapters[(int)curChapter]; } }
    public ChapterSO GetChapterSO(Chapter chapter) { return chapters[(int)chapter]; }

    protected override void Awake()
    {
        SceneController.ListeningEnter(SceneType.Lobby_FirstFloor, SetLoadGame);
        SceneController.ListeningEnter(SceneType.Clock_Lobby, SetLoadGame);
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
    public void GoChapterScene(Chapter change)
    {
        SceneController.ChangeScene(GetChapterSO(change).scene, true);
    }
    public void GoCurChapterScene()
    {
        SceneController.ChangeScene(GetCurChapter.scene,  true);
    }

    // Chapter 갱신
    public void SetCurChapter(Chapter _saveCurChapter)
    {
        if (curChapter == _saveCurChapter) return;
        curChapter = _saveCurChapter;
        Debug.Log(curChapter);
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
            save = new SaveData(Chapter.BasicTutorial, Chapter.BasicTutorial, null);
        }
    }

    // Data 챕터 저장하기
    private void SaveChapter()
    {
        if (save == null) return;
        save = new SaveData(curChapter, maxClearChapter, GetPetTypeList());
        SaveSystem.Save(save);
    }
    private void SetLoadGame()
    {
        if (save == null) return;

        EventParam eventParam = new();
        if(save.pets != null) eventParam["pets"] = save.pets;
        eventParam["position"] = GetCurChapter.savePoint;
        EventManager.TriggerEvent(EventName.LoadChapter, eventParam);
    }
    #endregion

    #region Init Chapter
    private List<PetType> GetPetTypeList()
    {
        if (PetManager.Instance == null) return null;
        List<PetType> typeList = new List<PetType>();
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

}
