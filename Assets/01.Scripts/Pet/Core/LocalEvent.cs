using System;
using System.Collections.Generic;

public class LocalEvent
{
    private List<Action<EventParam>> eventList = new List<Action<EventParam>>();

    public void StartListening(int eventIndex, Action<EventParam> listener) {
        if (eventIndex < eventList.Count && eventList[eventIndex] != null) {
            eventList[eventIndex] += listener;
        }
        else {
            eventList.Add(listener);
        }
    }

    public void StopListening(int eventIndex, Action<EventParam> listener) {
        if (eventIndex < eventList.Count && eventList[eventIndex] != null) {
            eventList[eventIndex] -= listener;
        }
    }

    public void TriggerEvent(int eventIndex, EventParam message = null) {
        if (eventIndex < eventList.Count && eventList[eventIndex] != null) {
            eventList[eventIndex].Invoke(message);
        }
    }
}