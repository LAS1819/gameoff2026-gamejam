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

        return TryInteract(interactable);
    }

    public bool TryInteract(InteractableData interactable)
    {
        if (interactable == null || string.IsNullOrEmpty(interactable.id))
        {
            return false;
        }

        if (IsConsumed(interactable.id))
        {
            return false;
        }

        if (RoomManager.Instance != null && !RoomManager.Instance.IsInteractableVisible(interactable))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(interactable.requiredItemId) &&
            (InventoryManager.Instance == null || !InventoryManager.Instance.HasItem(interactable.requiredItemId)))
        {
            Debug.LogWarning($"Missing required item for {interactable.id}: {interactable.requiredItemId}");
            return false;
        }

        var wasResolved = false;
        switch (interactable.interactionType)
        {
            case InteractionType.Pickup:
                wasResolved = ResolvePickup(interactable);
                break;
            case InteractionType.Use:
                wasResolved = ResolveUse(interactable);
                break;
            case InteractionType.Examine:
                wasResolved = ResolveExamine(interactable);
                break;
            case InteractionType.Toggle:
                wasResolved = ResolveToggle(interactable);
                break;
            case InteractionType.Transition:
                wasResolved = ResolveTransition(interactable);
                break;
        }

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

    private bool ResolvePickup(InteractableData interactable)
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager instance not found.");
            return false;
        }

        var itemId = string.IsNullOrEmpty(interactable.itemId) ? interactable.id : interactable.itemId;
        var itemAdded = InventoryManager.Instance.AddItem(new InventoryItemData
        {
            id = itemId,
            displayName = interactable.displayName,
            description = interactable.description,
            isKeyItem = true
        });

        if (!itemAdded)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        MarkConsumed(interactable.id);
        return true;
    }

    private bool ResolveUse(InteractableData interactable)
    {
        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        if (interactable.consumeAfterUse)
        {
            MarkConsumed(interactable.id);
        }

        return true;
    }

    private bool ResolveExamine(InteractableData interactable)
    {
        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        return true;
    }

    private bool ResolveToggle(InteractableData interactable)
    {
        RealityManager.Instance?.ToggleReality();

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        return true;
    }

    private bool ResolveTransition(InteractableData interactable)
    {
        if (string.IsNullOrEmpty(interactable.linkedRoomId))
        {
            return false;
        }

        return RoomManager.Instance != null && RoomManager.Instance.LoadRoom(interactable.linkedRoomId);
    }

    private void MarkConsumed(string interactableId)
    {
        if (!string.IsNullOrEmpty(interactableId))
        {
            consumedInteractables.Add(interactableId);
        }
    }
}
