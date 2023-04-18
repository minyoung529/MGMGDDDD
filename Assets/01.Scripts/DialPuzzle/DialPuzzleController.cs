using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialPuzzleController : MonoBehaviour {
    private int round = 0;

    private TimeType curType = TimeType.None;
    public TimeType CurType => curType;

    [Header("Event")]
    [SerializeField] private UnityEvent onPuzzleClear = null;
    [SerializeField] private UnityEvent onPuzzleOver = null;

    [Header("Object")]
    [SerializeField] private SpiderRope spider;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private CinemachineVirtualCameraBase dialCam;
    [SerializeField] private Transform ground;
    [SerializeField] private HoleScript hole;
    [SerializeField] private float holeSpeed = 0.05f;

    private PlayerMove player;
    private CinemachineVirtualCameraBase originCam;
    private float targetRadius = 0;

    [Header("Timer")]
    [SerializeField] private float timer = 30f;
    [SerializeField] private float loseAmount = 5f;
    private float remainTime = 0;
    public float RemainTime => remainTime;
    private bool pause = false;

    [Header("Dial Answer")]
    [SerializeField] private int maxRound = 2;
    [SerializeField] private List<AnswerData> answerDatas = new List<AnswerData>();
    [SerializeField] private List<AnswerList> correctAnswer = new List<AnswerList>();
    [SerializeField] private List<GameObject> patterns = new List<GameObject>();
    private List<ChangeEmission> emissions = new List<ChangeEmission>();
    private List<Material> materials = new List<Material>();
    private int correctCount = 0;
    private bool isBlockAnswer = false;

    [Header("Hint")]
    [SerializeField] private List<string> hintString = new List<string>();
    public string Hint => hintString[round];

    [Header("SpawnPoint")]
    [SerializeField] private Transform[] spawnPoints;
    public Vector3 SpawnPosition => spawnPoints[(int)curType].position;

    private void Start() {
        StartDialPuzzle();
        FindPatternCompo();
        hole.Radius = (remainTime / timer) * hole.MaxRadius;
    }

    private void FindPatternCompo() {
        foreach(GameObject item in patterns) {
            emissions.Add(item.GetComponent<ChangeEmission>());
            materials.Add(item.GetComponent<MeshRenderer>().material);
        }
    }

    private void Update() {
        UpdateCamPos();
        UpdateHoleSize();
    }

    private void UpdateCamPos() {
        //중심-플레이어 방향 벡터 만들기
        Vector3 groundPos = ground.position;
        groundPos.y = player.transform.position.y;
        Vector3 dir = (player.transform.position - groundPos).normalized;

        //방향 벡터와 일정 거리를 더한 지점을 카메라 위치로 지정
        Vector3 camPos = player.transform.position + dir * 20f;
        camPos.y += 50f;
        dialCam.transform.position = camPos;
        dialCam.transform.LookAt(groundPos);
    }

    private void UpdateHoleSize() {
        targetRadius = (remainTime / timer) * hole.MaxRadius;
        float dir = targetRadius - hole.Radius;
        if (dir > 0) {
            hole.Radius += holeSpeed * Time.deltaTime;
            if (hole.Radius > targetRadius)
                hole.Radius = targetRadius;
            return;
        }
        if (dir < 0) {
            hole.Radius -= holeSpeed * Time.deltaTime;
            if (hole.Radius < targetRadius)
                hole.Radius = targetRadius;
            return;
        }
    }

    #region Start/Stop
    public void StartDialPuzzle() {
        ResetDial(); 
        //spider.FallSpider();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Pet.IsCameraAimPoint = false;
        OilPetSkill.IsCrosshair = false;

        originCam = CameraSwitcher.activeCamera;
        CameraSwitcher.Register(dialCam);
        CameraSwitcher.SwitchCamera(dialCam);

        player = GameManager.Instance.Player;
    }

    public void StopDialPuzzle() {
        StopTimer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Pet.IsCameraAimPoint = true;
        OilPetSkill.IsCrosshair = true;

        CameraSwitcher.SwitchCamera(originCam);
        CameraSwitcher.UnRegister(dialCam);
    }

    private void ResetDial() {
        StartTimer();
        //spider.ResetSpider(RemainTime);
        round = 0;
    }
    #endregion

    #region Answer
    private void CheckAngle() {
        foreach(AnswerData data in answerDatas) {
            if()
        }
    }

    public void CheckAnswer(int typeID) {
        if ((TimeType)typeID == correctAnswer[round][correctCount])
            Correct(typeID);
        else
            Wrong(typeID);
    }

    private void Correct(int typeID) {
        correctCount++;
        emissions[typeID].SetColor(Color.green);
        emissions[typeID].Change();
        materials[typeID].color = Color.green;
        if(correctCount >= correctAnswer.Count) {
            correctCount = 0;
            Clear();
        }
    }

    private void Wrong(int typeID) {
        correctCount = 0;
        isBlockAnswer = true;
        emissions[typeID].SetColor(Color.red);
        emissions[typeID].Change();
        materials[typeID].color = Color.red;
        Invoke(nameof(ResetPattern), 2f);
    }

    private void Clear() {
        round++;
        isBlockAnswer = true;
        if (round > maxRound) {
            onPuzzleClear?.Invoke();
            Invoke(nameof(ResetPattern), 2f);
        }
    }

    private void ResetPattern() {
        foreach(ChangeEmission item in emissions)
            item.BackToOriginalColor();
        foreach(Material item in materials)
            item.color = Color.white;
        isBlockAnswer = false;
    }
    #endregion

    #region Timer
    public void StartTimer() {
        ResetTimer();
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer() {
        while (remainTime >= 0) {
            while (pause) {
                yield return null;
            }
            remainTime -= Time.deltaTime;
            yield return null;
        }
        onPuzzleOver?.Invoke();
    }

    private void StopTimer() {
        ResetTimer();
        StopCoroutine(GameTimer());
    }

    private void ResetTimer() {
        pause = false;
        remainTime = timer;
    }

    public void SetPause(bool _pause) {
        pause = _pause;
    }
    #endregion

    private void OnDestroy() {
        StopDialPuzzle();
    }
}