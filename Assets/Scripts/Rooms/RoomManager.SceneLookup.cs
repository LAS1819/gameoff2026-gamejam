using System;
using UnityEngine;

public partial class RoomManager
{
    private void HideAllRooms()
    {
        var roomsParent = FindRoomsParent();
        if (roomsParent == null)
        {
            Debug.LogError("[RoomManager] Could not find 'Rooms' parent anywhere in scene!");
            return;
        }

        foreach (Transform child in roomsParent.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void ShowRoom(string roomId)
    {
        var roomsParent = FindRoomsParent();
        if (roomsParent == null)
        {
            Debug.LogError("[RoomManager] Could not find 'Rooms' parent!");
            return;
        }

        foreach (Transform child in roomsParent.transform)
        {
            if (NormalizeIdentifier(child.gameObject.name)
                .Equals(NormalizeIdentifier(roomId), StringComparison.OrdinalIgnoreCase))
            {
                child.gameObject.SetActive(true);
                if (roomId.Equals(GameManager.ForestRoomId, StringComparison.OrdinalIgnoreCase))
                {
                    var depthController = child.GetComponentInChildren<ForestDepthController>(true);
                    if (depthController != null)
                    {
                        depthController.RefreshDepth();
                    }
                }
                return;
            }
        }

        Debug.LogError($"[RoomManager] Room GameObject not found for: {roomId}. Available rooms: {string.Join(", ", GetAvailableRoomNames(roomsParent))}");
    }

    private GameObject FindRoomRootById(string roomId)
    {
        var roomsParent = FindRoomsParent();
        if (roomsParent == null || string.IsNullOrEmpty(roomId))
        {
            return null;
        }

        foreach (Transform child in roomsParent.transform)
        {
            if (NormalizeIdentifier(child.gameObject.name)
                .Equals(NormalizeIdentifier(roomId), StringComparison.OrdinalIgnoreCase))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private GameObject FindRoomsParent()
    {
        var direct = GameObject.Find("Main/Rooms");
        if (direct != null)
        {
            return direct;
        }

        var allRoomsObjects = UnityEngine.Object.FindObjectsByType<Transform>();
        foreach (var transform in allRoomsObjects)
        {
            if (transform.gameObject.name == "Rooms")
            {
                return transform.gameObject;
            }
        }

        return null;
    }

    private string[] GetAvailableRoomNames(GameObject roomsParent)
    {
        var names = new System.Collections.Generic.List<string>();
        foreach (Transform child in roomsParent.transform)
        {
            names.Add(child.gameObject.name);
        }

        return names.ToArray();
    }
}
