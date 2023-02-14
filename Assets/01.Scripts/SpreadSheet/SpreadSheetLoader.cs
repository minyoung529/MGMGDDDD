using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

#if UNITY_EDITOR

public class SpreadSheetLoader : EditorWindow
{
    private string _DocumentID = "1JAXeIqWWtDhW6cLZv3HybynBfQRYuYXHqJin0XMiGkY";
    
    [MenuItem("Tools/SpreadSheetLoader")]
    public static void OpenSpreadSheetLoader()
    {
        EditorWindow wnd = GetWindow<SpreadSheetLoader>();
        wnd.titleContent = new GUIContent("SpreadsheetLoader");
    }

    private void CreateGUI()
    {
        Debug.Log("a");
        VisualTreeAsset tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editors/UI/ExcelSheetSyncWindow.uxml");
        VisualElement ui = tree.Instantiate();

        rootVisualElement.Add(ui);

        AddEvent(ui);
    }


    private void AddEvent(VisualElement ui)
    {
        //??? ????? ???? ??? ????????
        Button loadBtn = ui.Q<Button>("btn-load");
        TextField txtUrl = ui.Q<TextField>("txt-url");
        txtUrl.SetValueWithoutNotify(_DocumentID); //??????? ?? ???? ????? ????

        //loadBtn.RegisterCallback<ClickEvent>(evt =>
        //{
        //    ui.Q("popup").RemoveFromClassList("off");
        //    EditorCoroutineUtility.StartCoroutine(GetDataFromSheet("0", (dataArr)=>
        //    {
        //        CreateSourceCode(dataArr[0], dataArr[1], dataArr[2]);
        //    }), this);

        //    EditorCoroutineUtility.StartCoroutine(GetDataFromSheet("1844013295", (dataArr) =>
        //    {
        //        CreateScriptAbleObject(
        //            name:dataArr[0], 
        //            hp: int.Parse(dataArr[1]),
        //            dex: int.Parse(dataArr[2]), 
        //            critical: float.Parse(dataArr[3]) );
        //    }), this);
        //});

        txtUrl.RegisterCallback<ChangeEvent<string>>(evt =>
        {
            _DocumentID = evt.newValue; // ????? ?????? ?????? ????
        });  
    }

    IEnumerator GetDataFromSheet(string sheetID, Action<string[]> Processs)
    {
        Label statusLbl = rootVisualElement.Q<Label>("status-label");
        UnityWebRequest www = UnityWebRequest.Get(
                $"https://docs.google.com/spreadsheets/d/{_DocumentID}/export?format=tsv&gid={sheetID}");
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.ConnectionError || www.responseCode != 200)
        {
            statusLbl.text += $"{sheetID} : ??????? ?? ???? ???";
            rootVisualElement.Q("popup").AddToClassList("off");
            yield break;
        }

        string fileText = www.downloadHandler.text;
        statusLbl.text = $"{sheetID} : ???? ???. ???? ?????? ??? ????";
        yield return null; //?????? ui?? ????? ???? ???

        string[] lines = fileText.Split("\n");

        //u???? ???? ?????? ???? ????
        int lineNum = 1;
        try
        {
            for (lineNum = 1; lineNum < lines.Length; lineNum++)
            {
                string[] dataArr = lines[lineNum].Split("\t");
                Processs(dataArr);
            }
        }
        catch (Exception e)
        {
            statusLbl.text += $"\n {sheetID} : ???? ????? ???? ??? : ?u???? ???? ??";
            statusLbl.text += $"\n {sheetID} : {lineNum} ???????? ????";
            statusLbl.text += $"\n{e.Message}";
            
            yield break;
        } finally
        {
            rootVisualElement.Q("popup").AddToClassList("off");
        }

        statusLbl.text += $"\n {sheetID} ?????? {lineNum-1} ???? ?????? ?????????? ??????";
    }

    private void CreateSourceCode(string name, string className, string type)
    {
        string code = string.Format(CodeFormat.CharFormat, className, name, type);
        string path = $"{Application.dataPath}/Scripts/Character/";
        File.WriteAllText($"{path}{className}.cs", code);
    }

    //private void CreateScriptAbleObject(string name, int hp, int dex, float critical)
    //{
    //    CharDataSO asset;

    //    asset = AssetDatabase.LoadAssetAtPath<CharDataSO>($"Assets/SO/{name}.asset");
    //    if(asset == null)
    //    {
    //        asset = ScriptableObject.CreateInstance<CharDataSO>();
    //        string filename = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/SO/{name}.asset");
    //        AssetDatabase.CreateAsset(asset, filename);
    //    }

    //    asset.maxHP = hp;
    //    asset.dex = dex;
    //    asset.critical = critical;

    //    AssetDatabase.SaveAssets();
    //}
}

#endif