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

    protected virtual void FireCollision()
    {
        OnLight();
        OnLighted?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        FireBall fire = other.GetComponent<FireBall>();
        if(fire !=null)
        {
            Destroy(fire.gameObject);
            FireCollision();
        }
    }
}
