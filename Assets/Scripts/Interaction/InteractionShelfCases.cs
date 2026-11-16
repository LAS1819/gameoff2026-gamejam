using System;

internal static class InteractionShelfCases
{
    private const string WornBookItemId = "worn_book";

    public static bool ResolveShelfInteractive(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null)
        {
            return false;
        }

        var currentReality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        var gameManager = GameManager.Instance;
        var inventory = InventoryManager.Instance;
        var shelfActivated = gameManager != null && gameManager.HasFlag(GameFlagNames.LibraryShelfActivated);

        if (currentReality == RealityType.Normal)
        {
            if (shelfActivated)
            {
                ShowFeedback("The shelf no longer reveals anything.", 1.5f);
                return true;
            }

            ShowFeedback("There is nothing special about the shelf here.", 1.5f);
            return true;
        }

        if (inventory == null)
        {
            return false;
        }

        if (shelfActivated)
        {
            if (IsDifferentItemSelected(inventory))
            {
                ShowFeedback("That item does not belong here.", 1.5f);
                return true;
            }

            if (!inventory.HasItem(WornBookItemId))
            {
                inventory.AddItem(InteractionItemFactory.CreateWornBook());
                if (gameManager != null)
                {
                    gameManager.SetFlag(GameFlagNames.LibraryShelfActivated, false);
                    gameManager.SetFlag(GameFlagNames.LibrarySecretDoorOpen, false);
                    gameManager.SetFlag(GameFlagNames.BasementDoorVisible, false);
                    gameManager.SetFlag(GameFlagNames.LibrarySecretRevealed, false);
                }

                ShowFeedback("You take the Worn Book back from the shelf.", 1.8f);
                return true;
            }

            ShowFeedback("The shelf already holds the Worn Book.", 1.5f);
            return true;
        }

        if (!inventory.HasItem(WornBookItemId))
        {
            ShowFeedback("I think I will need something to use...", 1.6f);
            return true;
        }

        if (!IsWornBookSelected(inventory))
        {
            ShowFeedback("Select the Worn Book first.", 1.6f);
            return true;
        }

        inventory.RemoveItem(WornBookItemId);
        if (gameManager != null)
        {
            gameManager.SetFlag(GameFlagNames.LibraryShelfActivated, true);
            gameManager.SetFlag(GameFlagNames.LibrarySecretDoorOpen, true);
            gameManager.SetFlag(GameFlagNames.LibrarySecretRevealed, true);
            gameManager.SetFlag(GameFlagNames.BasementDoorVisible, true);
        }

        ShowFeedback("The shelf accepts the Worn Book and the hidden space settles back into place.", 1.8f);
        return true;
    }

    public static bool ResolveWornBookUse(InteractableData interactable, Action<string> onConsumed)
    {
        var inventory = InventoryManager.Instance;
        if (inventory == null)
        {
            return false;
        }

        var itemAdded = inventory.AddItem(InteractionItemFactory.CreateWornBook());
        if (!itemAdded)
        {
            ShowFeedback("You already have the Worn Book.", 1.5f);
            return false;
        }

        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.SetFlag(GameFlagNames.WornBookFound, true);
        }

        var uiManager = UIManager.Instance;
        if (uiManager != null)
        {
            uiManager.RefreshInventoryUI();
            uiManager.ShowFeedback("You take the Worn Book and add it to your inventory.", 1.8f);
        }

        InteractionOutcomeProcessor.MarkConsumedIfNeeded(interactable, onConsumed);
        return true;
    }

    private static bool IsWornBookSelected(InventoryManager inventory)
    {
        return !string.IsNullOrEmpty(inventory.SelectedItemId)
            && inventory.SelectedItemId.Equals(WornBookItemId, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsDifferentItemSelected(InventoryManager inventory)
    {
        if (string.IsNullOrEmpty(inventory.SelectedItemId))
        {
            return false;
        }

        return !inventory.SelectedItemId.Equals(WornBookItemId, StringComparison.OrdinalIgnoreCase);
    }

    private static void ShowFeedback(string message, float duration)
    {
        var ui = UIManager.Instance;
        if (ui != null)
        {
            ui.ShowFeedback(message, duration);
        }
    }
}
