using System;
using UnityEngine;

internal static class InteractionGuard
{
    public static bool CanProcess(InteractableData interactable)
    {
        if (RoomManager.Instance == null || RoomManager.Instance.IsInteractableVisible(interactable))
        {
            return true;
        }

        ShowBlockedInteractionFeedback(interactable);
        return false;
    }

    public static bool HasRequiredItem(InteractableData interactable)
    {
        if (string.IsNullOrEmpty(interactable.requiredItemId))
        {
            return true;
        }

        if (InventoryManager.Instance == null || !InventoryManager.Instance.HasItem(interactable.requiredItemId))
        {
            Debug.LogWarning($"Missing required item for {interactable.id}: {interactable.requiredItemId}");
            UIManager.Instance?.ShowFeedback("Required item missing.", 1.5f);
            return false;
        }

        var selectedItemId = InventoryManager.Instance.SelectedItemId;
        if (string.IsNullOrEmpty(selectedItemId) || !selectedItemId.Equals(interactable.requiredItemId, StringComparison.OrdinalIgnoreCase))
        {
            var requiredItem = InventoryManager.Instance.GetItemById(interactable.requiredItemId);
            var requiredName = requiredItem != null && !string.IsNullOrWhiteSpace(requiredItem.displayName)
                ? requiredItem.displayName
                : interactable.requiredItemId;

            UIManager.Instance?.ShowFeedback($"Select {requiredName} first.", 1.5f);
            return false;
        }

        return true;
    }

    private static void ShowBlockedInteractionFeedback(InteractableData interactable)
    {
        if (!interactable.id.Equals("bedroom_door", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (RoomManager.Instance == null || RoomManager.Instance.CurrentRoom == null)
        {
            return;
        }

        if (!RoomManager.Instance.CurrentRoom.id.Equals(GameManager.HallwayRoomId, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (RealityManager.Instance != null && RealityManager.Instance.CurrentReality == RealityType.OtherSide)
        {
            return;
        }

        UIManager.Instance?.ShowFeedback("Something blocks the way in this reality.", 1.5f);
    }
}
