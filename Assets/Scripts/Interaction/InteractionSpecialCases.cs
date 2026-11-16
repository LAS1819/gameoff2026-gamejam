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

    public static bool IsLightInteractable(InteractableData interactable)
    {
        return InteractionLightCases.IsLightInteractable(interactable);
    }
}
