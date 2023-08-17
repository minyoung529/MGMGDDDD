using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TriggerAfraid : MonoBehaviour
{
    [SerializeField] ParticleSystem tearsPrefab;
    [SerializeField] Animation sadAnim;
    [SerializeField] PetEmotion emotion;

    [Header("Change Emotion Type")]
    private EmotionType changeEmotionType = EmotionType.Afraid;
    private ParticleSystem tearsParticle;

    Action notAfraid;

    private void Awake()
    {
        CreateTears();
    }

    private void CreateTears()
    {
        tearsParticle = Instantiate(tearsPrefab);
        tearsParticle.transform.SetParent(transform);
        tearsParticle.transform.localPosition = new Vector3(0, 0, 0);
        tearsParticle.gameObject.SetActive(false);

        tearsParticle.playOnAwake = true;
    }

    public void Trigger()
    {
       if(tearsParticle) tearsParticle.gameObject.SetActive(true);
        if (emotion) emotion.SetEmotion(EmotionType.Sad);
        if (sadAnim) sadAnim.Play();
    }

    public void CancleAfraid()
    {
        if (tearsParticle) tearsParticle.gameObject.SetActive(false);
        if (emotion) emotion.SetEmotion(EmotionType.None);
        if (sadAnim) sadAnim.Stop();
    }
}
