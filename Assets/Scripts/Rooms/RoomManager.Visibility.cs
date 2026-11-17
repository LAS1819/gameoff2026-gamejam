using System;
using UnityEngine;

public partial class RoomManager
{
    public bool IsInteractableVisible(InteractableData interactable)
    {
        if (interactable == null)
        {
            return false;
        }

        if (CurrentRoom == null || !IsInteractableInCurrentRoom(interactable.id))
        {
            return false;
        }

        if (InteractionManager.Instance != null && InteractionManager.Instance.IsConsumed(interactable.id))
        {
            return false;
        }

        if (interactable.id == "staff_key" && GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.HasStaffKey))
        {
            return false;
        }

        if (interactable.id == "front_door" && GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.DoorOpened))
        {
            return false;
        }

        if (interactable.id == "hallway_entrance" && GameManager.Instance != null && !GameManager.Instance.HasFlag(GameFlagNames.DoorOpened))
        {
            return false;
        }

        if (interactable.id == "flashlight_tool" && GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.HasFlashlight))
        {
            return false;
        }

        if (interactable.id == "library_key" && GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.HasLibraryKey))
        {
            return false;
        }

        if (interactable.id == "library_key" && GameManager.Instance != null)
        {
            if (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide)
            {
                return false;
            }

            if (!GameManager.Instance.HasFlag(GameFlagNames.BedroomWardrobeOpened))
            {
                return false;
            }

            if (!GameManager.Instance.HasFlag(GameFlagNames.BedroomLightOn))
            {
                return false;
            }
        }

        if (interactable.id.Equals("worn_book", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null
            && GameManager.Instance.HasFlag(GameFlagNames.WornBookFound))
        {
            return false;
        }

        if (interactable.id.Equals("library_door", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("bedroom_door", StringComparison.OrdinalIgnoreCase))
        {
            if (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide)
            {
                return false;
            }
        }

        if (interactable.id.Equals("shelf_secret_door", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if ((interactable.id.Equals("basement_door", StringComparison.OrdinalIgnoreCase)
            || interactable.id.Equals("code_panel", StringComparison.OrdinalIgnoreCase))
            && GameManager.Instance != null)
        {
            var secretDoorOpen = GameManager.Instance.HasFlag(GameFlagNames.LibrarySecretDoorOpen);
            var shelfActivated = GameManager.Instance.HasFlag(GameFlagNames.LibraryShelfActivated);
            if (!secretDoorOpen || !shelfActivated)
            {
                return false;
            }
        }

        if (interactable.id == "wardrobe_monster" && !IsBedroomMonsterConditionMet())
        {
            return false;
        }

        if (interactable.id.Equals("bedroom_door", StringComparison.OrdinalIgnoreCase)
            && CurrentRoom != null
            && CurrentRoom.id.Equals(GameManager.HallwayRoomId, StringComparison.OrdinalIgnoreCase)
            && (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide))
        {
            return false;
        }

        if (RealityManager.Instance != null)
        {
            var isNormalReality = RealityManager.Instance.CurrentReality == RealityType.Normal;
            var shouldBeVisible = isNormalReality
                ? interactable.visibleInNormalReality
                : interactable.visibleInOtherReality;

            if (!shouldBeVisible)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsInteractableInCurrentRoom(string interactableId)
    {
        if (CurrentRoom == null || string.IsNullOrEmpty(interactableId))
        {
            return false;
        }

        var targetId = NormalizeIdentifier(interactableId);
        return CurrentRoom.interactables.Exists(item =>
            item != null
            && NormalizeIdentifier(item.id).Equals(targetId, StringComparison.OrdinalIgnoreCase));
    }

    private bool IsBedroomMonsterConditionMet()
    {
        if (RealityManager.Instance == null || RealityManager.Instance.CurrentReality != RealityType.OtherSide)
        {
            return false;
        }

        if (GameManager.Instance == null)
        {
            return false;
        }

        var wardrobeOpen = GameManager.Instance.HasFlag(GameFlagNames.BedroomWardrobeOpened);
        var lightOn = GameManager.Instance.HasFlag(GameFlagNames.BedroomLightOn);
        return wardrobeOpen && !lightOn;
    }

    private void RefreshConditionalInteractableObjects()
    {
        if (CurrentRoom == null || !CurrentRoom.id.Equals(GameManager.BedroomRoomId, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var roomRoot = FindRoomRootById(CurrentRoom.id);
        if (roomRoot == null)
        {
            return;
        }

        var behaviours = roomRoot.GetComponentsInChildren<InteractableBehaviour>(true);
        foreach (var behaviour in behaviours)
        {
            var data = behaviour != null ? behaviour.GetData() : null;
            if (data == null)
            {
                continue;
            }

            var isConditional = data.id.Equals("library_key", StringComparison.OrdinalIgnoreCase)
                || data.id.Equals("wardrobe_monster", StringComparison.OrdinalIgnoreCase);
            if (!isConditional)
            {
                continue;
            }

            var shouldBeActive = IsInteractableVisible(data);
            if (behaviour.gameObject.activeSelf != shouldBeActive)
            {
                behaviour.gameObject.SetActive(shouldBeActive);
            }

            if (!data.id.Equals("wardrobe_monster", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var spriteState = behaviour.GetComponent<SpriteStateController>();
            if (spriteState == null)
            {
                spriteState = behaviour.GetComponentInChildren<SpriteStateController>(true);
            }

            if (spriteState == null)
            {
                continue;
            }

            if (shouldBeActive)
            {
                spriteState.PlayLoop();
            }
            else
            {
                spriteState.StopLoop();
            }
        }
    }
}
