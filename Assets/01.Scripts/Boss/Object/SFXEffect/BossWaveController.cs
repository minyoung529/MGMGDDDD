using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.ParticleSystem;

public class BossWaveController : MonoBehaviour
{
    #region POOL
    private IObjectPool<ParticleSystem> pool;

    [SerializeField]
    private ParticleSystem particlePrefab;

    public void Initialize()
    {
        // NulL ¤Ð
        pool = new ObjectPool<ParticleSystem>(OnCreate, OnGetParticle, OnRelease, OnDestroyed, maxSize: 5);
    }

    private ParticleSystem OnCreate()
    {
        ParticleSystem obj = Instantiate(particlePrefab);
        return obj;
    }

    public void OnGetParticle(ParticleSystem obj)
    {
        obj.gameObject.SetActive(true);
    }

    public void OnRelease(ParticleSystem obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.localScale = Vector3.one;
    }

    public void OnDestroyed(ParticleSystem obj)
    {
        Destroy(obj.gameObject);
    }

    #endregion

    private List<BossSFXObject> bossSFXObjects;

    [SerializeField]
    private Boss boss;

    private GameObject sfxPos;

    private void Start()
    {
        bossSFXObjects = new List<BossSFXObject>(FindObjectsOfType<BossSFXObject>());
        sfxPos = new GameObject(" - - - SFX Pos - - - ");

        foreach (BossSFXObject obj in bossSFXObjects)
        {
            obj.StartListenTrigger(TriggerSFXObj);
            obj.StopListenDestroyed(OnObjDestroyed);
        }

        pool = new ObjectPool<ParticleSystem>(OnCreate, OnGetParticle, OnRelease, OnDestroyed);
    }

    private void TriggerSFXObj(Vector3 pos, float radius, string name)
    {
        ParticleSystem particle = pool.Get();
        particle?.Play();
        particle.transform.localScale = Vector3.one * radius;
        particle.transform.position = pos;

        CallBoss(pos, radius, name);

        StartCoroutine(ParticleDestroy(particle));
    }

    private void OnObjDestroyed(BossSFXObject obj)
    {
        obj.StopListenDestroyed(OnObjDestroyed);
        obj.StopListenTrigger(TriggerSFXObj);

        bossSFXObjects.Remove(obj);
    }

    private IEnumerator ParticleDestroy(ParticleSystem particle)
    {
        yield return new WaitForSeconds(particle.main.duration);
        pool.Release(particle);
    }

    private void CallBoss(Vector3 pos, float radius, string name)
    {
        float dist = Vector3.Distance(pos, boss.transform.position);
        sfxPos.transform.position = pos;

        if (dist < radius)
        {
            sfxPos.transform.name = name;
            EventParam eventParam = new(new Param("DetectPosition", sfxPos.transform));
            EventManager.TriggerEvent(EventName.BossDetectObject, eventParam);
        }
    }

    private void OnDestroy()
    {
        foreach (BossSFXObject obj in bossSFXObjects)
        {
            obj.StopListenTrigger(TriggerSFXObj);
            obj.StopListenDestroyed(OnObjDestroyed);
        }

        bossSFXObjects.Clear();
    }
}
