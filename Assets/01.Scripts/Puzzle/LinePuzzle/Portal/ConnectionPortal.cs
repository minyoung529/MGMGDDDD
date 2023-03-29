using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPortal : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem rootParticle;
    [SerializeField]
    private ParticleSystem ringParticle;
    [SerializeField]
    private Light pointLight;

    public int Identification { get; set; } = 0;

    public void Initialize(int index, ref Color[] colors)
    {
        Identification = index;
        SetColor(colors[index]);
    }

    private void SetColor(Color color)
    {
        ParticleSystem.MainModule main = rootParticle.main;
        main.startColor = color;

        Gradient grad = new Gradient();
        grad.SetKeys
           (new GradientColorKey[] { new GradientColorKey(color, 0.066f), new GradientColorKey(color, 0.7f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(0.0f, 1.0f) });

        ParticleSystem.ColorOverLifetimeModule col = ringParticle.colorOverLifetime;
        col.color = grad;

        pointLight.color = color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pet pet = collision.transform.GetComponent<Pet>();

        if (pet)
        {
            OnContactPet(pet);
        }
    }

    protected virtual void OnContactPet(Pet pet) { }
}
