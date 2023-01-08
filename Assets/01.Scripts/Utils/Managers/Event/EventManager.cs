using System;
using System.Collections.Generic;

public class EventManager
{
    static private Dictionary<EventName, Action<Dictionary<string, object>>> eventDictionary = new Dictionary<EventName, Action<Dictionary<string, object>>>();
    
    public static void StartListening(EventName eventName, Action<Dictionary<string, object>> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] += listener;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(EventName eventName, Action<Dictionary<string, object>> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] += listener;
        }
    }

    public static void TriggerEvent(EventName eventName, Dictionary<string, object> message)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName].Invoke(message);
        }
    }
}