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
    private UnityEvent OnLighting;

    ParticleSystem[] particles;

    private void Awake()
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

        Debug.Log(particles.Length);
        //fireParticle.Play();
    }

    public void OffLight()
    {
        foreach (ParticleSystem p in particles)
            p.Stop();

        isOn = false;
        Debug.Log(particles.Length);

        //fireParticle.Stop();
    }

    protected virtual void FireCollision()
    {
        OnLight();
        OnLighting?.Invoke();
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
