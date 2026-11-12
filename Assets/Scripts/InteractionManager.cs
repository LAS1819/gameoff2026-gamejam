using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public event Action<string, InteractableData> InteractionResolved;

    private readonly HashSet<string> consumedInteractables = new HashSet<string>();

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

    public bool TryInteract(string interactableId)
    {
        if (string.IsNullOrEmpty(interactableId))
        {
            return false;
        }

        var room = RoomManager.Instance?.CurrentRoom;
        if (room == null)
        {
            return false;
        }

        var interactable = room.interactables.Find(item => item != null && item.id == interactableId);
        if (interactable == null)
        {
            Debug.LogWarning($"Interactable not found: {interactableId}");
            return false;
        }

        return TryInteract(interactable, null);
    }

    public bool TryInteract(InteractableData interactable, GameObject sourceObject = null)
    {
        if (interactable == null || string.IsNullOrEmpty(interactable.id))
        {
            return false;
        }

        if (IsConsumed(interactable.id))
        {
            return false;
        }

        if (!InteractionGuard.CanProcess(interactable))
        {
            return false;
        }

        if (!InteractionGuard.HasRequiredItem(interactable))
        {
            return false;
        }

        var wasResolved = InteractionResolver.Resolve(interactable, sourceObject, MarkConsumed);
        if (wasResolved)
        {
            RoomManager.Instance?.RefreshInteractables();
            InteractionResolved?.Invoke(interactable.id, interactable);
        }

        return wasResolved;
    }

    public bool IsConsumed(string interactableId)
    {
        return !string.IsNullOrEmpty(interactableId) && consumedInteractables.Contains(interactableId);
    }

    public void ResetRuntimeState()
    {
        consumedInteractables.Clear();
    }

    private void MarkConsumed(string interactableId)
    {
        if (!string.IsNullOrEmpty(interactableId))
        {
            consumedInteractables.Add(interactableId);
        }
    }
}
