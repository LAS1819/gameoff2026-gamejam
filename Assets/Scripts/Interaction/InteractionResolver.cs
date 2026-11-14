using System;
using UnityEngine;

internal static class InteractionResolver
{
    public static bool ResolveExamineOnly(InteractableData interactable)
    {
        return ResolveExamine(interactable);
    }

    public static bool Resolve(InteractableData interactable, GameObject sourceObject, Action<string> onConsumed)
    {
        switch (interactable.interactionType)
        {
            case InteractionType.Pickup:
                return ResolvePickup(interactable, onConsumed);
            case InteractionType.Use:
                return ResolveUse(interactable, sourceObject, onConsumed);
            case InteractionType.Examine:
                return ResolveExamine(interactable);
            case InteractionType.Toggle:
                return ResolveToggle(interactable);
            case InteractionType.Transition:
                return ResolveTransition(interactable);
            default:
                return false;
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

            GameManager.Instance?.SetFlag(GameFlagNames.HasFlashlight, true);
            UIManager.Instance?.RefreshInventoryUI();

            var feedbackMessage = "You find the flashlight on the nightstand. No need to use it now.";
            UIManager.Instance?.ShowFeedback(feedbackMessage, 1.8f);

            if (interactable.interactionType == InteractionType.Examine &&
                !string.IsNullOrEmpty(interactable.resultFlag))
            {
                GameManager.Instance?.SetFlag(interactable.resultFlag, true);
            }

            if (interactable.consumeAfterUse)
            {
                onConsumed?.Invoke(interactable.id);
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

        UIManager.Instance?.RefreshInventoryUI();

        var pickupMessage = $"Picked up {interactable.displayName}.";
        UIManager.Instance?.ShowFeedback(pickupMessage, 1.8f);

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        if (interactable.consumeAfterUse)
        {
            onConsumed?.Invoke(interactable.id);
        }

        return true;
    }

    private static bool ResolveUse(InteractableData interactable, GameObject sourceObject, Action<string> onConsumed)
    {
        if (IsLightInteractable(interactable))
        {
            return ResolveLightSwitch(interactable);
        }

        if (interactable.id.Equals("wardrobe", StringComparison.OrdinalIgnoreCase))
        {
            return ResolveWardrobe(interactable, sourceObject);
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        if (interactable.consumeAfterUse)
        {
            onConsumed?.Invoke(interactable.id);
        }

        UIManager.Instance?.ShowFeedback($"Used {interactable.displayName}.", 1.5f);
        return true;
    }

    private static bool ResolveToggle(InteractableData interactable)
    {
        if (IsLightInteractable(interactable))
        {
            return ResolveLightSwitch(interactable);
        }

        if (interactable != null && interactable.id.Equals("shift_trigger", StringComparison.OrdinalIgnoreCase))
        {
            RealityManager.Instance?.ToggleReality();

            if (!string.IsNullOrEmpty(interactable.resultFlag))
            {
                GameManager.Instance?.SetFlag(interactable.resultFlag, true);
            }

            return true;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        return true;
    }

    private static bool ResolveExamine(InteractableData interactable)
    {
        if (interactable != null &&
            (interactable.id == "library_door" || interactable.id == "bedroom_door") &&
            (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide))
        {
            Debug.LogWarning($"[InteractionManager] '{interactable.displayName}' can only be used in the Other Side reality.");
            UIManager.Instance?.ShowFeedback("This is only accessible in the Other Side.", 1.5f);
            return false;
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        var currentReality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        var descriptionText = interactable.GetDescriptionForReality(currentReality);
        if (string.IsNullOrWhiteSpace(descriptionText))
        {
            descriptionText = $"You inspected {interactable.displayName}.";
        }

        Debug.Log($"[InteractionManager] Examine: {interactable.displayName} - {descriptionText}");
        UIManager.Instance?.ShowFeedback(descriptionText, 2f);
        return true;
    }

    private static bool ResolveTransition(InteractableData interactable)
    {
        if (string.IsNullOrEmpty(interactable.linkedRoomId))
        {
            Debug.LogError($"[InteractionManager] Transition '{interactable.id}' has no linkedRoomId set.");
            return false;
        }

        Debug.Log($"[InteractionManager] ResolveTransition: attempting to load room '{interactable.linkedRoomId}'");

        if (RoomManager.Instance == null)
        {
            Debug.LogError($"[InteractionManager] RoomManager instance not found. Cannot transition to '{interactable.linkedRoomId}'");
            return false;
        }

        var success = RoomManager.Instance.LoadRoom(interactable.linkedRoomId);
        Debug.Log($"[InteractionManager] LoadRoom('{interactable.linkedRoomId}') returned: {success}");
        return success;
    }

    private static bool ResolveWardrobe(InteractableData interactable, GameObject sourceObject)
    {
        if (GameManager.Instance == null)
        {
            return false;
        }

        var wardrobeController = sourceObject != null
            ? sourceObject.GetComponent<WardrobeController>() ?? sourceObject.GetComponentInParent<WardrobeController>()
            : null;

        if (wardrobeController == null)
        {
            var wardrobeObject = FindInteractableObject(interactable.id);
            if (wardrobeObject != null)
            {
                wardrobeController = wardrobeObject.GetComponent<WardrobeController>() ?? wardrobeObject.GetComponentInParent<WardrobeController>();
            }
        }

        if (wardrobeController == null)
        {
            Debug.LogWarning($"[InteractionManager] WardrobeController not found for interactable '{interactable.id}'.");
            return false;
        }

        var reality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        var isOtherSide = reality == RealityType.OtherSide;
        var isLightOn = GameManager.Instance.HasFlag(GameFlagNames.BedroomLightOn);

        var isAlreadyOpen = wardrobeController.IsOpen;
        if (isAlreadyOpen)
        {
            wardrobeController.SetOpen(false);
            GameManager.Instance.SetFlag(GameFlagNames.BedroomWardrobeOpened, false);
            UIManager.Instance?.ShowFeedback("You close the wardrobe.", 1.5f);
            return true;
        }

        if (isOtherSide && !isLightOn)
        {
            wardrobeController.SetOpen(true);
            GameManager.Instance.SetFlag(GameFlagNames.BedroomWardrobeOpened, true);
            UIManager.Instance?.ShowFeedback("You open the wardrobe. Something is moving inside.", 1.9f);
            return true;
        }

        wardrobeController.SetOpen(true);
        GameManager.Instance.SetFlag(GameFlagNames.BedroomWardrobeOpened, true);

        if (isOtherSide && isLightOn)
        {
            UIManager.Instance?.ShowFeedback("You open the wardrobe. Something glints inside.", 1.8f);
            return true;
        }

        UIManager.Instance?.ShowFeedback("You open the wardrobe.", 1.5f);
        return true;
    }

    private static bool ResolveLightSwitch(InteractableData interactable)
    {
        var currentReality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        if (currentReality != RealityType.Normal)
        {
            UIManager.Instance?.ShowFeedback("The switch only works in the normal reality.", 1.6f);
            return false;
        }

        var target = FindLightTargetObject();
        var spriteState = target != null
            ? target.GetComponent<SpriteStateController>() ?? target.GetComponentInChildren<SpriteStateController>()
            : null;

        var isLightOn = GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.BedroomLightOn);
        var nextState = !isLightOn;

        if (spriteState != null)
        {
            spriteState.SetState(nextState ? 1 : 0);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BedroomLightOn, nextState);
        }

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, nextState);
        }

        UIManager.Instance?.ShowFeedback(nextState ? "You switch on the ceiling light." : "You switch off the ceiling light.", 1.75f);
        return true;
    }

    private static bool IsLightInteractable(InteractableData interactable)
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

        var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var candidate in allObjects)
        {
            if (candidate == null)
            {
                continue;
            }

            string normalized = candidate.name.Trim();
            string lower = normalized.ToLowerInvariant();

            bool containsCeiling = lower.Contains("ceiling");
            bool containsRoof = lower.Contains("roof");
            bool containsLamp = lower.Contains("lamp");
            bool containsLight = lower.Contains("light");

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

        var currentRoom = RoomManager.Instance?.CurrentRoom;
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

        var interactables = UnityEngine.Object.FindObjectsOfType<InteractableBehaviour>();
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

        var roomsParent = GameObject.Find("Main/Rooms") ?? GameObject.Find("Rooms");
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
