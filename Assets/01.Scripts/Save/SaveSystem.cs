using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string SavePath => Application.dataPath + "/saves/";
    public static SaveData CurSaveData { get; set; }

    public static void ResetData(string saveFileName = "Save")
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            //  Debug.LogError("No such saveFile exists");
            return;
        }
        File.Delete(saveFilePath);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static void Save(SaveData saveData, string saveFileName = "Save")
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);

        string saveFilePath = SavePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static SaveData Load(string saveFileName = "Save")
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            SaveData newData = new (0, 0, new List<PetType>());
            CurSaveData = newData;
            Save(newData);
            return newData;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        CurSaveData = saveData;
        return saveData;
    }
}