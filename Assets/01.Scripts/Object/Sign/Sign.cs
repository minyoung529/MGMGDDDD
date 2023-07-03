using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class Sign : MonoBehaviour
{
    [SerializeField] ParticleSystem tearsParticle;

    [Header("Change Emotion Type")]
    private EmotionType changeEmotionType = EmotionType.Afraid;

    private IObjectPool<ParticleSystem> pool;
    private Dictionary<Pet, ParticleSystem> petParticles = new Dictionary<Pet, ParticleSystem>();
    Action notAfraid;

    private void Awake()
    {
        pool = new ObjectPool<ParticleSystem>(CreateTears, GetTears, ReleaseTears, DestroyTears, maxSize: 6);

        petParticles.Clear();
    }

    private void Afraid(Pet pet)
    {
        PlayPetAnimation afraidAnim = pet.GetComponent<PlayPetAnimation>();
        if (afraidAnim)
        {
            if (afraidAnim.CurAnim == AnimType.Afraid) return;
            afraidAnim.ChangeAnimation(AnimType.Afraid);
        }

        ChangePetEmotion emotion = pet.GetComponent<ChangePetEmotion>();
        if (emotion)
        {
            pet.Event.TriggerEvent((int)PetEventName.OnAfraid);
            emotion.RemoveAllListener();
        }

        ParticleSystem tears = pool.Get();
        if (tears)
        {
            tears.transform.SetParent(pet.transform);
            tears.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (pet && tears) petParticles.Add(pet, tears);

        notAfraid = () => NotAfraid(pet);
        pet.Event.StartListening((int)PetEventName.OnFly, notAfraid);
    }

    private void NotAfraid(Pet pet)
    {
        pet.Event.StopListening((int)PetEventName.OnFly, notAfraid);

        PlayPetAnimation afraidAnim = pet.GetComponent<PlayPetAnimation>();
        if (afraidAnim) afraidAnim.ChangeAnimation(AnimType.Idle);

        if (petParticles.ContainsKey(pet))
        {
            pool.Release(petParticles[pet]);
            petParticles.Remove(pet);
        }

        ChangePetEmotion emotion = pet.GetComponent<ChangePetEmotion>();
        if (emotion)
        {
            emotion.AddAllListener();
            pet.Event.TriggerEvent((int)PetEventName.OnNotAfraid);
        }
    }

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        Pet pet = other.GetComponent<Pet>();
        if (pet)
        {
            Afraid(pet);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Pet pet = other.GetComponent<Pet>();
        if (pet)
        {
            NotAfraid(pet);
        }
    }

    #endregion

    #region Pool

    private ParticleSystem CreateTears()
    {
        ParticleSystem obj = Instantiate(tearsParticle);
        obj.transform.SetParent(transform);
        return obj;
    }

    private void GetTears(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);
    }

    private void ReleaseTears(ParticleSystem particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void DestroyTears(ParticleSystem particle)
    {
        Destroy(particle.gameObject);
    }

    #endregion

}