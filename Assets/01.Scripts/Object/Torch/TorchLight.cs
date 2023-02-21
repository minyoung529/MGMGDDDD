using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class TorchLight : MonoBehaviour
{
    ParticleSystem fireParticle;
    private bool isOn = false;

    public bool IsOn { get { return isOn; } }

    [SerializeField]
    private UnityEvent OnLighted;

    ParticleSystem[] particles;

    protected virtual void Awake()
    {
        fireParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        particles= GetComponentsInChildren<ParticleSystem>();
        OffLight();
    }

    public void OnLight()
    {
        isOn = true;
        FireCollision();

        foreach (ParticleSystem p in particles)
            p.Play();

        //fireParticle.Play();
    }

    public void OffLight()
    {
        foreach (ParticleSystem p in particles)
            p.Stop();

        isOn = false;

        //fireParticle.Stop();
    }
    public void Lighting()
    {
        Debug.Log(IsOn);
        if (IsOn) OffLight();
        else OnLight();
    }

    public virtual void FireCollision()
    {
        OnLighted?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        Fire fire = other.GetComponent<Fire>();
        if(fire !=null)
        {
            if (!fire.IsBurn) return;
            OnLight();
        }
    }
}
