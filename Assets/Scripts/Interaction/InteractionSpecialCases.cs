using System;
using UnityEngine;

internal static class InteractionSpecialCases
{
    public static bool ResolveMainMenuStart()
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return false;
        }

        if (UIManager.Instance == null)
        {
            Debug.LogError("[InteractionSpecialCases] UIManager instance not found while opening Main Menu intro.");
            return false;
        }

        gameManager.SetFlag(GameFlagNames.MainMenuIntroStarted, true);
        UIManager.Instance.ShowIntro(
            "After six months",
            "For six months, I believed my father had disappeared and would return to explain everything.\n\nToday, a message arrived in his handwriting.",
            "Continue");

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RefreshInteractables();
        }

        return true;
    }

    public static bool ResolveMainMenuSettings()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.UiDenied);
        }
        ShowFeedback("Settings are not available in this prototype.", 1.8f);
        return true;
    }

    public static bool ResolveMainMenuExit()
    {
#if UNITY_EDITOR
        ShowFeedback("Exit is disabled inside the Unity Editor.", 1.8f);
#elif UNITY_WEBGL
        ShowFeedback("Close the browser tab to exit the game.", 2f);
#else
        Application.Quit();
#endif
        return true;
    }

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
            ShowFeedback("The box is ordinary here. There is no handle to reach.", 1.8f);
            return true;
        }

        var inventory = InventoryManager.Instance;
        var gameManager = GameManager.Instance;
        var handleAlreadyCollected = (inventory != null && inventory.HasItem("valve_handle"))
            || (gameManager != null
                && (gameManager.HasFlag(GameFlagNames.ValveHandleFound)
                    || gameManager.HasFlag(GameFlagNames.PressureValveRepaired)));
        if (handleAlreadyCollected)
        {
            ShowFeedback("The box is empty. The handle has already been taken.", 1.8f);
            return true;
        }

        if (inventory != null && !inventory.HasItem("valve_handle"))
        {
            inventory.AddItem(InteractionItemFactory.CreateValveHandle());
        }

        if (gameManager != null)
        {
            gameManager.SetFlag(GameFlagNames.BasementEvidenceFound, true);
            gameManager.SetFlag(GameFlagNames.ValveHandleFound, true);
        }

        ShowFeedback("Inside the box, I find the Valve Handle.", 2f);
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
            ShowFeedback("The valve does not exist in this reality.", 1.5f);
            return true;
        }

        InteractionOutcomeProcessor.ApplyUseOutcome(interactable, onConsumed);
        ShowFeedback("The Valve Handle locks into place. Pressure begins to escape.", 2f);

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
            ShowFeedback("The steam burns here. I cannot pass through it.", 2f);
            return true;
        }

        var gameManager = GameManager.Instance;
        var canEnterForest = gameManager != null
            && gameManager.HasFlag(GameFlagNames.PressureValveRepaired)
            && gameManager.HasFlag(GameFlagNames.SteamEnabled)
            && gameManager.HasFlag(GameFlagNames.DenialManifestationRejected);
        if (!canEnterForest)
        {
            ShowFeedback("The exit is still blocked.", 1.8f);
            return true;
        }

        ShowFeedback("The path is clear now. A door to the forest waits beyond.", 2f);
        gameManager.SetFlag(GameFlagNames.ForestExitUnlocked, true);

        return RoomManager.Instance != null
            && RoomManager.Instance.LoadRoom(GameManager.ForestRoomId);
    }

    public static bool ResolveForestAdvance()
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return false;
        }

        if (gameManager.ForestDepthLevel >= 6)
        {
            ShowFeedback("The path ends here. The final room waits.", 2f);
            return true;
        }

        var previousDepth = gameManager.ForestDepthLevel;
        var advanced = gameManager.AdvanceForestDepth();
        if (!advanced)
        {
            return true;
        }

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RefreshForestDepth();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.RoomTransition);
        }

        var newDepth = gameManager.ForestDepthLevel;
        if (newDepth >= 6)
        {
            ShowFeedback("The forest disappears. A table waits in the darkness.", 2f);
        }
        else
        {
            ShowFeedback("The trees move closer.", 1f);
        }

        Debug.Log($"[ForestAdvance] Depth changed from {previousDepth} to {newDepth}.");
        return true;
    }

    public static bool ResolveForestRevelationLetter(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.SetFlag(GameFlagNames.ForestLetterRead, true);
        }

        var memoryCount = gameManager != null
            ? gameManager.CountRecoveredMemories()
            : 0;
        var letter = ResolveForestLetter(memoryCount);
        if (UIManager.Instance == null)
        {
            Debug.LogWarning("[InteractionSpecialCases] UIManager instance not found while opening the Forest letter.");
            return true;
        }

        UIManager.Instance.ShowStoryOverlay(
            letter.title,
            letter.body,
            "Continue",
            StoryOverlayMode.Letter);
        return true;
    }

    private static (string title, string body) ResolveForestLetter(int memoryCount)
    {
        if (memoryCount <= 2)
        {
            return (
                "A letter unfinished",
                "My son, if you are reading this it is because you kept searching for an answer I could not give you. I still cannot ask you to accept what happened. Allow me to stay a little longer in the lie.\n\nI am not ready to say goodbye.");
        }

        if (memoryCount <= 4)
        {
            return (
                "A letter incomplete",
                "My son, I know you found part of the truth. You understood that something terrible happened, but there is still a pain that no one knew how to name.\n\nYou do not know how much I regret leaving you with a farewell half-done. You understood too late that staying together does not always mean continuing to search.");
        }

        return (
            "To my son",
            "My son, if you have come this far, you have gathered the pieces I was not able to deliver. There is no need to look for me on the path or among the lights anymore.\n\nAccept what happened, also accept what you are now, and let this letter be the farewell we have been waiting for six months. It is time to go together.");
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

        ShowFeedback("The code is accepted. The Basement Door is now accessible.", 2f);
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

    private static void ShowFeedback(string message, float duration)
    {
        if (UIManager.Instance == null)
        {
            return;
        }

        UIManager.Instance.ShowFeedback(message, duration);
    }
}
