using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
    [SerializeField] ParticleSystem fireParticle;
    private bool isOn = false;

    public bool IsOn { get { return isOn; } }

    private void Awake()
    {
        OffLight();
    }

    public void OnLight()
    {
        isOn = true;
        fireParticle.Play();
    }

    public void OffLight()
    {
        isOn = false;
        fireParticle.Stop();
    }

    protected virtual void FireCollision()
    {

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
