using System;
using UnityEngine;

internal static class InteractionSpecialCases
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

    public static bool ResolveWardrobe(InteractableData interactable, GameObject sourceObject)
    {
        if (interactable == null)
        {
            return false;
        }

        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return false;
        }

        WardrobeController wardrobeController = null;
        if (sourceObject != null)
        {
            wardrobeController = sourceObject.GetComponent<WardrobeController>();
            if (wardrobeController == null)
            {
                wardrobeController = sourceObject.GetComponentInParent<WardrobeController>();
            }
        }

        if (wardrobeController == null)
        {
            var wardrobeObject = FindInteractableObject(interactable.id);
            if (wardrobeObject != null)
            {
                wardrobeController = wardrobeObject.GetComponent<WardrobeController>();
                if (wardrobeController == null)
                {
                    wardrobeController = wardrobeObject.GetComponentInParent<WardrobeController>();
                }
            }
        }

        if (wardrobeController == null)
        {
            Debug.LogWarning($"[InteractionManager] WardrobeController not found for interactable '{interactable.id}'.");
            return false;
        }

        var reality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        var isOtherSide = reality == RealityType.OtherSide;
        var isLightOn = gameManager.HasFlag(GameFlagNames.BedroomLightOn);

        var isAlreadyOpen = wardrobeController.IsOpen;
        if (isAlreadyOpen)
        {
            wardrobeController.SetOpen(false);
            gameManager.SetFlag(GameFlagNames.BedroomWardrobeOpened, false);
            var ui = UIManager.Instance;
            if (ui != null)
            {
                ui.ShowFeedback("You close the wardrobe.", 1.5f);
            }

            return true;
        }

        if (isOtherSide && !isLightOn)
        {
            wardrobeController.SetOpen(true);
            gameManager.SetFlag(GameFlagNames.BedroomWardrobeOpened, true);
            var ui = UIManager.Instance;
            if (ui != null)
            {
                ui.ShowFeedback("You open the wardrobe. Something is moving inside.", 1.9f);
            }

            return true;
        }

        wardrobeController.SetOpen(true);
        gameManager.SetFlag(GameFlagNames.BedroomWardrobeOpened, true);

        if (isOtherSide && isLightOn)
        {
            var ui = UIManager.Instance;
            if (ui != null)
            {
                ui.ShowFeedback("You open the wardrobe. Something glints inside.", 1.8f);
            }

            return true;
        }

        var hud = UIManager.Instance;
        if (hud != null)
        {
            hud.ShowFeedback("You open the wardrobe.", 1.5f);
        }

        return true;
    }

    public static bool ResolveWornBookUse(InteractableData interactable, Action<string> onConsumed)
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager instance not found.");
            return false;
        }

        var item = new InventoryItemData
        {
            id = "worn_book",
            displayName = "Worn Book",
            description = "A worn book with a hidden compartment in its back pages.",
            descriptionInNormalReality = "The cover is worn, but the pages feel preserved, as if the book has been carefully kept from the world.",
            descriptionInOtherReality = "The pages are not just old: they feel archived, as if they remember a version of the house that still exists somewhere else.",
            isKeyItem = true
        };

        var itemAdded = InventoryManager.Instance.AddItem(item);
        if (!itemAdded)
        {
            var ui = UIManager.Instance;
            if (ui != null)
            {
                ui.ShowFeedback("You already have the Worn Book.", 1.5f);
            }

            return false;
        }

        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.SetFlag(GameFlagNames.WornBookFound, true);
        }

        var hud = UIManager.Instance;
        if (hud != null)
        {
            hud.RefreshInventoryUI();
            hud.ShowFeedback("You take the Worn Book and add it to your inventory.", 1.8f);
        }

        InteractionOutcomeProcessor.MarkConsumedIfNeeded(interactable, onConsumed);
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

            if ((containsCeiling && (containsLamp || containsLight)) ||
                (containsRoof && containsLamp) ||
                lower == "lamp" ||
                lower == "ceilinglamp" ||
                lower == "ceilinglight")
            {
                return candidate;
            }
        }

        return null;
    }

    private static GameObject FindInteractableObject(string interactableId)
    {
        if (string.IsNullOrEmpty(interactableId))
        {
            return null;
        }

        var currentRoom = RoomManager.Instance != null ? RoomManager.Instance.CurrentRoom : null;
        if (currentRoom != null)
        {
            var roomRoot = FindRoomRoot(currentRoom.id);
            if (roomRoot != null)
            {
                foreach (var behaviour in roomRoot.GetComponentsInChildren<InteractableBehaviour>(true))
                {
                    if (behaviour != null && behaviour.GetData() != null &&
                        behaviour.GetData().id.Equals(interactableId, StringComparison.OrdinalIgnoreCase))
                    {
                        return behaviour.gameObject;
                    }
                }
            }
        }

        var directMatch = GameObject.Find(interactableId);
        if (directMatch != null)
        {
            return directMatch;
        }

        var interactables = UnityEngine.Object.FindObjectsByType<InteractableBehaviour>();
        foreach (var behaviour in interactables)
        {
            if (behaviour == null || behaviour.GetData() == null)
            {
                continue;
            }

            if (behaviour.GetData().id.Equals(interactableId, StringComparison.OrdinalIgnoreCase))
            {
                return behaviour.gameObject;
            }
        }

        return null;
    }

    private static GameObject FindRoomRoot(string roomId)
    {
        if (string.IsNullOrEmpty(roomId))
        {
            return null;
        }

        var roomsParent = GameObject.Find("Main/Rooms");
        if (roomsParent == null)
        {
            roomsParent = GameObject.Find("Rooms");
        }

        if (roomsParent == null)
        {
            return null;
        }

        foreach (Transform child in roomsParent.transform)
        {
            if (child.gameObject.name.Equals(roomId, StringComparison.OrdinalIgnoreCase))
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
