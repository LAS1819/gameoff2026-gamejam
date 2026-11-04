using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    private readonly Dictionary<string, RoomData> rooms = new Dictionary<string, RoomData>();

    public RoomData CurrentRoom { get; private set; }

    public event Action<RoomData> RoomChanged;

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
                LoadRoom(firstRoomId);
            }
        }
    }

    public bool LoadRoom(string roomId)
    {
        if (string.IsNullOrEmpty(roomId) || !rooms.TryGetValue(roomId, out var room))
        {
            Debug.LogWarning($"Room not found: {roomId}");
            return false;
        }

        CurrentRoom = room;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCurrentRoom(roomId);
        }

        RefreshInteractables();
        RoomChanged?.Invoke(CurrentRoom);
        return true;
    }

    public bool TryGetRoom(string roomId, out RoomData room)
    {
        return rooms.TryGetValue(roomId, out room);
    }

    public void RefreshInteractables()
    {
        if (CurrentRoom == null)
        {
            return;
        }

        foreach (var interactable in CurrentRoom.interactables)
        {
            if (interactable == null)
            {
                continue;
            }

            interactable.isVisible = IsInteractableVisible(interactable);
        }
    }

    public bool IsInteractableVisible(InteractableData interactable)
    {
        if (interactable == null)
            return false;

        // Check reality visibility rules
        if (RealityManager.Instance != null)
        {
            bool isNormalReality = RealityManager.Instance.CurrentReality == RealityType.Normal;
            bool shouldBeVisible = isNormalReality 
                ? interactable.visibleInNormalReality 
                : interactable.visibleInOtherReality;
            
            if (!shouldBeVisible)
                return false;
        }

        // Check if interactable is consumed (temporary - for MVP we don't track this yet)
        // Later: track in a persistent state system

        return true;
    }
}
