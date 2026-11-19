using System;
using UnityEngine;

internal static class InteractionSpecialCases
{
    public static bool ResolveLightSwitch(InteractableData interactable)
    {
        return InteractionLightCases.ResolveLightSwitch(interactable);
    }

    public static bool ResolveWardrobe(InteractableData interactable, GameObject sourceObject)
    {
        return InteractionWardrobeCases.ResolveWardrobe(interactable, sourceObject);
    }

    public static bool ResolveShelfInteractive(InteractableData interactable, Action<string> onConsumed)
    {
        return InteractionShelfCases.ResolveShelfInteractive(interactable, onConsumed);
    }

    public static bool ResolveWornBookUse(InteractableData interactable, Action<string> onConsumed)
    {
        return InteractionShelfCases.ResolveWornBookUse(interactable, onConsumed);
    }

    public static bool ResolveBedroomComputerTower(InteractableData interactable, GameObject sourceObject)
    {
        return InteractionBedroomComputerCases.ResolveTower(interactable, sourceObject);
    }

    public static bool ResolveBedroomComputerMonitor(InteractableData interactable, GameObject sourceObject)
    {
        return InteractionBedroomComputerCases.ResolveMonitor(interactable, sourceObject);
    }

    public static bool ResolvePersonalEffectsBox(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        if (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide)
        {
            UIManager.Instance?.ShowFeedback("The box is ordinary here. There is no handle to reach.", 1.8f);
            return true;
        }

        var inventory = InventoryManager.Instance;
        var gameManager = GameManager.Instance;
        if (inventory != null && !inventory.HasItem("valve_handle"))
        {
            inventory.AddItem(InteractionItemFactory.CreateValveHandle());
        }

        if (gameManager != null)
        {
            gameManager.SetFlag(GameFlagNames.BasementEvidenceFound, true);
            gameManager.SetFlag(GameFlagNames.ValveHandleFound, true);
        }

        UIManager.Instance?.ShowFeedback("Inside the box, I find the Valve Handle.", 2f);
        return true;
    }

    public static bool ResolvePressureReleaseValve(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null)
        {
            return false;
        }

        if (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.Normal)
        {
            UIManager.Instance?.ShowFeedback("The valve does not exist in this reality.", 1.5f);
            return true;
        }

        InteractionOutcomeProcessor.ApplyUseOutcome(interactable, onConsumed);
        UIManager.Instance?.ShowFeedback("The Valve Handle locks into place. Pressure begins to escape.", 2f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.SteamEnabled, true);
            TryResolveBasementProgress();
        }

        return true;
    }

    public static bool ResolveForestExit(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        var reality = RealityManager.Instance != null
            ? RealityManager.Instance.CurrentReality
            : RealityType.Normal;

        if (reality == RealityType.OtherSide)
        {
            UIManager.Instance?.ShowFeedback("The steam burns here. I cannot pass through it.", 2f);
            return true;
        }

        UIManager.Instance?.ShowFeedback("The path is clear now. A door to the forest waits beyond.", 2f);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.ForestExitUnlocked, true);
        }

        return true;
    }

    public static bool ResolveCodePanel(InteractableData interactable, Action<string> onConsumed)
    {
        if (interactable == null)
        {
            return false;
        }

        InteractionOutcomeProcessor.ApplyUseOutcome(interactable, onConsumed);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BasementCodeUsed, true);
            GameManager.Instance.SetFlag(GameFlagNames.BasementUnlocked, true);
            GameManager.Instance.SetFlag(GameFlagNames.BasementDoorVisible, true);
        }

        UIManager.Instance?.ShowFeedback("The code is accepted. The Basement Door is now accessible.", 2f);
        return true;
    }

    private static void TryResolveBasementProgress()
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return;
        }

        var resolved = gameManager.HasFlag(GameFlagNames.BasementEvidenceFound)
            && gameManager.HasFlag(GameFlagNames.PressureValveRepaired)
            && gameManager.HasFlag(GameFlagNames.SteamEnabled);

        gameManager.SetFlag(GameFlagNames.BasementResolved, resolved);
        gameManager.SetFlag(GameFlagNames.BasementExitUnlocked, resolved);
        gameManager.SetFlag(GameFlagNames.ForestExitUnlocked, resolved);
    }

    public static bool IsLightInteractable(InteractableData interactable)
    {
        return InteractionLightCases.IsLightInteractable(interactable);
    }
}
