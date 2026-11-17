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

    public static bool IsLightInteractable(InteractableData interactable)
    {
        return InteractionLightCases.IsLightInteractable(interactable);
    }
}
