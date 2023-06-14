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
   
    public Chapter CurChapter {
        get 
        {
            return SaveSystem.CurSaveData.curChapter;
        } 
        set
        {
            SaveSystem.CurSaveData.curChapter = value;
            if (CurChapter > MaxChapter) MaxChapter = CurChapter;
        } 
    }
    public Chapter MaxChapter { 
        get 
        {
            return SaveSystem.CurSaveData.maxChapter;
        }
        set
        {
            SaveSystem.CurSaveData.maxChapter = value;
        }
    }
    public ChapterSO GetCurChapterSO { get { return chapters[(int)CurChapter]; } }
    public ChapterSO GetChapterSO(Chapter chapter) { return chapters[(int)chapter]; }
    int compare(ChapterSO a, ChapterSO b) { return (int)a.chapterName < (int)b.chapterName ? -1 : 1; }
    private List<PetType> GetPetTypeList()
    {
        List<PetType> typeList = new List<PetType>();
        if (PetManager.Instance == null)
        {
            return SaveSystem.CurSaveData.pets;
        }
        for (int i = 0; i < PetManager.Instance.GetPetList.Count; i++)
        {
            typeList.Add(PetManager.Instance.GetPetList[i].GetPetType);
        }
        return typeList;
    }
    private List<PetType> EnumToListPetType(PetType flag)
    {
        List<PetType> petList = new List<PetType>();

        if ((flag & PetType.FirePet) != 0)
        {
            petList.Add(PetType.FirePet);
        }
        if ((flag & PetType.OilPet) != 0)
        {
            petList.Add(PetType.OilPet);
        }
        if ((flag & PetType.StickyPet) != 0)
        {
            petList.Add(PetType.StickyPet);
        }

        return petList;
    }

    protected override void Awake()
    {
        base.Awake();
        chapters.Sort(compare);
    }

    [ContextMenu("Reset")]
    public void ResetData()
    {
        SaveSystem.ResetData();
    }

    public void SetSavePoint(SavePoint point)
    {
        SaveChapter(point.Chapter);
    }


    public void LoadGame()
    {
        if (SaveSystem.CurSaveData == null) return;
        EventParam eventParam = new();
        
        eventParam["pets"] = SaveSystem.CurSaveData.pets;
        eventParam["position"] = GetCurChapterSO.savePoint;

        EventManager.TriggerEvent(EventName.LoadChapter, eventParam);
    }

    public void SaveChapter(Chapter chapter)
    {
        CurChapter = chapter;
    }
    public void SaveChapter(int chapter)
    {
        CurChapter = (Chapter)chapter;
    }

    public void SavePets()
    {
        SaveData saveData = SaveSystem.CurSaveData;
        saveData.pets = GetPetTypeList();
        SaveSystem.CurSaveData = saveData;
    }

    public void SaveEnumToListPet(Chapter chapter)
    {
        SaveSystem.CurSaveData.pets = EnumToListPetType(GetChapterSO(chapter).pets);
    }

}
