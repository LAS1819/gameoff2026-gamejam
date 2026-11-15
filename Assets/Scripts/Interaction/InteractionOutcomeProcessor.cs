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
    }

    public static void ConsumeRequiredItemIfNeeded(InteractableData interactable)
    {
        if (interactable == null || !interactable.consumeAfterUse || string.IsNullOrEmpty(interactable.requiredItemId))
        {
            return;
        }

        var inventoryManager = InventoryManager.Instance;
        if (inventoryManager != null)
        {
            inventoryManager.RemoveItem(interactable.requiredItemId);
        }
    }

    public static void MarkConsumedIfNeeded(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null || !interactable.consumeAfterUse)
        {
            return;
        }

        onConsumed?.Invoke(interactable.id);
    }
}
