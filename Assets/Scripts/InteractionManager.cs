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

        if (IsAlreadyCollected(interactable))
        {
            UIManager.Instance?.ShowFeedback("There is nothing interesting here.", 1.5f);
            return true;
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

    private static bool IsAlreadyCollected(InteractableData interactable)
    {
        if (interactable == null || interactable.interactionType != InteractionType.Pickup)
        {
            return false;
        }

        var itemId = string.IsNullOrEmpty(interactable.itemId) ? interactable.id : interactable.itemId;
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(itemId))
        {
            return true;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag) && GameManager.Instance != null && GameManager.Instance.HasFlag(interactable.resultFlag))
        {
            return true;
        }

        return false;
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
