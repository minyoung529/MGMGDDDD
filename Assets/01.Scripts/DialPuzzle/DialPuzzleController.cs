using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialPuzzleController : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private UnityEvent onDialClear = null;
    [SerializeField] private UnityEvent onNextRound = null;
    [SerializeField] private UnityEvent onPuzzleOver = null;
    [SerializeField] private float playerDieHeight = -10f;
    public Action<TimeType> OnTimeChange = null;

    [Header("Object")]
    [SerializeField] private SpiderRope spider;
    [SerializeField] private CinemachineVirtualCameraBase dialCam;
    [SerializeField] private CinemachineFreeLook playerCam;
    [SerializeField] private Transform ground;
    [SerializeField] private HoleScript hole;
    [SerializeField] private float holeSpeed = 0.05f;
    [SerializeField] private float correctAddRadius = 0.08f;

    private PlayerController player;
    private CinemachineVirtualCameraBase originCam;
    private float targetRadius = 0;
    private Vector3 center2Player = Vector3.zero;

    [Header("Timer")]
    [SerializeField] private float timer = 30f;
    [SerializeField] private float answerTime = 5f;
    // [SerializeField] private float spiderTime = 50f;

    private int round = 0;
    private TimeType curType = TimeType.None;
    public float RemainTime => remainTime;
    private float remainTime = 0;
    private bool pause = false;

    [Header("Dial Answer")]
    [SerializeField] private int maxRound = 2;
    [SerializeField] private List<AnswerData> answerDatas = new List<AnswerData>();
    [SerializeField] private List<AnswerList> correctAnswer = new List<AnswerList>();
    [SerializeField] private List<GameObject> patterns = new List<GameObject>();
    [SerializeField] private List<GameObject> selecctButtons = new List<GameObject>();

    private List<ChangeEmission> emissions = new List<ChangeEmission>();
    private List<Material> materials = new List<Material>();
    private int correctCount = 0;
    private bool isBlockAnswer = false;

    [Header("Hint")]
    [SerializeField] private List<string> hintString = new List<string>();
    public string Hint => hintString[round];

    [Header("SpawnPoint")]
    [SerializeField] private Transform[] spawnPoints;
    private Vector3 spawnPosition;
    public Vector3 SpawnPosition => spawnPosition;

    private DialPuzzleUI dialUIManager;

    private void Awake()
    {
        dialUIManager = GetComponent<DialPuzzleUI>();
    }

    private void Start()
    {
        foreach (GameObject item in patterns)
        {
            emissions.Add(item.GetComponent<ChangeEmission>());
            materials.Add(item.GetComponent<MeshRenderer>().material);
        }
        remainTime = timer;
        hole.Radius = hole.MaxRadius;
        spawnPosition = spawnPoints[0].position;
        dialUIManager.MaxFillAmount = hole.MaxRadius;
        player = GameManager.Instance.PlayerController;
    }

    private void Update()
    {
        CheckTime();
        CheckRespawn();
        UpdateCamPos();
        UpdateHoleSize();
    }

    [SerializeField]
    private float height = 20f;
    [SerializeField]
    private float distance = 10f;

    private void UpdateCamPos()
    {
        //�߽�-�÷��̾� ���� ���� �����
        Vector3 groundPos = ground.position;
        groundPos.y = player.transform.position.y;
        center2Player = (player.transform.position - groundPos).normalized;

        //���� ���Ϳ� ���� �Ÿ��� ���� ������ ī�޶� ��ġ�� ����
        Vector3 camPos = player.transform.position + center2Player * distance;
        //camPos.z += switchOffset;
        camPos.y += (height);
        // camPos.y += (height + (switchOffset));

        dialCam.transform.position = camPos;
        dialCam.transform.LookAt(Vector3.Lerp(player.transform.position, center2Player, 0.1f));
    }

    private void UpdateHoleSize()
    {
        targetRadius = (remainTime / timer) * hole.MaxRadius;
        dialUIManager.SetRadiusSlide(targetRadius);

        float dir = targetRadius - hole.Radius;
        if (dir > 0)
        {
            hole.Radius += holeSpeed * Time.deltaTime;
            if (hole.Radius > targetRadius)
            {
                hole.Radius = targetRadius;
            }
            return;
        }
        if (dir < 0)
        {
            hole.Radius -= holeSpeed * Time.deltaTime;
            if (hole.Radius < targetRadius)
            {
                hole.Radius = targetRadius;
            }
            return;
        }

    }

    private void CheckTime()
    {
        float angle = Vector3.SignedAngle(Vector3.forward, center2Player, Vector3.up) + 180;

        foreach (AnswerData data in answerDatas)
        {
            float maxAngle = data.maxAngle;
            float playerAngle = angle;

            if (data.minAngle > maxAngle)
            {
                maxAngle += 360;
                if (angle < data.minAngle)
                    playerAngle += 360;
            }

            if (data.minAngle < playerAngle && playerAngle < maxAngle)
            {
                if (curType != data.time)
                {
                    curType = data.time;

                    if (selecctButtons[(int)curType].activeSelf) OnTimeChange?.Invoke(curType);
                    CameraSetting(angle, data);
                }
            }
        }
    }

    private void CheckRespawn()
    {
        foreach (Transform item in spawnPoints)
        {
            if (item.position == spawnPosition) continue;
            Vector3 groundPos = ground.position;
            groundPos.y = spawnPosition.y;
            Vector3 center2Spawn = (spawnPosition - groundPos).normalized;
            float angle = Vector3.Angle(center2Spawn, center2Player) * 0.5f;
            Vector3 dir = Vector3.RotateTowards(center2Spawn, center2Player, angle * Mathf.Deg2Rad, 0f);
            groundPos.y = item.position.y;
            Vector3 center2NewSpawn = (item.position - groundPos).normalized;
            if (Vector3.Angle(center2NewSpawn, dir) <= angle)
            {
                spawnPosition = item.transform.position;
                break;
            }
        }
    }

    private void CheckFall()
    {
        //if (player.transform.position.y < playerDieHeight)
        //{
        //    EventParam eventParam = new();
        //    eventParam["position"] = SpawnPosition;
        //    Debug.Log("ASD");
        //    EventManager.TriggerEvent(EventName.PlayerDie, eventParam);
        //}
    }

    private void CameraSetting(float angle, AnswerData data)
    {
        if ((int)curType % 2 == 0)
        {
            GameManager.Instance.SetCursorVisible(false);

            CameraSwitcher.SwitchCamera(dialCam);
        }
        else
        {
            if (data.time == TimeType.AfternoonToEvening)
            {
                GameManager.Instance.SetCursorVisible(true);
                return;
            }
            
            GameManager.Instance.SetCursorVisible(false);
            playerCam.m_XAxis.Value = angle;
            playerCam.m_YAxis.Value = 0.7f;
            CameraSwitcher.SwitchCamera(playerCam);
        }
    }

    #region Start/Stop
    [ContextMenu("Start")]
    public void StartDialPuzzle()
    {
        hole.Radius = hole.MaxRadius;
        dialUIManager.SetUIVisible(true);
        ResetDial();

        dialUIManager.SetHintText(Hint);
        spider.ResetSpider();
        spider.StartFalling(timer);

        GameManager.Instance.SetCursorVisible(false);
        OilPetSkill.IsCrosshair = false;

        originCam = CameraSwitcher.activeCamera;
        CameraSwitcher.Register(dialCam);
        CameraSwitcher.SwitchCamera(dialCam);

        player = GameManager.Instance.PlayerController;
    }

    public void StopDialPuzzle()
    {
        StopTimer();

        GameManager.Instance.SetCursorVisible(false);
        OilPetSkill.IsCrosshair = true;

        dialUIManager.SetUIVisible(false);
        CameraSwitcher.SwitchCamera(originCam);
        CameraSwitcher.UnRegister(dialCam);
    }

    private void ResetDial()
    {
        round = 0;

        ActiveRound(TimeType.Morning, false);
        ActiveRound(TimeType.Evening, false);

        StartTimer();
        spider.ResetSpider();
    }
    #endregion

    #region Answer
    public void CheckAnswer(int typeID)
    {
        if (isBlockAnswer) return;
        if ((TimeType)typeID == correctAnswer[round][correctCount])
            Correct(typeID);
        else
            Wrong(typeID);
    }

    private void Correct(int typeID)
    {
        correctCount++;
        StartCoroutine(SetPatternsColor(Color.green, typeID));
        hole.Radius = hole.Radius - correctAddRadius;

        if (correctCount >= correctAnswer[round].array.Count)
        {
            correctCount = 0;
            Clear();
        }
    }

    private void Wrong(int typeID)
    {
        correctCount = 0;
        isBlockAnswer = true;
        remainTime -= answerTime;
        StartCoroutine(SetPatternsColor(Color.red, typeID));
        StartCoroutine(SetPatternsColor(Color.white, -1, 1f, () => isBlockAnswer = false));
    }

    private void Clear()
    {
        round++;
        remainTime += answerTime;
        isBlockAnswer = true;
        if (round > maxRound)
        {
            onDialClear?.Invoke();
        }
        else
        {
            NextRound();
        }
        StartCoroutine(SetPatternsColor(Color.yellow, -1, 1f));
        StartCoroutine(SetPatternsColor(Color.white, -1, 2f, () => isBlockAnswer = false));
    }

    private void ActiveRound(TimeType type, bool _active)
    {
        patterns[(int)type].SetActive(_active);
        selecctButtons[(int)type].SetActive(_active);
    }

    private void NextRound()
    {
         onNextRound?.Invoke();

        ActiveRound(TimeType.Morning, true);
        ActiveRound(TimeType.Evening, true);

        hole.Radius = hole.MaxRadius;
         StartTimer();
         spider.ResetSpider();
         dialUIManager.SetHintText(Hint);
    }

    private IEnumerator SetPatternsColor(Color color, int index = -1, float delay = 0f, Action onChange = null)
    {
        yield return new WaitForSeconds(delay);
        if (index < 0)
        {
            foreach (ChangeEmission item in emissions)
            {
                item.SetColor(color);
                item.Change();
            }
            foreach (Material item in materials)
                item.color = color;
        }
        else
        {
            emissions[index].SetColor(color);
            emissions[index].Change();
            materials[index].color = color;
        }
        onChange?.Invoke();
    }
    #endregion

    #region Timer
    public void StartTimer()
    {
        ResetTimer();
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (remainTime >= 0)
        {
            while (pause)
            {
                yield return null;
            }
            remainTime -= Time.deltaTime;
            yield return null;
        }
        onPuzzleOver?.Invoke();
    }

    public void GameOver()
    {
        EventParam eventParam = new();
        eventParam["position"] = spawnPoints[0].position;
        EventManager.TriggerEvent(EventName.PlayerDie, eventParam);

        StartDialPuzzle();
    }

    private void StopTimer()
    {
        ResetTimer();
        StopCoroutine(TimerCoroutine());
    }

    private void ResetTimer()
    {
        pause = false;
        remainTime = timer;
    }

    public void SetPause(bool _pause)
    {
        pause = _pause;
    }
    #endregion

}