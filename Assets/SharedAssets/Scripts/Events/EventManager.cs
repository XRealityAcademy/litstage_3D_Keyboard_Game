using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // ✅ Dictionary to store event subscribers
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    // ✅ Subscribe to an event
    public static void Subscribe(string eventName, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            eventDictionary[eventName] += listener; // ✅ Add another listener
        }
    }

    // ✅ Unsubscribe from an event (optional)
    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    // ✅ Trigger an event
    public static void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            Debug.Log($"🔥 Event Triggered: {eventName}");
            eventDictionary[eventName]?.Invoke(); // ✅ Notify all listeners
        }
    }
}