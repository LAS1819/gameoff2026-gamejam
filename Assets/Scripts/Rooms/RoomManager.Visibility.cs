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

            if (interactable.id.Equals("basement_door", StringComparison.OrdinalIgnoreCase)
                && !GameManager.Instance.HasFlag(GameFlagNames.BasementUnlocked)
                && !GameManager.Instance.HasFlag(GameFlagNames.BasementCodeUsed))
            {
                return false;
            }

            if (interactable.id.Equals("code_panel", StringComparison.OrdinalIgnoreCase)
                && (GameManager.Instance.HasFlag(GameFlagNames.BasementUnlocked)
                    || GameManager.Instance.HasFlag(GameFlagNames.BasementCodeUsed)))
            {
                return false;
            }
        }

        if (interactable.id.Equals("denial_manifestation", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null)
        {
            if (GameManager.Instance.HasFlag(GameFlagNames.SteamEnabled)
                || GameManager.Instance.HasFlag(GameFlagNames.DenialManifestationRejected))
            {
                return false;
            }
        }

        if (interactable.id.Equals("forest_exit_door", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null)
        {
            var resolved = GameManager.Instance.HasFlag(GameFlagNames.PressureValveRepaired)
                && GameManager.Instance.HasFlag(GameFlagNames.SteamEnabled)
                && GameManager.Instance.HasFlag(GameFlagNames.DenialManifestationRejected);

            if (!resolved)
            {
                return false;
            }
        }

        if (interactable.id.Equals("forest_advance", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null
            && GameManager.Instance.ForestDepthLevel >= 6)
        {
            return false;
        }

        if (interactable.id.Equals("forest_revelation_letter", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null)
        {
            if (GameManager.Instance.ForestDepthLevel < 6
                || !GameManager.Instance.HasFlag(GameFlagNames.ForestFinalTableVisible))
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
        if (CurrentRoom == null)
        {
            return;
        }

        if (CurrentRoom.id.Equals(GameManager.BasementRoomId, StringComparison.OrdinalIgnoreCase))
        {
            RefreshBasementConditionalObjects();
        }

        if (!CurrentRoom.id.Equals(GameManager.BedroomRoomId, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var roomRoot = FindRoomRootById(CurrentRoom.id);
        if (roomRoot == null)
        {
            return;
        }

        RefreshBedroomPersistentState(roomRoot);

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

    private static void RefreshBedroomPersistentState(GameObject roomRoot)
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null || roomRoot == null)
        {
            return;
        }

        var wardrobe = roomRoot.GetComponentInChildren<WardrobeController>(true);
        if (wardrobe != null)
        {
            wardrobe.SetOpen(gameManager.HasFlag(GameFlagNames.BedroomWardrobeOpened));
        }

        var computer = roomRoot.GetComponentInChildren<BedroomComputerController>(true);
        if (computer != null)
        {
            computer.SetPowered(gameManager.HasFlag(GameFlagNames.BedroomComputerPowered));
            computer.SetDiskInserted(gameManager.HasFlag(GameFlagNames.BedroomComputerDiskInserted));
        }

        var ceilingLamp = roomRoot.transform.Find("CeilingLamp");
        if (ceilingLamp == null)
        {
            var transforms = roomRoot.GetComponentsInChildren<Transform>(true);
            foreach (var transform in transforms)
            {
                if (transform != null
                    && transform.name.Equals("CeilingLamp", StringComparison.OrdinalIgnoreCase))
                {
                    ceilingLamp = transform;
                    break;
                }
            }
        }

        var lightState = ceilingLamp != null
            ? ceilingLamp.GetComponent<SpriteStateController>()
            : null;
        if (lightState != null)
        {
            lightState.SetState(gameManager.HasFlag(GameFlagNames.BedroomLightOn) ? 1 : 0);
        }
    }

    private void RefreshBasementConditionalObjects()
    {
        var roomRoot = FindRoomRootById(GameManager.BasementRoomId);
        if (roomRoot == null)
        {
            return;
        }

        var valveHandleVisual = roomRoot.transform.Find("ValveHandle");
        if (valveHandleVisual == null)
        {
            valveHandleVisual = FindNamedTransformInRoot(roomRoot.transform, "ValveHandle");
        }

        if (valveHandleVisual != null)
        {
            var inNormalReality = RealityManager.Instance == null
                || RealityManager.Instance.CurrentReality == RealityType.Normal;
            var repaired = GameManager.Instance != null
                && GameManager.Instance.HasFlag(GameFlagNames.PressureValveRepaired);
            valveHandleVisual.gameObject.SetActive(repaired && inNormalReality);
        }

        var steamPipes = roomRoot.transform.Find("SteamPipes");
        if (steamPipes == null)
        {
            steamPipes = FindNamedTransformInRoot(roomRoot.transform, "SteamPipes");
        }

        if (steamPipes != null)
        {
            var steamEnabled = GameManager.Instance != null
                && GameManager.Instance.HasFlag(GameFlagNames.SteamEnabled);
            var inOtherReality = RealityManager.Instance != null
                && RealityManager.Instance.CurrentReality == RealityType.OtherSide;
            steamPipes.gameObject.SetActive(steamEnabled && inOtherReality);

            var spriteState = steamPipes.GetComponent<SpriteStateController>();
            if (spriteState != null)
            {
                if (steamPipes.gameObject.activeSelf)
                {
                    spriteState.PlayLoop();
                }
                else
                {
                    spriteState.StopLoop();
                }
            }
        }

        var denial = FindNamedTransformInRoot(roomRoot.transform, "DenialManifestation");
        if (denial != null)
        {
            var shouldBeVisible = IsNamedInteractableVisible("denial_manifestation");
            var rejectionController = denial.GetComponent<DenialManifestationController>();
            if (rejectionController == null)
            {
                rejectionController = denial.GetComponentInChildren<DenialManifestationController>(true);
            }

            if (rejectionController == null)
            {
                rejectionController = denial.gameObject.AddComponent<DenialManifestationController>();
            }

            if (!shouldBeVisible
                && GameManager.Instance != null
                && GameManager.Instance.HasFlag(GameFlagNames.DenialManifestationRejected)
                && !rejectionController.HasPlayed)
            {
                denial.gameObject.SetActive(true);
                rejectionController.PlayRejection();
            }
            else if (!rejectionController.IsPlaying)
            {
                denial.gameObject.SetActive(shouldBeVisible);
            }

            var spriteState = denial.GetComponent<SpriteStateController>();
            if (spriteState != null)
            {
                if (shouldBeVisible)
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

    private Transform FindNamedTransformInRoot(Transform roomRoot, string name)
    {
        if (roomRoot == null || string.IsNullOrEmpty(name))
        {
            return null;
        }

        var transforms = roomRoot.GetComponentsInChildren<Transform>(true);
        foreach (var transform in transforms)
        {
            if (transform != null && transform.name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return transform;
            }
        }

        return null;
    }

    private bool IsNamedInteractableVisible(string interactableId)
    {
        if (CurrentRoom == null)
        {
            return false;
        }

        var interactable = CurrentRoom.interactables.Find(item =>
            item != null && item.id.Equals(interactableId, StringComparison.OrdinalIgnoreCase));
        return interactable != null && IsInteractableVisible(interactable);
    }
}
