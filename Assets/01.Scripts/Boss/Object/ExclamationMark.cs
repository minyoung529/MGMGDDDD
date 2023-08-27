using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    [SerializeField]
    private Transform exclamationMarkCenter;

    [SerializeField]
    private float appearTime = 0.25f;

    [SerializeField]
    private float disappearTime = 0.3f;

    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private Boss boss;

    private Vector3 originalScale;
    private SpriteRenderer renderer;
    private int markPoint = 0;
    private bool isMarking = false;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private string[] questionNames;

    private Color markColor;

    private void Awake()
    {
        EventManager.StartListening(EventName.BossDetectObject, Detect);
    }

    private void Start()
    {
        originalScale = transform.localScale;
        renderer = GetComponent<SpriteRenderer>();
        markColor = renderer.color;
        renderer.color = Color.clear;
    }

    private void Update()
    {
        if (isMarking)
        {
            MarkFixedPosition();
        }
    }

    private void Detect(EventParam param = null)
    {
        if (boss.StateMachine.CurStateIndex == (int)BossStateName.Stun) return;

        markPoint++;

        if (isMarking) return;

        Transform detected = (Transform)param["DetectPosition"];

        if (boss.PrevTarget == detected.name)
        {
            return;
        }

        Debug.Log(boss.PrevTarget + " : " + detected.name);

        if (GetIsQuestionMark(detected.gameObject.name))
        {
            renderer.sprite = sprites[0];   // ?
        }
        else
        {
            renderer.sprite = sprites[1];   // !
        }

        StartCoroutine(ExclamationMarkAnim());
    }

    bool GetIsQuestionMark(string objName)
    {
        foreach (string name in questionNames)
        {
            if (objName.Contains(name))
            {
                return true;
            }
        }

        return false;
    }

    private void MarkFixedPosition()
    {
        Vector3 markCenter = exclamationMarkCenter.position;
        Transform cam = GameManager.Instance.MainCam.transform;
        Vector3 dir = (cam.position - markCenter).normalized; // 보스가 카메라를 보는 방향
        dir.y = 0f;

        transform.position = markCenter + dir * 2f;
        transform.LookAt(cam, Vector3.up);
    }

    private IEnumerator ExclamationMarkAnim()
    {
        isMarking = true;
        int curMark = markPoint;
        transform.localScale = Vector3.zero;
        renderer.color = markColor;

        transform.DOKill();
        transform.DOScale(originalScale, appearTime).SetEase(Ease.InBounce);

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        isMarking = false;
        renderer.DOFade(0f, disappearTime);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.BossDetectObject, Detect);
    }
}
