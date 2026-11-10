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
            var catalogMatch = CurrentRoom.interactables.Find(item =>
                item != null && IsSameInteractable(behaviour.gameObject.name, item));

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
            Debug.Log($"[RoomManager] Deactivated: {child.gameObject.name}");
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
            if (child.gameObject.name.Equals(roomId, StringComparison.OrdinalIgnoreCase))
            {
                child.gameObject.SetActive(true);
                Debug.Log($"[RoomManager] Activated room: {roomId}");
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
            if (child.gameObject.name.Equals(roomId, StringComparison.OrdinalIgnoreCase))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private GameObject FindRoomsParent()
    {
        // Try direct path first
        var direct = GameObject.Find("Main/Rooms");
        if (direct != null) return direct;

        // Try just looking for any "Rooms" GameObject
        var allRoomsObjects = FindObjectsOfType<Transform>();
        foreach (var t in allRoomsObjects)
        {
            if (t.gameObject.name == "Rooms")
            {
                Debug.Log("[RoomManager] Found Rooms at path: " + GetFullPath(t));
                return t.gameObject;
            }
        }

        return null;
    }

    private string GetFullPath(Transform t)
    {
        var path = t.name;
        var current = t.parent;
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        return path;
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

        if (interactable.id == "flashlight_tool" && GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.HasFlashlight))
        {
            return false;
        }

        if (interactable.id == "library_key" && GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.HasLibraryKey))
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
            item != null &&
            NormalizeIdentifier(item.id).Equals(targetId, StringComparison.OrdinalIgnoreCase));
    }
}
