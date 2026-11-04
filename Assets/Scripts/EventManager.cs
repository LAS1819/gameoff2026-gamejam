using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// EventManager handles game events, narrative triggers, and audio cues.
/// This is a stub for MVP. Later expansion: narrative sequences, music state, sound effects.
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private string eventManagerName = "EventManager";

    private Dictionary<string, Action> eventCallbacks = new Dictionary<string, Action>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log($"[{eventManagerName}] Initialized (stub for MVP)");
    }

    /// <summary>
    /// Subscribe to a named event. When event is fired, callback executes.
    /// </summary>
    public void Subscribe(string eventName, Action callback)
    {
        if (!eventCallbacks.ContainsKey(eventName))
            eventCallbacks[eventName] = null;

        eventCallbacks[eventName] += callback;
    }

    /// <summary>
    /// Unsubscribe from a named event.
    /// </summary>
    public void Unsubscribe(string eventName, Action callback)
    {
        if (eventCallbacks.ContainsKey(eventName))
            eventCallbacks[eventName] -= callback;
    }

    /// <summary>
    /// Fire a named event. All subscribers execute.
    /// </summary>
    public void FireEvent(string eventName)
    {
        if (eventCallbacks.ContainsKey(eventName) && eventCallbacks[eventName] != null)
        {
            eventCallbacks[eventName].Invoke();
        }
    }
}
