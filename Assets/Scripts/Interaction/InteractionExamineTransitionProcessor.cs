using System;
using UnityEngine;

internal static class InteractionExamineTransitionProcessor
{
    public static bool ResolveExamine(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        if (TryResolveMemoryDiscovery(interactable))
        {
            return true;
        }

        if ((interactable.id == "library_door" || interactable.id == "bedroom_door") &&
            (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide))
        {
            Debug.LogWarning($"[InteractionManager] '{interactable.displayName}' can only be used in the Other Side reality.");

            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.ShowFeedback("This is only accessible in the Other Side.", 1.5f);
            }

            return false;
        }

        ApplyExplicitExamineFlag(interactable);

        var currentReality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        var descriptionText = interactable.GetDescriptionForReality(currentReality);
        if (string.IsNullOrWhiteSpace(descriptionText))
        {
            descriptionText = $"You inspected {interactable.displayName}.";
        }

        Debug.Log($"[InteractionManager] Examine: {interactable.displayName} - {descriptionText}");

        var ui = UIManager.Instance;
        if (ui != null)
        {
            ui.ShowFeedback(descriptionText, 2f);
        }

        TryResolveBasementProgress();

        return true;
    }

    private static void ApplyExplicitExamineFlag(InteractableData interactable)
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null || interactable == null)
        {
            return;
        }

        if (interactable.id.Equals("hallway_obstacle", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("shelf_secret_door", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("shelf_right", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("right_shelf", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("storage_shelf_left", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("storage_shelf_right", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("personal_effects_box", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("pipelines", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("ambulance_lights", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("denial_manifestation", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(interactable.resultFlag))
            {
                gameManager.SetFlag(interactable.resultFlag, true);
            }
        }
    }

    private static bool TryResolveMemoryDiscovery(InteractableData interactable)
    {
        if (string.IsNullOrWhiteSpace(interactable.memoryId))
        {
            return false;
        }

        var isLobbyFirstMemory = interactable.id.Equals("memory_01_family_picture", StringComparison.OrdinalIgnoreCase)
            && interactable.memoryId == GameFlagNames.Memory_01;
        if (!isLobbyFirstMemory
            && (RealityManager.Instance == null
                || RealityManager.Instance.CurrentReality != RealityType.OtherSide))
        {
            return false;
        }

        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return false;
        }

        if (gameManager.HasFlag(interactable.memoryId))
        {
            return false;
        }

        if (!MemoryCatalog.TryGet(interactable.memoryId, out var memory))
        {
            Debug.LogWarning($"[MemoryDiscovery] Missing catalog entry for '{interactable.memoryId}'.");
            return false;
        }

        gameManager.SetFlag(interactable.memoryId, true);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMemoryDiscovery(memory);
        }

        return true;
    }

    public static bool ResolveTransition(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(interactable.linkedRoomId))
        {
            Debug.LogError($"[InteractionManager] Transition '{interactable.id}' has no linkedRoomId set.");
            return false;
        }

        if (interactable.consumeAfterUse || interactable.consumeRequiredItemOnUse)
        {
            InteractionOutcomeProcessor.ConsumeRequiredItemIfNeeded(interactable);
        }

        if (interactable.consumeAfterUse)
        {
            InteractionOutcomeProcessor.MarkConsumedIfNeeded(interactable, onConsumed);
        }

        Debug.Log($"[InteractionManager] ResolveTransition: attempting to load room '{interactable.linkedRoomId}'");

        if (RoomManager.Instance == null)
        {
            Debug.LogError($"[InteractionManager] RoomManager instance not found. Cannot transition to '{interactable.linkedRoomId}'");
            return false;
        }

        var success = RoomManager.Instance.LoadRoom(interactable.linkedRoomId);
        Debug.Log($"[InteractionManager] LoadRoom('{interactable.linkedRoomId}') returned: {success}");

        if (success && !string.IsNullOrEmpty(interactable.resultFlag) && GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(interactable.resultFlag, true);
        }

        if (success)
        {
            TryResolveBasementProgress();
        }

        return success;
    }

    private static void TryResolveBasementProgress()
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return;
        }

        var hasEvidence = gameManager.HasFlag(GameFlagNames.BasementEvidenceFound);
        var valveRepaired = gameManager.HasFlag(GameFlagNames.PressureValveRepaired);
        var resolved = hasEvidence && valveRepaired;
        gameManager.SetFlag(GameFlagNames.BasementResolved, resolved);
    }
}
