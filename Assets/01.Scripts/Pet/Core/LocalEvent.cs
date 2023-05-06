using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalEvent {
    private Dictionary<int, Action> eventDictionary = new Dictionary<int, Action>();

    public void StartListening(int eventIndex, Action listener) {
        if (eventDictionary.ContainsKey(eventIndex)) {
            eventDictionary[eventIndex] += listener;
        }
        else {
            eventDictionary.Add(eventIndex, listener);
        }
    }

    public void StopListening(int eventIndex, Action listener) {
        if (eventDictionary.ContainsKey(eventIndex)) {
            eventDictionary[eventIndex] -= listener;
        }
    }

    public void TriggerEvent(int eventIndex) {
        if (eventDictionary.ContainsKey(eventIndex)) {
            eventDictionary[eventIndex]?.Invoke();
        }
    }
}