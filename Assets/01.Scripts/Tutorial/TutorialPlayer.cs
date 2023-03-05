using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class TutorialPlayer : MonoBehaviour
{
    [SerializeField] TutorialType tutorialType;
    [SerializeField] private bool isStart = false;      // 처음부터 시작인가
    [SerializeField] private bool isOnce = true;        // 한 번만 실행되는가
    [SerializeField] private float removeDelayTime = 2.2f;

    [SerializeField] private Animator animator;

    [SerializeField]
    private List<AutoInputPanel> autoPanelList = new List<AutoInputPanel>();

    [SerializeField]
    private List<CanvasGroup> autoGroups = new List<CanvasGroup>();

    private int autoIdx = 0;

    private bool isChainging = false;
    private bool isStarted = false;     // 튜토리얼이 시작했는가
    private bool isFinish = false;      // 튜토리얼이 끝났는가

    #region PROPERTY
    public Animator Animator { set => animator = value; }
    private int ActiveCount => autoPanelList.Count(x => x.Active);
    private int SuccessCount => autoPanelList.Count(x => x.Success && x.Active);
    private bool IsValidIdx => autoIdx >= 0 && autoIdx < autoGroups.Count;
    public TutorialType TutorialType => tutorialType;
    #endregion

    private void Start()
    {
        if (isStart)
            StartTutorial();

        //SceneController.ListningEnter(SceneType.NewClock_QU, Init);
    }

    private void Update()
    {
        if (!isStarted) return;
        if (isFinish) return;

        if (SuccessCount != 0 && !isChainging && SuccessCount == ActiveCount)
        {
            ShowNextTutorial();
        }
    }

    private void ShowNextTutorial()
    {
        Sequence seq = DOTween.Sequence();

        if (autoGroups.Count >= autoIdx) return;

        CanvasGroup curGroup = autoGroups[autoIdx];
        isChainging = true;

        ++autoIdx;

        seq.AppendInterval(removeDelayTime);
        seq.Append(RemovePreviusPanel(curGroup));
        seq.AppendCallback(() =>
        {
            if (IsValidIdx)
            {
                seq.Append(ShowCurrentPanel());
            }
            else
            {
                StopTutorial();
            }
        });
    }

    private Tweener RemovePreviusPanel(CanvasGroup canvas)
    {
        return canvas.DOFade(0f, 0.3f).OnComplete(() => canvas.gameObject.SetActive(false));
    }

    private Tweener ShowCurrentPanel(string name = "")
    {
        CanvasGroup canvasGroup = autoGroups[autoIdx];
        canvasGroup.DOFade(0f, 0f);
        canvasGroup.gameObject.SetActive(true);

        foreach (AutoInputPanel panel in autoPanelList)
        {
            if (IsCurrentPanel(canvasGroup.transform, panel.transform))
                panel.SetName(name);
        }

        return canvasGroup.DOFade(1f, 0.6f).OnComplete(() =>
        {
            foreach (AutoInputPanel panel in autoPanelList)
            {
                if (IsCurrentPanel(canvasGroup.transform, panel.transform))
                {
                    panel.Active = true;
                }
                else
                {
                    panel.Active = false;
                }
            }

            isChainging = false;
        });
    }

    private bool IsCurrentPanel(Transform parent, Transform child)
    {
        if (child == parent) return true;

        while (child != null)
        {
            child = child.transform.parent;
            if (child == parent) return true;
        }

        return false;
    }

    [ContextMenu("Start Tutorial")]
    public void StartTutorial(string name = "")
    {
        if (isOnce && (isStarted || isFinish)) return;
        if (!isOnce)    // 반복 실행이라면 데이터 초기화
        {
            autoIdx = 0;
            isFinish = false;
        }


        ShowCurrentPanel(name);
        isStarted = true;
    }

    public void StopTutorial()
    {
        if (isFinish) return;

        foreach (CanvasGroup canvas in autoGroups)
        {
            canvas.DOFade(0f, 0.3f).OnComplete(() => canvas.gameObject.SetActive(false));
        }

        autoPanelList.ForEach(x => x.ResetData());

        isFinish = true;
    }

    public void Init()
    {
        foreach (AutoInputPanel panel in autoPanelList)
        {
            panel.Init(animator);
        }
    }

    private void OnDestroy()
    {
        //SceneController.StopListningEnter(SceneType.NewClock_QU, Init);
    }
}
