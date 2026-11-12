using System.Collections.Generic;
using UnityEngine;

public static class GameFlagNames
{
    public const string HasStaffKey = "HasStaffKey";
    public const string DoorOpened = "DoorOpened";
    public const string RealityUnlocked = "RealityUnlocked";
    public const string ShiftPerformed = "ShiftPerformed";
    public const string HasFlashlight = "HasFlashlight";
    public const string BedroomLightOn = "BedroomLightOn";
    public const string BedroomWardrobeOpened = "BedroomWardrobeOpened";
    public const string BedroomMonsterVisible = "BedroomMonsterVisible";
    public const string HasLibraryKey = "HasLibraryKey";
}

public class GameManager : MonoBehaviour
{
    public const string MainSceneName = "Main";
    public const string LobbyRoomId = "lobby";
    public const string HallwayRoomId = "hallway";
    public const string BedroomRoomId = "bedroom";

    public static GameManager Instance { get; private set; }

    public Dictionary<string, bool> Flags { get; } = new Dictionary<string, bool>();

    public string CurrentRoomId { get; private set; } = LobbyRoomId;

    public event System.Action<string> FlagChanged;

    private bool isInitialized;

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

    private void Start()
    {
        InitializeMVPIfNeeded();
    }

    private void InitializeMVPIfNeeded()
    {
        if (isInitialized)
        {
            return;
        }

        InitializeMVP();
        isInitialized = true;
    }

    public void InitializeMVP()
    {
        Flags.Clear();
        SetFlag(GameFlagNames.HasStaffKey, false);
        SetFlag(GameFlagNames.DoorOpened, false);
        SetFlag(GameFlagNames.RealityUnlocked, false);
        SetFlag(GameFlagNames.ShiftPerformed, false);
        SetFlag(GameFlagNames.HasFlashlight, false);
        SetFlag(GameFlagNames.BedroomLightOn, false);
        SetFlag(GameFlagNames.BedroomWardrobeOpened, false);
        SetFlag(GameFlagNames.BedroomMonsterVisible, false);
        SetFlag(GameFlagNames.HasLibraryKey, false);

        EnsureRuntimeSystems();
        InventoryManager.Instance?.ResetInventory();
        InteractionManager.Instance?.ResetRuntimeState();

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SetRooms(DefaultRoomCatalog.BuildMvpRooms());
            RoomManager.Instance.LoadRoom(LobbyRoomId);
        }

        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.SetReality(RealityType.Normal);
        }
    }

    public void EnsureRuntimeSystems()
    {
        EnsureRuntimeSystem<RoomManager>("RoomManager");
        EnsureRuntimeSystem<InventoryManager>("InventoryManager");
        EnsureRuntimeSystem<InteractionManager>("InteractionManager");
        EnsureRuntimeSystem<RealityManager>("RealityManager");
        EnsureRuntimeSystem<EventManager>("EventManager");
        EnsureRuntimeSystem<SaveManager>("SaveManager");
        EnsureRuntimeSystem<AudioManager>("AudioManager");
        EnsureRuntimeSystem<UIManager>("UIManager");
        EnsureRuntimeSystem<InputHandler>("InputHandler");
    }

    private void EnsureRuntimeSystem<T>(string objectName) where T : MonoBehaviour
    {
        if (FindAnyObjectByType<T>() != null)
        {
            return;
        }

        var systemObject = new GameObject(objectName);
        systemObject.transform.SetParent(transform);
        systemObject.AddComponent<T>();
    }

    public static List<RoomData> BuildDefaultRooms()
    {
        return DefaultRoomCatalog.BuildMvpRooms();
    }

    public bool HasFlag(string flagName)
    {
        return Flags.TryGetValue(flagName, out var value) && value;
    }

    public bool SetFlag(string flagName, bool value)
    {
        if (!Flags.TryGetValue(flagName, out var existingValue) || existingValue != value)
        {
            Flags[flagName] = value;
            FlagChanged?.Invoke(flagName);
            return true;
        }

        return false;
    }

    public void SetCurrentRoom(string roomId)
    {
        CurrentRoomId = roomId;
    }
}
