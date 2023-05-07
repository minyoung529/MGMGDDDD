using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TorchLight : MonoBehaviour
{
   // ParticleSystem fireParticle;
    private bool isOn = false;

    public bool IsOn { get { return isOn; } }
    public UnityEvent<bool> OnLighted;

    ParticleSystem[] particles;

    protected virtual void Awake()
    {
      //  fireParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        particles= GetComponentsInChildren<ParticleSystem>();
    }

    private void Start()
    {
        OffLight();
    }

    [ContextMenu("OnLight")]
    public void OnLight()
    {
        isOn = true;

        foreach (ParticleSystem p in particles)
            p.Play();

        OnLighted?.Invoke(true);
        //fireParticle.Play();
    }

    [ContextMenu("OffLight")]
    public void OffLight()
    {
        isOn = false;

        foreach (ParticleSystem p in particles)
            p.Stop();

        //fireParticle.Stop();
    }
    public void Lighting()
    {
        if (IsOn) OffLight();
        else OnLight();

        OnLighted?.Invoke(isOn);
    }

    public virtual void FireCollision()
    {
        //OnLighted?.Invoke(isOn);
    }

    private void OnTriggerStay(Collider other)
    {
        Fire fire = other.GetComponent<Fire>();
        if(fire !=null)
        {
            if (!fire.IsBurn || isOn) return;
            OnLight();
            OnLighted?.Invoke(isOn);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Fire fire = collision.collider.GetComponent<Fire>();
            Debug.Log(collision.collider.name);
        if (fire != null)
        {
            if (!fire.IsBurn || isOn) return;
            OnLight();
            OnLighted?.Invoke(isOn);
        }
    }

}
