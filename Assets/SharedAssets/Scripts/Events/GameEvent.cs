using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public string eventID;  // Unique identifier for the event
    public string dependsOnEventID; // ✅ If this event depends on another event

    private void Start()
    {

        if (string.IsNullOrEmpty(eventID))
        {
            Debug.LogError($"[GameEvent] Missing eventID on '{gameObject.name}'. Please assign a unique Event ID.");
        }

        if (!string.IsNullOrEmpty(dependsOnEventID))
        {
            // ✅ Wait for the dependency event to complete before running
            EventManager.Subscribe(dependsOnEventID, StartEvent);
        }
    }

    public void StartEvent()
    {
        Debug.Log($"🚀 Starting Event: {eventID}");

        // ✅ Simulate an event happening (Replace this with actual event logic)
        Invoke(nameof(CompleteEvent), 2f); // Simulating delay of 2 seconds
    }

    public void CompleteEvent()
    {
        Debug.Log($"✅ Event Completed: {eventID}");
        EventManager.TriggerEvent(eventID); // ✅ Notify subscribers this event is done
    }
}