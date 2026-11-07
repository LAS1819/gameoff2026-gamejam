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
            UIManager.Instance?.ShowFeedback("Required item missing.", 1.5f);
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

        UIManager.Instance?.RefreshInventoryUI();
        UIManager.Instance?.ShowFeedback($"Picked up {interactable.displayName}.", 1.5f);

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

        UIManager.Instance?.ShowFeedback($"Used {interactable.displayName}.", 1.5f);
        return true;
    }

    private bool ResolveExamine(InteractableData interactable)
    {
        if (interactable != null &&
            (interactable.id == "library_door" || interactable.id == "bedroom_door") &&
            (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide))
        {
            Debug.LogWarning($"[InteractionManager] '{interactable.displayName}' can only be used in the Other Side reality.");
            UIManager.Instance?.ShowFeedback("This is only accessible in the Other Side.", 1.5f);
            return false;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        var descriptionText = !string.IsNullOrEmpty(interactable.description)
            ? interactable.description
            : $"You inspected {interactable.displayName}.";

        Debug.Log($"[InteractionManager] Examine: {interactable.displayName} - {descriptionText}");
        UIManager.Instance?.ShowFeedback(descriptionText, 2f);
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
            Debug.LogError($"[InteractionManager] Transition '{interactable.id}' has no linkedRoomId set.");
            return false;
        }

        Debug.Log($"[InteractionManager] ResolveTransition: attempting to load room '{interactable.linkedRoomId}'");
        
        if (RoomManager.Instance == null)
        {
            Debug.LogError($"[InteractionManager] RoomManager instance not found. Cannot transition to '{interactable.linkedRoomId}'");
            return false;
        }

        var success = RoomManager.Instance.LoadRoom(interactable.linkedRoomId);
        Debug.Log($"[InteractionManager] LoadRoom('{interactable.linkedRoomId}') returned: {success}");
        return success;
    }

    private void MarkConsumed(string interactableId)
    {
        if (!string.IsNullOrEmpty(interactableId))
        {
            consumedInteractables.Add(interactableId);
        }
    }
}
