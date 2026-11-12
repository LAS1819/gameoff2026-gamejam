using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public event Action<string, InteractableData> InteractionResolved;

    private readonly HashSet<string> consumedInteractables = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool TryInteract(string interactableId)
    {
        if (string.IsNullOrEmpty(interactableId))
        {
            return false;
        }

        var room = RoomManager.Instance?.CurrentRoom;
        if (room == null)
        {
            return false;
        }

        var interactable = room.interactables.Find(item => item != null && item.id == interactableId);
        if (interactable == null)
        {
            Debug.LogWarning($"Interactable not found: {interactableId}");
            return false;
        }

        return TryInteract(interactable, null);
    }

    public bool TryInteract(InteractableData interactable, GameObject sourceObject = null)
    {
        if (interactable == null || string.IsNullOrEmpty(interactable.id))
        {
            return false;
        }

        if (IsConsumed(interactable.id))
        {
            return false;
        }

        if (RoomManager.Instance != null && !RoomManager.Instance.IsInteractableVisible(interactable))
        {
            if (interactable.id.Equals("bedroom_door", StringComparison.OrdinalIgnoreCase)
                && RoomManager.Instance.CurrentRoom != null
                && RoomManager.Instance.CurrentRoom.id.Equals(GameManager.HallwayRoomId, StringComparison.OrdinalIgnoreCase)
                && (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide))
            {
                UIManager.Instance?.ShowFeedback("Something blocks the way in this reality.", 1.5f);
            }

            return false;
        }

        if (!string.IsNullOrEmpty(interactable.requiredItemId))
        {
            if (InventoryManager.Instance == null || !InventoryManager.Instance.HasItem(interactable.requiredItemId))
            {
                Debug.LogWarning($"Missing required item for {interactable.id}: {interactable.requiredItemId}");
                UIManager.Instance?.ShowFeedback("Required item missing.", 1.5f);
                return false;
            }

            var selectedItemId = InventoryManager.Instance.SelectedItemId;
            if (string.IsNullOrEmpty(selectedItemId) || !selectedItemId.Equals(interactable.requiredItemId, StringComparison.OrdinalIgnoreCase))
            {
                var requiredItem = InventoryManager.Instance.GetItemById(interactable.requiredItemId);
                var requiredName = requiredItem != null && !string.IsNullOrWhiteSpace(requiredItem.displayName)
                    ? requiredItem.displayName
                    : interactable.requiredItemId;

                UIManager.Instance?.ShowFeedback($"Select {requiredName} first.", 1.5f);
                return false;
            }
        }

        var wasResolved = false;
        switch (interactable.interactionType)
        {
            case InteractionType.Pickup:
                wasResolved = ResolvePickup(interactable);
                break;
            case InteractionType.Use:
                wasResolved = ResolveUse(interactable, sourceObject);
                break;
            case InteractionType.Examine:
                wasResolved = ResolveExamine(interactable);
                break;
            case InteractionType.Toggle:
                wasResolved = ResolveToggle(interactable);
                break;
            case InteractionType.Transition:
                wasResolved = ResolveTransition(interactable);
                break;
        }

        if (wasResolved)
        {
            RoomManager.Instance?.RefreshInteractables();
            InteractionResolved?.Invoke(interactable.id, interactable);
        }

        return wasResolved;
    }

    public bool IsConsumed(string interactableId)
    {
        return !string.IsNullOrEmpty(interactableId) && consumedInteractables.Contains(interactableId);
    }

    public void ResetRuntimeState()
    {
        consumedInteractables.Clear();
    }

    private bool ResolvePickup(InteractableData interactable)
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager instance not found.");
            return false;
        }

        var itemId = string.IsNullOrEmpty(interactable.itemId) ? interactable.id : interactable.itemId;
        var itemAdded = InventoryManager.Instance.AddItem(new InventoryItemData
        {
            id = itemId,
            displayName = interactable.displayName,
            description = interactable.description,
            isKeyItem = true
        });

        if (!itemAdded)
        {
            return false;
        }

        UIManager.Instance?.RefreshInventoryUI();
        UIManager.Instance?.ShowFeedback($"Picked up {interactable.displayName}.", 1.5f);

        if (!string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance?.SetFlag(interactable.resultFlag, true);
        }

        MarkConsumed(interactable.id);
        return true;
    }

    private bool ResolveUse(InteractableData interactable, GameObject sourceObject)
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
            MarkConsumed(interactable.id);
        }

        UIManager.Instance?.ShowFeedback($"Used {interactable.displayName}.", 1.5f);
        return true;
    }

    private bool ResolveToggle(InteractableData interactable)
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

    private bool IsLightInteractable(InteractableData interactable)
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

    private bool ResolveLightSwitch(InteractableData interactable)
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

        var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var candidate in allObjects)
        {
            if (candidate == null)
            {
                continue;
            }

            var name = candidate.name;
            var normalized = name.Trim();
            var lower = normalized.ToLowerInvariant();

            if ((lower.Contains("ceiling") && (lower.Contains("lamp") || lower.Contains("light"))) ||
                (lower.Contains("roof") && lower.Contains("lamp")) ||
                lower.Equals("lamp") || lower.Equals("ceilinglamp") || lower.Equals("ceilinglight"))
            {
                return candidate;
            }
        }

        return null;
    }

    private bool ResolveWardrobe(InteractableData interactable, GameObject sourceObject)
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

    private bool ResolveExamine(InteractableData interactable)
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

    private bool ResolveTransition(InteractableData interactable)
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

    private void MarkConsumed(string interactableId)
    {
        if (!string.IsNullOrEmpty(interactableId))
        {
            consumedInteractables.Add(interactableId);
        }
    }

    private static void ApplyInteractableState(string interactableId, int stateIndex)
    {
        var target = FindInteractableObject(interactableId);
        if (target == null)
        {
            return;
        }

        var spriteState = target.GetComponent<SpriteStateController>();
        if (spriteState != null)
        {
            spriteState.SetState(stateIndex);
        }
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

        var interactables = FindObjectsByType<InteractableBehaviour>(FindObjectsSortMode.None);
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
