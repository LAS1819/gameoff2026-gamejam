using System;
using UnityEngine;

internal static class InteractionWardrobeCases
{
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

        var wardrobeController = ResolveWardrobeController(interactable.id, sourceObject);
        if (wardrobeController == null)
        {
            Debug.LogWarning($"[InteractionManager] WardrobeController not found for interactable '{interactable.id}'.");
            return false;
        }

        var reality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        var isOtherSide = reality == RealityType.OtherSide;
        var isLightOn = gameManager.HasFlag(GameFlagNames.BedroomLightOn);

        if (wardrobeController.IsOpen)
        {
            wardrobeController.SetOpen(false);
            gameManager.SetFlag(GameFlagNames.BedroomWardrobeOpened, false);
            ShowFeedback("You close the wardrobe.", 1.5f);
            return true;
        }

        wardrobeController.SetOpen(true);
        gameManager.SetFlag(GameFlagNames.BedroomWardrobeOpened, true);

        if (isOtherSide && !isLightOn)
        {
            ShowFeedback("You open the wardrobe. Something is moving inside.", 1.9f);
            return true;
        }

        if (isOtherSide && isLightOn)
        {
            ShowFeedback("You open the wardrobe. Something glints inside.", 1.8f);
            return true;
        }

        ShowFeedback("You open the wardrobe.", 1.5f);
        return true;
    }

    private static WardrobeController ResolveWardrobeController(string interactableId, GameObject sourceObject)
    {
        WardrobeController wardrobeController = null;
        if (sourceObject != null)
        {
            wardrobeController = sourceObject.GetComponent<WardrobeController>();
            if (wardrobeController == null)
            {
                wardrobeController = sourceObject.GetComponentInParent<WardrobeController>();
            }
        }

        if (wardrobeController != null)
        {
            return wardrobeController;
        }

        var wardrobeObject = FindInteractableObject(interactableId);
        if (wardrobeObject == null)
        {
            return null;
        }

        wardrobeController = wardrobeObject.GetComponent<WardrobeController>();
        if (wardrobeController == null)
        {
            wardrobeController = wardrobeObject.GetComponentInParent<WardrobeController>();
        }

        return wardrobeController;
    }

    private static void ShowFeedback(string message, float duration)
    {
        var ui = UIManager.Instance;
        if (ui != null)
        {
            ui.ShowFeedback(message, duration);
        }
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
                    if (behaviour == null || behaviour.GetData() == null)
                    {
                        continue;
                    }

                    if (behaviour.GetData().id.Equals(interactableId, StringComparison.OrdinalIgnoreCase))
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
