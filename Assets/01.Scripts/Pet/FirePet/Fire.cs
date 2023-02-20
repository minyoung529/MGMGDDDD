using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] bool isDestroyType = false;

    bool isReadyBurn = false;
    bool isBurn = false;
    float burningTime = 2f;
    float burningReadyTime = 2f;

    public bool IsBurn { get { return isBurn; } }

    private void Awake()
    {
        isBurn = false;
        fireParticle.Stop();
    }

    public void Burn()
    {
        isBurn = true;
        fireParticle.Play();
        if (isDestroyType) DestroyBurn();

        StartCoroutine(CoolFire());
    }

    private IEnumerator CoolFire()
    {
        yield return new WaitForSeconds(10f);
        StopBurn();
    }

public void StopBurn()  
    {
        isBurn = false;
        fireParticle.Stop();
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
