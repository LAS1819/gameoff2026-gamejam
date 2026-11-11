using System;
using UnityEngine;

public class RealityManager : MonoBehaviour
{
    public static RealityManager Instance { get; private set; }

    public RealityType CurrentReality { get; private set; } = RealityType.Normal;

    public event Action<RealityType> RealityChanged;

    private GameObject backgroundNormal;
    private GameObject backgroundOtherSide;

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

    private void OnEnable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged += HandleRoomChanged;
        }
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged -= HandleRoomChanged;
        }
    }

    public void SetReality(RealityType reality)
    {
        if (CurrentReality == reality)
        {
            return;
        }

        CurrentReality = reality;

        // Update background visibility - ALWAYS search fresh since rooms activate/deactivate
        UpdateBackgrounds();

        RoomManager.Instance?.RefreshInteractables();
        RealityChanged?.Invoke(CurrentReality);

        Debug.Log($"[RealityManager] Reality changed to: {CurrentReality}");
    }

    public bool IsShiftAllowedInCurrentRoom()
    {
        var roomId = RoomManager.Instance?.CurrentRoom?.id;
        if (string.IsNullOrWhiteSpace(roomId))
        {
            return false;
        }

        return roomId.Equals(GameManager.HallwayRoomId, StringComparison.OrdinalIgnoreCase)
            || roomId.Equals(GameManager.BedroomRoomId, StringComparison.OrdinalIgnoreCase);
    }

    public bool TryToggleReality()
    {
        if (!IsShiftAllowedInCurrentRoom())
        {
            UIManager.Instance?.ShowFeedback("Shift unavailable here.", 1.25f);
            Debug.LogWarning($"[RealityManager] SHIFT blocked in room '{RoomManager.Instance?.CurrentRoom?.id ?? "<unknown>"}'");
            return false;
        }

        ToggleReality();
        GameManager.Instance?.SetFlag(GameFlagNames.ShiftPerformed, true);
        return true;
    }

    public void ToggleReality()
    {
        SetReality(CurrentReality == RealityType.Normal ? RealityType.OtherSide : RealityType.Normal);
    }

    /// <summary>
    /// Public static wrapper for UI buttons - can be called directly from Inspector
    /// </summary>
    public static void ToggleRealityStatic()
    {
        if (Instance != null)
        {
            Instance.ToggleReality();
            Debug.Log("[RealityManager] Reality toggled from UI");
        }
        else
        {
            Debug.LogError("[RealityManager] Instance not found for UI toggle");
        }
    }

    private void UpdateBackgrounds()
    {
        FindBackgroundsForCurrentRoom();

        if (backgroundNormal != null && backgroundOtherSide != null)
        {
            var normalActive = CurrentReality == RealityType.Normal;
            var otherActive = CurrentReality == RealityType.OtherSide;

            backgroundNormal.SetActive(normalActive);
            backgroundOtherSide.SetActive(otherActive);

            Debug.Log($"[RealityManager] Updated backgrounds - Normal: {normalActive}, OtherSide: {otherActive}");
        }
        else if (backgroundNormal != null)
        {
            Debug.LogWarning("[RealityManager] BackgroundNormal found, but BackgroundOtherSide NOT found - current room may not have OtherSide variant");
        }
        else
        {
            Debug.LogWarning("[RealityManager] No backgrounds found in current room");
        }
    }

    private void HandleRoomChanged(RoomData _)
    {
        UpdateBackgrounds();
    }

    private void FindBackgroundsForCurrentRoom()
    {
        backgroundNormal = null;
        backgroundOtherSide = null;

        var currentRoomId = RoomManager.Instance?.CurrentRoom?.id;
        if (string.IsNullOrEmpty(currentRoomId))
        {
            return;
        }

        var roomsRoot = GameObject.Find("Rooms");
        if (roomsRoot == null)
        {
            Debug.LogWarning("[RealityManager] 'Rooms' root not found.");
            return;
        }

        Transform roomRoot = null;
        foreach (Transform child in roomsRoot.transform)
        {
            if (child.name.Equals(currentRoomId, StringComparison.OrdinalIgnoreCase))
            {
                roomRoot = child;
                break;
            }
        }

        if (roomRoot == null)
        {
            Debug.LogWarning($"[RealityManager] Active room root not found for '{currentRoomId}'.");
            return;
        }

        var visuals = roomRoot.Find("Visuals");
        if (visuals == null)
        {
            return;
        }

        var normal = visuals.Find("BackgroundNormal");
        var other = visuals.Find("BackgroundOtherSide");

        if (normal != null)
        {
            backgroundNormal = normal.gameObject;
            Debug.Log($"[RealityManager] Found BackgroundNormal at {GetFullPath(normal)}");
        }

        if (other != null)
        {
            backgroundOtherSide = other.gameObject;
            Debug.Log($"[RealityManager] Found BackgroundOtherSide at {GetFullPath(other)}");
        }
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
}
