using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRespawn : PlayerMono {
    [SerializeField] private ParticleSystem dieParticlePref;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private float deadLineY = -30f;
    [SerializeField] private Transform spawnPointParent;
    [SerializeField] private UnityEvent respawnEvent; 
    [SerializeField] private UnityEvent startFadeIn; 

    private Transform[] points;
    public Vector3 CurRespawnPoint => points[curIndex].position;
    private int curIndex = 1;
    private int maxIndex = 1;

    private ParticleSystem dieParticle;

    [Header("Prefab")]
    private CanvasGroup dieCanvas;

    private void Awake() {
        CanvasGroup canvasPrefab = Resources.Load<CanvasGroup>("DieCanvas");
        dieCanvas = Instantiate(canvasPrefab);

        if (dieParticlePref != null)
            dieParticle = Instantiate(dieParticlePref);

        EventManager.StartListening(EventName.PlayerDie, OnDie);

        if (spawnPointParent)
            points = spawnPointParent.GetComponentsInChildren<Transform>();
    }

    private void Update() {
        CheckSpawnPoint();
        CheckFallDown();
    }

    private void CheckSpawnPoint() {
        if (points == null) return;

        for(int i = maxIndex + 1; i < points.Length; i++) {
            Vector3 dir = transform.position - points[i].position;
            if (dir.magnitude <= 20f && Vector3.Dot(points[i].forward, dir) > 0) {
                maxIndex = i;
            }
        }

        float min = float.MaxValue;
        for(int i = 1; i <= maxIndex; i++) {
            Vector3 dir = transform.position - points[i].position;
            float distance = dir.magnitude;
            if(distance < min) {
                min = distance;
                curIndex = i;
            }
        }
    }

    private void CheckFallDown() {
        if(transform.position.y <= deadLineY) {
            EventManager.TriggerEvent(EventName.PlayerDie);
        }
    }

    private void OnDie(EventParam param = null) {
        if (param == null || !param.Contain("position"))
            Respawn(CurRespawnPoint);
        else
            Respawn((Vector3)param["position"]);
    }

    private void Respawn(Vector3 point) {
        //gameObject.SetActive(false);

        if (dieParticle) {
            dieParticle.Stop();
            dieParticle.transform.position = transform.position;
            dieParticle.Play();
        }

        Sequence seq = DOTween.Sequence();
        dieCanvas.gameObject.SetActive(true);
        PetManager.Instance.AllPetActions(x => x.transform.position = point);
        
        startFadeIn?.Invoke();

        seq.Append(dieCanvas.DOFade(1f, 1f));
        seq.AppendInterval(0.8f);
        seq.AppendCallback(() => {
            //gameObject.SetActive(true);
            controller.Move.ChangeState(PlayerStateName.DefaultMove);

            transform.position = point;
        });
        seq.AppendCallback(() => Time.timeScale = 1f);
        seq.AppendCallback(()=>respawnEvent?.Invoke());
        seq.Append(dieCanvas.DOFade(0f, 1f));
        seq.AppendCallback(() => 
        {
            dieCanvas.gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.PlayerDie, OnDie);
    }
}