using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public SaveData(Chapter _chapter, Chapter _maxChapter, List<Pet> _pets)
    {
        curChapter = _chapter;
        maxChapter = _maxChapter;
        pets = _pets;
    }
    
    public Chapter curChapter;
    public Chapter maxChapter;
    public List<Pet> pets;
}