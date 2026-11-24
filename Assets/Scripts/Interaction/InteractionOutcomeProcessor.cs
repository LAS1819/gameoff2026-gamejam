using System;

internal static class InteractionOutcomeProcessor
{
    public static void ApplyPickupOutcome(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.SetFlag(interactable.resultFlag, true);
            }
        }

        MarkConsumedIfNeeded(interactable, onConsumed);
    }

    public static void ApplyUseOutcome(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.SetFlag(interactable.resultFlag, true);
            }
        }

        ConsumeRequiredItemIfNeeded(interactable);
        MarkConsumedIfNeeded(interactable, onConsumed);
        TryResolveBasementProgress(interactable);
    }

    public static void ConsumeRequiredItemIfNeeded(InteractableData interactable)
    {
        if (interactable == null || string.IsNullOrEmpty(interactable.requiredItemId))
        {
            return;
        }

        if (!interactable.consumeAfterUse && !interactable.consumeRequiredItemOnUse)
        {
            return;
        }

        var inventoryManager = InventoryManager.Instance;
        if (inventoryManager != null)
        {
            inventoryManager.RemoveItem(interactable.requiredItemId);
        }

        var flagName = GetFlagNameForConsumedItem(interactable.requiredItemId);
        if (!string.IsNullOrEmpty(flagName) && GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(flagName, false);
        }
    }

    private static string GetFlagNameForConsumedItem(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return null;
        }

        switch (itemId)
        {
            case "staff_key":
                return GameFlagNames.HasStaffKey;
            case "library_key":
                return GameFlagNames.HasLibraryKey;
            case "basement_code":
            case "code_basement":
                return GameFlagNames.BasementCodeAcquired;
            default:
                return null;
        }
    }

    public static void MarkConsumedIfNeeded(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null || !interactable.consumeAfterUse)
        {
            return;
        }

        if (onConsumed != null)
        {
            onConsumed.Invoke(interactable.id);
        }
    }

    private static void TryResolveBasementProgress(InteractableData interactable)
    {
        if (interactable == null || GameManager.Instance == null)
        {
            return;
        }

        if (interactable.id == "pressure_release_valve")
        {
            GameManager.Instance.SetFlag(GameFlagNames.SteamEnabled, true);
        }

        var hasEvidence = GameManager.Instance.HasFlag(GameFlagNames.BasementEvidenceFound);
        var valveRepaired = GameManager.Instance.HasFlag(GameFlagNames.PressureValveRepaired);
        GameManager.Instance.SetFlag(GameFlagNames.BasementResolved, hasEvidence && valveRepaired);
    }
}
