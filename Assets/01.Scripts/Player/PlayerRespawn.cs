using DG.Tweening;
using System.Linq;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private ParticleSystem dieParticlePref;
    [SerializeField] private Transform startRespawnPoint;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private Vector3 curRespawnPoint;
    private ParticleSystem dieParticle;
    public Vector3 CurRespawnPoint => curRespawnPoint;

    [Header("Prefab")]
    private CanvasGroup dieCanvas;

    private void Awake()
    {
        CanvasGroup canvasPrefab = Resources.Load<CanvasGroup>("DieCanvas");
        dieCanvas = Instantiate(canvasPrefab);

        if (startRespawnPoint != null)
            curRespawnPoint = startRespawnPoint.position;

        if (dieParticlePref != null)
            dieParticle = Instantiate(dieParticlePref);

        EventManager.StartListening(EventName.PlayerDie, OnDie);
    }

    private void OnDie(EventParam param = null)
    {
        if (param == null || !param.Contain("position"))
            Respawn(curRespawnPoint);
        else
            Respawn((Vector3)param["position"]);
    }

    private void Respawn(Vector3 point)
    {
        Debug.Log("RESPAWN");

        if (dieParticle)
        {
            dieParticle.Stop();
            dieParticle.transform.position = transform.position;
            dieParticle.Play();
        }

        Sequence seq = DOTween.Sequence();
        dieCanvas.gameObject.SetActive(true);
        PetManager.Instance.AllPetActions(x => x.transform.position = point);

        seq.Append(dieCanvas.DOFade(1f, 1f));
        seq.AppendInterval(0.8f);
        seq.AppendCallback(() =>
        {
            transform.position = point;
            Time.timeScale = 1f;
            //GameManager.Instance.PlayerController.Move.ChangeState(StateName.DefaultMove);
        });
        seq.Append(dieCanvas.DOFade(0f, 1f));
        seq.AppendCallback(() => dieCanvas.gameObject.SetActive(false));

    }

    public void RenewSpawnPoint(Vector3 point)
    {
        curRespawnPoint = point;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.PlayerDie, OnDie);
    }
}
