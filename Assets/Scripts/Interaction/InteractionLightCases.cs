using System;
using UnityEngine;

internal static class InteractionLightCases
{
    public static bool ResolveLightSwitch(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        var realityManager = RealityManager.Instance;
        var currentReality = realityManager != null ? realityManager.CurrentReality : RealityType.Normal;
        if (currentReality != RealityType.Normal)
        {
            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.ShowFeedback("The switch only works in the normal reality.", 1.6f);
            }

            return false;
        }

        var target = FindLightTargetObject();
        SpriteStateController spriteState = null;
        if (target != null)
        {
            spriteState = target.GetComponent<SpriteStateController>();
            if (spriteState == null)
            {
                spriteState = target.GetComponentInChildren<SpriteStateController>();
            }
        }

        var gameManager = GameManager.Instance;
        var isLightOn = gameManager != null && gameManager.HasFlag(GameFlagNames.BedroomLightOn);
        var nextState = !isLightOn;

        if (spriteState != null)
        {
            spriteState.SetState(nextState ? 1 : 0);
        }

        if (target != null)
        {
            var beam = target.transform.Find("CeilingBeamLight");
            if (beam == null)
            {
                foreach (var child in target.GetComponentsInChildren<Transform>(true))
                {
                    if (child != null
                        && child.name.Equals("CeilingBeamLight", StringComparison.OrdinalIgnoreCase))
                    {
                        beam = child;
                        break;
                    }
                }
            }

            if (beam != null)
            {
                beam.gameObject.SetActive(nextState);
            }
        }

        if (gameManager != null)
        {
            gameManager.SetFlag(GameFlagNames.BedroomLightOn, nextState);
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag) && gameManager != null)
        {
            gameManager.SetFlag(interactable.resultFlag, nextState);
        }

        var ui = UIManager.Instance;
        if (ui != null)
        {
            ui.ShowFeedback(nextState ? "You switch on the ceiling light." : "You switch off the ceiling light.", 1.75f);
        }

        return true;
    }

    public static bool IsLightInteractable(InteractableData interactable)
    {
        if (interactable == null || string.IsNullOrEmpty(interactable.id))
        {
            return false;
        }

        return interactable.id.Equals("switch", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("lamp", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("ceiling_light", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("ceiling-light", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("roof_lamp", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("roof-lamp", StringComparison.OrdinalIgnoreCase);
    }

    private static GameObject FindLightTargetObject()
    {
        var candidateNames = new[]
        {
            "CeilingLamp",
            "CeilingLight",
            "ceiling_lamp",
            "ceiling_light",
            "ceiling-light",
            "Lamp",
            "lamp",
            "RoofLamp",
            "roof_lamp",
            "roof-lamp"
        };

        foreach (var candidate in candidateNames)
        {
            var found = GameObject.Find(candidate);
            if (found != null)
            {
                return found;
            }
        }

        var allObjects = UnityEngine.Object.FindObjectsByType<GameObject>();
        foreach (var candidate in allObjects)
        {
            if (candidate == null)
            {
                continue;
            }

            var normalized = candidate.name.Trim();
            var lower = normalized.ToLowerInvariant();

            var containsCeiling = lower.Contains("ceiling");
            var containsRoof = lower.Contains("roof");
            var containsLamp = lower.Contains("lamp");
            var containsLight = lower.Contains("light");

            if ((containsCeiling && (containsLamp || containsLight))
                || (containsRoof && containsLamp)
                || lower == "lamp"
                || lower == "ceilinglamp"
                || lower == "ceilinglight")
            {
                return candidate;
            }
        }

        return null;
    }
}
