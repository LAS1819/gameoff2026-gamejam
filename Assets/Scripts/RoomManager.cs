using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
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
        DontDestroyOnLoad(gameObject);
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
            // Don't auto-load here; let the caller decide which room to load first
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

        RefreshInteractables();
        RoomChanged?.Invoke(CurrentRoom);
        
        Debug.Log($"[RoomManager] Loaded room: {roomId}");
        return true;
    }

    private void HideAllRooms()
    {
        var roomsParent = FindRoomsParent();
        if (roomsParent == null)
        {
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
            return;
        }

        foreach (Transform child in roomsParent.transform)
        {
            if (child.gameObject.name.Equals(roomId, System.StringComparison.OrdinalIgnoreCase))
            {
                child.gameObject.SetActive(true);
                Debug.Log($"[RoomManager] Loaded room: {roomId}");
                return;
            }
        }

        Debug.LogWarning($"[RoomManager] Room '{roomId}' not found. Available: {string.Join(", ", GetAvailableRoomNames(roomsParent))}");
    }

    private GameObject FindRoomsParent()
    {
        // Search for "Rooms" GameObject in scene
        var allTransforms = FindObjectsOfType<Transform>();
        foreach (var t in allTransforms)
        {
            if (t.gameObject.name == "Rooms")
            {
                return t.gameObject;
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

    public bool TryGetRoom(string roomId, out RoomData room)
    {
        return rooms.TryGetValue(roomId, out room);
    }

    public void RefreshInteractables()
    {
        InteractablesRefreshed?.Invoke();
    }

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

        return CurrentRoom.interactables.Exists(item => item != null && item.id == interactableId);
    }
}
