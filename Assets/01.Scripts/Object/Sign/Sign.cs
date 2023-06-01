using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
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

    private void Awake()
    {
        pool = new ObjectPool<ParticleSystem>(CreateTears, GetTears, ReleaseTears, DestroyTears, maxSize:6);

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
        if (emotion) emotion.ChangeEmotion(changeEmotionType);

        ParticleSystem tears = pool.Get();
        if (tears)
        {
            tears.transform.SetParent(pet.transform);
            tears.transform.localPosition = new Vector3(0, 0, 0);
        }
        
        if(pet && tears) petParticles.Add(pet, tears);

        Action act = () => NotAfraid(pet);
        pet.Event.StartListening((int)PetEventName.OnFly, act);
    }

    private void NotAfraid(Pet pet)
    {
        ChangePetEmotion emotion = pet.GetComponent<ChangePetEmotion>();
        if(emotion) emotion.ChangeEmotion(EmotionType.None);

        PlayPetAnimation afraidAnim = pet.GetComponent<PlayPetAnimation>();
        if (afraidAnim) afraidAnim.ChangeAnimation(AnimType.Idle);

        Action act = () => NotAfraid(pet);
        pet.Event.StopListening((int)PetEventName.OnFly, act);

        if(petParticles.ContainsKey(pet))
        {
            pool.Release(petParticles[pet]);
            petParticles.Remove(pet);
        }
    }

    #region Trigger

    private void OnTriggerStay(Collider other)
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
