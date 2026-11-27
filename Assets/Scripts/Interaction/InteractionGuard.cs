using System;
using UnityEngine;

internal static class InteractionGuard
{
    public static bool CanProcess(InteractableData interactable)
    {
        if (interactable.requiresLantern)
        {
            var roomAllowed = RoomManager.Instance != null
                && RoomManager.Instance.CurrentRoom != null
                && RoomManager.Instance.CurrentRoom.id.Equals(
                    GameManager.BasementRoomId,
                    StringComparison.OrdinalIgnoreCase);
            var gameManager = GameManager.Instance;
            var lanternActive = gameManager != null
                && gameManager.HasFlag(GameFlagNames.FlashlightActive);
            var hasLantern = gameManager != null
                && gameManager.HasFlag(GameFlagNames.HasFlashlight);

            if (!roomAllowed || !hasLantern || !lanternActive)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(SFXCue.UiDenied);
                }
                ShowFeedback(
                    "It is too dark to understand. Turn on the flashlight.",
                    1.8f);
                return false;
            }
        }

        if (RoomManager.Instance == null || RoomManager.Instance.IsInteractableVisible(interactable))
        {
            return true;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.UiDenied);
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

        if (GameManager.Instance != null
            && !string.IsNullOrEmpty(interactable.resultFlag)
            && GameManager.Instance.HasFlag(interactable.resultFlag))
        {
            return true;
        }

        if (InventoryManager.Instance == null || !InventoryManager.Instance.HasItem(interactable.requiredItemId))
        {
            Debug.LogWarning($"Missing required item for {interactable.id}: {interactable.requiredItemId}");
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(SFXCue.UiDenied);
            }
            ShowFeedback("Required item missing.", 1.5f);
            return false;
        }

        var selectedItemId = InventoryManager.Instance.SelectedItemId;
        if (string.IsNullOrEmpty(selectedItemId) || !selectedItemId.Equals(interactable.requiredItemId, StringComparison.OrdinalIgnoreCase))
        {
            var requiredItem = InventoryManager.Instance.GetItemById(interactable.requiredItemId);
            var requiredName = requiredItem != null && !string.IsNullOrWhiteSpace(requiredItem.displayName)
                ? requiredItem.displayName
                : interactable.requiredItemId;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(SFXCue.UiDenied);
            }
            ShowFeedback($"Select {requiredName} first.", 1.5f);
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

        ShowFeedback("Something blocks the way in this reality.", 1.5f);
    }

    private static void ShowFeedback(string message, float duration)
    {
        if (UIManager.Instance == null)
        {
            return;
        }

        UIManager.Instance.ShowFeedback(message, duration);
    }
}
