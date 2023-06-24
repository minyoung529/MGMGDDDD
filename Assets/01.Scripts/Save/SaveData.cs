using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public SaveData(Chapter _chapter, Chapter _maxChapter)
    {
        curChapter = _chapter;
        maxChapter = _maxChapter;

        masterVolume = 1f;
        bgmVolume = 1f;
        sfxVolume = 1f;
        hSensitivity = 4f;
        vSensitivity = 2.5f;
    }

    public Chapter curChapter;
    public Chapter maxChapter;
    public List<PetType> pets;

    #region SETTING
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
    public float hSensitivity = 1f;
    public float vSensitivity = 1f;
    #endregion
}