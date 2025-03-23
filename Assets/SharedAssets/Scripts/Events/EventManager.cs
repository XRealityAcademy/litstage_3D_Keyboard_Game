using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    void Awake()
    {
        // ✅ Ensure only ONE instance of EventManager exists (Singleton)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ✅ Persist across scenes
        }
        else
        {
            Destroy(gameObject); // ✅ Destroy duplicates
        }
    }

    public static void Subscribe(string eventName, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            eventDictionary[eventName] += listener;
        }
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
//            Debug.Log($"🔥 Event Triggered: {eventName}");
            eventDictionary[eventName]?.Invoke();
        }
    }


}