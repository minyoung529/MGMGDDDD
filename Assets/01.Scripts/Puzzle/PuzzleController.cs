using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private Chapter chapter;
    [SerializeField] private List<CutScenePlayer> cutscenes;
    [SerializeField] private UnityEvent onLoadEvent;

    private void Awake()
    {
        EventManager.StartListening(EventName.LoadChapter, LoadPuzzle);
    }
    private void LoadPuzzle(EventParam eventParam = null)
    {
        ChapterManager.Instance.LoadChapter();
        if ((int)ChapterManager.Instance.CurChapter < (int)chapter) return;

        onLoadEvent?.Invoke();
        if ((int)ChapterManager.Instance.CurChapter > (int)chapter)
        {
            for (int i = 0; i < cutscenes.Count; i++)
            {
                cutscenes[i].SetHasplayed(true);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.LoadChapter, LoadPuzzle);
    }
}
