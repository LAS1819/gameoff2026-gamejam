using System;
using UnityEngine;

internal static class InteractionResolver
{
    public static bool ResolveExamineOnly(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        if (interactable.id.Equals("bedroom_computer_monitor", StringComparison.OrdinalIgnoreCase))
        {
            return InteractionSpecialCases.ResolveBedroomComputerMonitor(interactable, null);
        }

        return InteractionExamineTransitionProcessor.ResolveExamine(interactable);
    }

    public static bool Resolve(InteractableData interactable, GameObject sourceObject, Action<string> onConsumed)
    {
        if (interactable == null)
        {
            return false;
        }

        switch (interactable.interactionType)
        {
            case InteractionType.Pickup:
                return ResolvePickup(interactable, onConsumed);
            case InteractionType.Use:
                return ResolveUse(interactable, sourceObject, onConsumed);
            case InteractionType.Examine:
                if (interactable.id.Equals("bedroom_computer_monitor", StringComparison.OrdinalIgnoreCase))
                {
                    return InteractionSpecialCases.ResolveBedroomComputerMonitor(interactable, sourceObject);
                }

                return InteractionExamineTransitionProcessor.ResolveExamine(interactable);
            case InteractionType.Toggle:
                return ResolveToggle(interactable);
            case InteractionType.Transition:
                return InteractionExamineTransitionProcessor.ResolveTransition(interactable, onConsumed);
            default:
                return false;
        }
    }

    private static void ClearSelectedItemIfNeeded()
    {
        var inventory = InventoryManager.Instance;
        if (inventory != null && !string.IsNullOrEmpty(inventory.SelectedItemId))
        {
            inventory.DeselectSelectedItem();
        }
    }

    private static bool ResolvePickup(InteractableData interactable, Action<string> onConsumed)
    {
        var itemId = string.IsNullOrEmpty(interactable.itemId) ? interactable.id : interactable.itemId;
        var isLanternPickup = itemId.Equals("flashlight_tool", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("nightstand", StringComparison.OrdinalIgnoreCase);

        if (isLanternPickup)
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.RemoveItem("flashlight_tool");
            }

            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.SetFlag(GameFlagNames.HasFlashlight, true);
            }

            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.RefreshInventoryUI();
            }

            var feedbackMessage = "You find the flashlight on the nightstand. No need to use it now.";
            if (uiManager != null)
            {
                uiManager.ShowFeedback(feedbackMessage, 1.8f);
            }

            if (interactable.interactionType == InteractionType.Examine && !string.IsNullOrEmpty(interactable.resultFlag))
            {
                if (gameManager != null)
                {
                    gameManager.SetFlag(interactable.resultFlag, true);
                }
            }

            if (interactable.consumeAfterUse && onConsumed != null)
            {
                onConsumed(interactable.id);
            }

            return true;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager instance not found.");
            return false;
        }

        var itemAdded = InventoryManager.Instance.AddItem(new InventoryItemData
        {
            id = itemId,
            displayName = interactable.displayName,
            description = interactable.description,
            descriptionInNormalReality = interactable.descriptionInNormalReality,
            descriptionInOtherReality = interactable.descriptionInOtherReality,
            isKeyItem = true
        });

        if (!itemAdded)
        {
            return false;
        }

        var hud = UIManager.Instance;
        if (hud != null)
        {
            hud.RefreshInventoryUI();
        }

        var pickupMessage = $"Picked up {interactable.displayName}.";
        if (hud != null)
        {
            hud.ShowFeedback(pickupMessage, 1.8f);
        }

        InteractionOutcomeProcessor.ApplyPickupOutcome(interactable, onConsumed);
        return true;
    }

    private static bool ResolveUse(InteractableData interactable, GameObject sourceObject, Action<string> onConsumed)
    {
        if (InteractionSpecialCases.IsLightInteractable(interactable))
        {
            return InteractionSpecialCases.ResolveLightSwitch(interactable);
        }

        if (interactable.id.Equals("wardrobe", StringComparison.OrdinalIgnoreCase))
        {
            return InteractionSpecialCases.ResolveWardrobe(interactable, sourceObject);
        }

        if (interactable.id.Equals("bedroom_computer_tower", StringComparison.OrdinalIgnoreCase))
        {
            return InteractionSpecialCases.ResolveBedroomComputerTower(interactable, sourceObject);
        }

        if (interactable.id.Equals("bedroom_computer_monitor", StringComparison.OrdinalIgnoreCase))
        {
            return InteractionSpecialCases.ResolveBedroomComputerMonitor(interactable, sourceObject);
        }

        if (interactable.id.Equals("shelf_interactive", StringComparison.OrdinalIgnoreCase))
        {
            return InteractionSpecialCases.ResolveShelfInteractive(interactable, onConsumed);
        }

        if (interactable.id.Equals("worn_book", StringComparison.OrdinalIgnoreCase))
        {
            return InteractionSpecialCases.ResolveWornBookUse(interactable, onConsumed);
        }

        ClearSelectedItemIfNeeded();
        InteractionOutcomeProcessor.ApplyUseOutcome(interactable, onConsumed);

        var uiManager = UIManager.Instance;
        if (uiManager != null)
        {
            uiManager.ShowFeedback($"Used {interactable.displayName}.", 1.5f);
        }

        return true;
    }

    private static bool ResolveToggle(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        if (InteractionSpecialCases.IsLightInteractable(interactable))
        {
            return InteractionSpecialCases.ResolveLightSwitch(interactable);
        }

        if (interactable.id.Equals("shift_trigger", StringComparison.OrdinalIgnoreCase))
        {
            var realityManager = RealityManager.Instance;
            if (realityManager != null)
            {
                realityManager.ToggleReality();
            }

            if (!string.IsNullOrEmpty(interactable.resultFlag))
            {
                var gameManager = GameManager.Instance;
                if (gameManager != null)
                {
                    gameManager.SetFlag(interactable.resultFlag, true);
                }
            }

            return true;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.SetFlag(interactable.resultFlag, true);
            }
        }

        return true;
    }
}
