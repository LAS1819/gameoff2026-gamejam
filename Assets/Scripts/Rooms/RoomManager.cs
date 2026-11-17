using System;
using System.Collections.Generic;
using UnityEngine;

public partial class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    private readonly Dictionary<string, RoomData> rooms = new Dictionary<string, RoomData>();

    public RoomData CurrentRoom { get; private set; }

    public event Action<RoomData> RoomChanged;
    public event Action InteractablesRefreshed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetRooms(List<RoomData> roomList)
    {
        rooms.Clear();

        if (roomList == null)
        {
            return;
        }

        foreach (var room in roomList)
        {
            if (room == null || string.IsNullOrEmpty(room.id))
            {
                continue;
            }

            rooms[room.id] = room;
        }

        if (CurrentRoom == null && rooms.Count > 0)
        {
            var firstRoomId = default(string);
            foreach (var roomId in rooms.Keys)
            {
                firstRoomId = roomId;
                break;
            }

            if (!string.IsNullOrEmpty(firstRoomId))
            {
                CurrentRoom = rooms[firstRoomId];
            }
        }
    }

    public bool LoadRoom(string roomId)
    {
        if (string.IsNullOrEmpty(roomId) || !rooms.TryGetValue(roomId, out var room))
        {
            Debug.LogWarning($"[RoomManager] Room not found: {roomId}");
            return false;
        }

        CurrentRoom = room;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCurrentRoom(roomId);
        }

        HideAllRooms();
        ShowRoom(roomId);
        BindCurrentRoomInteractables();

        RefreshInteractables();
        RoomChanged?.Invoke(CurrentRoom);
        
        Debug.Log($"[RoomManager] Loaded room: {roomId}");
        return true;
    }

    private void BindCurrentRoomInteractables()
    {
        if (CurrentRoom == null)
        {
            return;
        }

        var roomRoot = FindRoomRootById(CurrentRoom.id);
        if (roomRoot == null)
        {
            return;
        }

        var interactableBehaviours = roomRoot.GetComponentsInChildren<InteractableBehaviour>(true);
        foreach (var behaviour in interactableBehaviours)
        {
            if (behaviour == null)
            {
                continue;
            }

            var currentData = behaviour.GetData();
            var catalogMatch = FindCatalogMatchForTransform(behaviour.transform, roomRoot.transform);

            if (catalogMatch == null)
            {
                continue;
            }

            if (currentData == null || string.IsNullOrEmpty(currentData.id) ||
                !currentData.id.Equals(catalogMatch.id, StringComparison.OrdinalIgnoreCase))
            {
                behaviour.Configure(catalogMatch);
            }
        }
    }

    private InteractableData FindCatalogMatchForTransform(Transform start, Transform roomRoot)
    {
        if (CurrentRoom == null || start == null)
        {
            return null;
        }

        var current = start;
        while (current != null)
        {
            var match = CurrentRoom.interactables.Find(item =>
                item != null && IsSameInteractable(current.name, item));

            if (match != null)
            {
                return match;
            }

            if (current == roomRoot)
            {
                break;
            }

            current = current.parent;
        }

        return null;
    }

    private bool IsSameInteractable(string objectName, InteractableData data)
    {
        if (string.IsNullOrEmpty(objectName) || data == null)
        {
            return false;
        }

        var normalizedObjectName = NormalizeIdentifier(objectName);
        var normalizedId = NormalizeIdentifier(data.id);
        var normalizedDisplayName = NormalizeIdentifier(data.displayName);

        return objectName.Equals(data.id, StringComparison.OrdinalIgnoreCase)
            || objectName.Equals(data.displayName, StringComparison.OrdinalIgnoreCase)
            || normalizedObjectName.Equals(normalizedId, StringComparison.OrdinalIgnoreCase)
            || normalizedObjectName.Equals(normalizedDisplayName, StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeIdentifier(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var builder = new System.Text.StringBuilder();
        foreach (var character in value)
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(char.ToLowerInvariant(character));
            }
        }

        return builder.ToString();
    }

    public bool TryGetRoom(string roomId, out RoomData room)
    {
        return rooms.TryGetValue(roomId, out room);
    }

    public void RefreshInteractables()
    {
        RefreshConditionalInteractableObjects();
        InteractablesRefreshed?.Invoke();
    }

}
