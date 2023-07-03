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

    private Dictionary<PetEventName, Action> emotioniEventDict;

    private void Start()
    {
        pet = GetComponent<Pet>();
        petEmotion = pet.GetComponentInChildren<PetEmotion>();
        emotioniEventDict = new Dictionary<PetEventName, Action>();
        
        InitEventDictionary();
        AddAllListener();
    }
    private void InitEventDictionary()
    {
        foreach(EmotionByPetEvent emotion in emotionByPetEvents)
        {
            emotioniEventDict.Add(emotion.eventName, () => ChangeEmotion(emotion.emotionType));
        }
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
            pet.Event.StartListening((int)emotionEvent.eventName, emotioniEventDict[emotionEvent.eventName]);
        }
    }

    public void RemoveAllListener()
    {
        foreach (EmotionByPetEvent emotionEvent in emotionByPetEvents)
        {
            pet.Event.StopListening((int)emotionEvent.eventName, emotioniEventDict[emotionEvent.eventName]);
        }
    }

    public void ChangeEmotion(EmotionType type)
    {
        petEmotion.SetEmotion(type);
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
        this.action -= action;
    }
}