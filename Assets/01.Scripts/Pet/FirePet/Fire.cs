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
    [SerializeField] ParticleSystem[] fireParticle;
    [SerializeField] bool isDestroyType = false;
    [SerializeField] private float burnDelay = 0f;

    bool isReadyBurn = false;
    bool isBurn = false;
    float burningTime = 2f;
    float burningReadyTime = 2f;

    public bool IsBurn { get { return isBurn; } }

    private void Awake()
    {
        isBurn = playOnAwake;
        FireParticleStop();
        if(playOnAwake)
        {
            Burn();
        }
    }

    public void Burn()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(burnDelay);
        seq.AppendCallback(() =>
        {
            isBurn = true;

            FireParticlePlay();
            if (isDestroyType) DestroyBurn();
            fireEvent?.Invoke();
            //StartCoroutine(CoolFire());
        });
    }

    private IEnumerator CoolFire()
    {
        yield return new WaitForSeconds(10f);
        StopBurn();
    }
    private void FireParticlePlay()
    {
        for (int i = 0; i < fireParticle.Length; i++)
        {
            fireParticle[i].Play();
        }
    }
    private void FireParticleStop()
    {
        for (int i = 0; i < fireParticle.Length; i++)
        {
            fireParticle[i].Stop();
        }
    }
    public void StopBurn()  
    {
        isBurn = false;
        FireParticleStop();
    }

    public void DestroyBurn()
    {
        Destroy(gameObject, burningTime);
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
        if (!IsBurn) return;

        IceMelting[] ices = other.GetComponents<IceMelting>();
        foreach (IceMelting ice in ices)
        {
            ice.Melt();
        }

        Fire[] fires = other.GetComponents<Fire>();
        foreach (Fire f in fires)
        {
            if (f.IsBurn) continue;
            transform.DOKill();
            f.Burn();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!IsBurn) return;

        IceMelting[] ices = collision.collider.GetComponents<IceMelting>();
        foreach (IceMelting ice in ices)
        {
            ice.Melt();
        }

        Fire[] fires = collision.collider.GetComponents<Fire>();
        foreach (Fire f in fires)
        {
            if (f.IsBurn) continue;
            transform.DOKill();
            f.Burn();
        }

    }

}
