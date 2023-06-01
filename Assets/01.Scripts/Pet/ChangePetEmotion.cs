using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePetEmotion : MonoBehaviour
{
    private Pet pet;
    private PetEmotion petEmotion;

    [SerializeField]
    private List<EmotionByPetEvent> emotionByPetEvents;

    private void Start()
    {
        pet = GetComponent<Pet>();
        petEmotion = pet.GetComponentInChildren<PetEmotion>();

        foreach (EmotionByPetEvent emotionEvent in emotionByPetEvents)
        {
            emotionEvent.StartListening(() => petEmotion.SetEmotion(emotionEvent.emotionType));
        }

        AddAllListener();
    }

    private void OnDestroy()
    {
        if (pet == null) return;
        if (pet.Event == null) return;

        RemoveAllListener();
    }

    public void AddAllListener()
    {
        foreach (EmotionByPetEvent emotionEvent in emotionByPetEvents)
        {
            pet.Event.StartListening((int)emotionEvent.eventName, emotionEvent.Action);
        }
    }

    public void RemoveAllListener()
    {
        foreach (EmotionByPetEvent emotionEvent in emotionByPetEvents)
        {
            pet.Event.StopListening((int)emotionEvent.eventName, emotionEvent.Action);
        }
    }
}

[System.Serializable]
public class EmotionByPetEvent
{
    public PetEventName eventName;
    public EmotionType emotionType;

    private Action action;
    public Action Action => action;

    public void StartListening(Action action)
    {
        this.action += action;
    }

    public void StopListening(Action action)
    {
        this.action += action;
    }
}