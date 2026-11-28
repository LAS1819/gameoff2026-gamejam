using System;
using UnityEngine;

public class RealityManager : MonoBehaviour
{
    public static RealityManager Instance { get; private set; }

    public RealityType CurrentReality { get; private set; } = RealityType.Normal;

    public event Action<RealityType> RealityChanged;

    private GameObject backgroundNormal;
    private GameObject backgroundOtherSide;
    private bool roomEventsSubscribed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        TrySubscribeRoomEvents();
    }

    private void Start()
    {
        TrySubscribeRoomEvents();
    }

    private void Update()
    {
        if (!roomEventsSubscribed)
        {
            TrySubscribeRoomEvents();
        }
    }

    private void OnDisable()
    {
        if (roomEventsSubscribed && RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged -= HandleRoomChanged;
        }

        roomEventsSubscribed = false;
    }

    public void SetReality(RealityType reality)
    {
        if (CurrentReality == reality)
        {
            return;
        }

        CurrentReality = reality;
        ReconcileBasementRealityTransition();

        // Update background visibility - ALWAYS search fresh since rooms activate/deactivate
        UpdateBackgrounds();

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RefreshInteractables();
        }

        if (RealityChanged != null)
        {
            RealityChanged.Invoke(CurrentReality);
        }

    }

    private void ReconcileBasementRealityTransition()
    {
        RoomData room = null;
        if (RoomManager.Instance != null)
        {
            room = RoomManager.Instance.CurrentRoom;
        }
        var gameManager = GameManager.Instance;
        if (room == null || gameManager == null)
        {
            return;
        }

        if (!room.id.Equals(GameManager.BasementRoomId, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (CurrentReality != RealityType.OtherSide)
        {
            return;
        }

        if (!gameManager.HasFlag(GameFlagNames.PressureValveRepaired)
            || !gameManager.HasFlag(GameFlagNames.SteamEnabled))
        {
            return;
        }

        gameManager.SetFlag(GameFlagNames.DenialManifestationRejected, true);
        gameManager.SetFlag(GameFlagNames.DenialManifestationVisible, false);
        gameManager.SetFlag(GameFlagNames.ForestExitUnlocked, true);
        gameManager.SetFlag(GameFlagNames.BasementResolved, true);
        PlayDenialManifestationRejection();
    }

    private void PlayDenialManifestationRejection()
    {
        RoomData room = null;
        if (RoomManager.Instance != null)
        {
            room = RoomManager.Instance.CurrentRoom;
        }
        if (room == null)
        {
            return;
        }

        var roomRoot = GameObject.Find("Main/Rooms") ?? GameObject.Find("Rooms");
        if (roomRoot == null)
        {
            return;
        }

        Transform basementRoot = null;
        foreach (Transform child in roomRoot.transform)
        {
            if (child.name.Equals(GameManager.BasementRoomId, System.StringComparison.OrdinalIgnoreCase))
            {
                basementRoot = child;
                break;
            }
        }

        if (basementRoot == null)
        {
            return;
        }

        var manifestationTransform = basementRoot.Find("DenialManifestation");
        if (manifestationTransform == null)
        {
            manifestationTransform = FindNamedTransform(basementRoot, "DenialManifestation");
        }

        if (manifestationTransform == null)
        {
            return;
        }

        manifestationTransform.gameObject.SetActive(true);

        var controller = manifestationTransform.GetComponent<DenialManifestationController>();
        if (controller == null)
        {
            controller = manifestationTransform.gameObject.AddComponent<DenialManifestationController>();
        }

        var renderer = manifestationTransform.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }

        controller.ResetForReplay();
        controller.PlayRejection();
    }

    private static Transform FindNamedTransform(Transform root, string objectName)
    {
        if (root == null || string.IsNullOrEmpty(objectName))
        {
            return null;
        }

        var transforms = root.GetComponentsInChildren<Transform>(true);
        foreach (var transform in transforms)
        {
            if (transform != null
                && transform.name.Equals(objectName, StringComparison.OrdinalIgnoreCase))
            {
                return transform;
            }
        }

        return null;
    }

    public bool IsShiftAllowedInCurrentRoom()
    {
        var roomId = string.Empty;
        if (RoomManager.Instance != null && RoomManager.Instance.CurrentRoom != null)
        {
            roomId = RoomManager.Instance.CurrentRoom.id;
        }
        if (string.IsNullOrWhiteSpace(roomId))
        {
            return false;
        }

        return roomId.Equals(GameManager.HallwayRoomId, StringComparison.OrdinalIgnoreCase)
            || roomId.Equals(GameManager.BedroomRoomId, StringComparison.OrdinalIgnoreCase)
            || roomId.Equals(GameManager.LibraryRoomId, StringComparison.OrdinalIgnoreCase)
            || roomId.Equals(GameManager.BasementRoomId, StringComparison.OrdinalIgnoreCase);
    }

    public bool TryToggleReality()
    {
        if (!IsShiftAllowedInCurrentRoom())
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowFeedback("Shift unavailable here.", 1.25f);
            }

            var roomId = "<unknown>";
            if (RoomManager.Instance != null && RoomManager.Instance.CurrentRoom != null)
            {
                roomId = RoomManager.Instance.CurrentRoom.id;
            }

            Debug.LogWarning($"[RealityManager] SHIFT blocked in room '{roomId}'");
            return false;
        }

        ToggleReality();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.ShiftPerformed, true);
        }
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
            return;
        }

        if (backgroundNormal != null)
        {
            backgroundNormal.SetActive(CurrentReality == RealityType.Normal);
            return;
        }

        if (backgroundOtherSide != null)
        {
            backgroundOtherSide.SetActive(CurrentReality == RealityType.OtherSide);
        }
    }

    private void HandleRoomChanged(RoomData _)
    {
        UpdateBackgrounds();
    }

    private void TrySubscribeRoomEvents()
    {
        if (roomEventsSubscribed || RoomManager.Instance == null)
        {
            return;
        }

        RoomManager.Instance.RoomChanged += HandleRoomChanged;
        roomEventsSubscribed = true;
        UpdateBackgrounds();
    }

    private void FindBackgroundsForCurrentRoom()
    {
        backgroundNormal = null;
        backgroundOtherSide = null;

        var currentRoomId = string.Empty;
        if (RoomManager.Instance != null && RoomManager.Instance.CurrentRoom != null)
        {
            currentRoomId = RoomManager.Instance.CurrentRoom.id;
        }
        if (string.IsNullOrEmpty(currentRoomId))
        {
            return;
        }

        var roomsRoot = GameObject.Find("Main/Rooms") ?? GameObject.Find("Rooms");
        if (roomsRoot == null)
        {
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
        }

        if (other != null)
        {
            backgroundOtherSide = other.gameObject;
        }
    }

}
