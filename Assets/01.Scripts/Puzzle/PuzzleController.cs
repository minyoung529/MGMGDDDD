using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
        if ((int)ChapterManager.Instance.CurChapter < (int)chapter) return;
        if ((int)ChapterManager.Instance.CurChapter > (int)chapter)
        {
            for (int i = 0; i < cutscenes.Count; i++)
            {
                cutscenes[i].SetHasplayed(true);
            }
        }
        onLoadEvent?.Invoke();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.LoadChapter, LoadPuzzle);
    }
}
