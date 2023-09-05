using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Fire : MonoBehaviour
{
    [SerializeField] bool playOnAwake = false;
    [SerializeField] UnityEvent fireEvent;
    [SerializeField] UnityEvent onFireComplete;
    [SerializeField] ParticleSystem[] fireParticle;
    [SerializeField] bool isDestroyType = false;
    [SerializeField] float burnDelay = 0f;
    [SerializeField] float burningTime = 2f;
    // [SerializeField] bool isTriggerBurn = false;

    bool isReadyBurn = false;
    bool isBurn = false;
    float burningReadyTime = 2f;

    public bool IsBurn { get { return isBurn; } }
    // public bool IsTriggerBurn { get { return isTriggerBurn; } }

    Sequence seq;

    [SerializeField]
    private bool isCool = false;

    [SerializeField]
    private bool isClingTo = true;

    [SerializeField]
    private bool onlyScriptBurn = false;

    public float BurningTime
    {
        get => burningTime;
        set => burningTime = value;
    }

    public UnityEvent OnBurn => fireEvent;
    public bool IsClingTo => isClingTo;

    private void Awake()
    {
        isBurn = playOnAwake;
        FireParticleStop();
        if (playOnAwake)
        {
            Burn();
        }
    }

    public void Burn(bool isPhysics = false)
    {
        if (!gameObject.activeSelf) return;
        if (isPhysics && onlyScriptBurn) return;
        if (IsBurn) return;

        isBurn = true;

        seq = DOTween.Sequence();
        seq.AppendInterval(burnDelay);
        seq.AppendCallback(() =>
        {
            isBurn = true;

            FireParticlePlay();
            if (isDestroyType) StartCoroutine(DestroyBurn());
            fireEvent?.Invoke();
            
            if (isCool)
            {
                StartCoroutine(CoolFire());
            }
        });
    }

    private IEnumerator CoolFire()
    {
        yield return new WaitForSeconds(burningTime);
        StopBurn();
    }

    private void FireParticlePlay()
    {
        if (fireParticle == null) return;

        for (int i = 0; i < fireParticle.Length; i++)
        {
            fireParticle[i].Play();
        }
    }
    private void FireParticleStop()
    {
        if (fireParticle == null) return;

        for (int i = 0; i < fireParticle.Length; i++)
        {
            fireParticle[i].Stop();
        }
    }
    public void StopBurn()
    {
        isBurn = false;
        FireParticleStop();
        onFireComplete?.Invoke();
    }

    public IEnumerator DestroyBurn()
    {
        yield return new WaitForSeconds(burningTime);
        onFireComplete?.Invoke();
        Destroy(gameObject);
    }

    public void DestroyBurn(float destroyTime)
    {
        StartCoroutine(StopAndDestroy(destroyTime));
    }

    IEnumerator StopAndDestroy(float t)
    {
        yield return new WaitForSeconds(t);
        FireParticleStop();
        Destroy(gameObject, 1.0f);
    }

    public void StayFire()
    {
        isReadyBurn = true;
        StartCoroutine(StayInFire());
    }
    private void ExitInFire()
    {
        if (isBurn) return;
        isReadyBurn = false;
        StopCoroutine(StayInFire());
    }

    private IEnumerator StayInFire()
    {
        yield return new WaitForSeconds(burningReadyTime);

        if (isReadyBurn)
        {
            Burn();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsBurn /* && IsTriggerBurn*/) return;

        IceMelting[] ices = other.GetComponents<IceMelting>();
        foreach (IceMelting ice in ices)
        {
            ice.Melt();
        }

        if (!isClingTo) return;

        Fire[] fires = other.GetComponents<Fire>();
        foreach (Fire f in fires)
        {
            if (f.IsBurn) continue;
            transform.DOKill();
            f.Burn(true);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!IsBurn /*&& IsTriggerBurn*/) return;

        IceMelting[] ices = collision.collider.GetComponents<IceMelting>();
        foreach (IceMelting ice in ices)
        {
            ice.Melt();
        }

        if (!isClingTo) return;

        Fire[] fires = collision.collider.GetComponents<Fire>();
        foreach (Fire f in fires)
        {
            if (f.IsBurn) continue;

            transform.DOKill();
            f.Burn(true);
        }
    }

    private void OnDestroy()
    {
        seq.Kill();
    }

    public void ListeningFireEvent(UnityAction action)
    {
        fireEvent.RemoveListener(action);
        fireEvent.AddListener(action);
    }
}
