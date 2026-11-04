using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameFlagNames
{
    public const string HasStaffKey = "HasStaffKey";
    public const string DoorOpened = "DoorOpened";
    public const string RealityUnlocked = "RealityUnlocked";
    public const string ShiftPerformed = "ShiftPerformed";
}

public class GameManager : MonoBehaviour
{
    public const string BootstrapSceneName = "Bootstrap";
    public const string MainSceneName = "Main";
    public const string LobbyRoomId = "lobby";
    public const string HallwayRoomId = "hallway";

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        var activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == BootstrapSceneName)
        {
            SceneManager.LoadScene(MainSceneName, LoadSceneMode.Single);
            return;
        }

        InitializeMVPIfNeeded();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == MainSceneName)
        {
            InitializeMVPIfNeeded();
        }
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

        EnsureRuntimeSystems();
        InventoryManager.Instance?.ResetInventory();
        InteractionManager.Instance?.ResetRuntimeState();

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SetRooms(BuildDefaultRooms());
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
        EnsureRuntimeSystem<UIManager>("UIManager");
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
        var lobby = new RoomData
        {
            id = LobbyRoomId,
            displayName = "Lobby",
            connectedRoomIds = new List<string> { HallwayRoomId }
        };

        lobby.interactables.Add(new InteractableData
        {
            id = "staff_key",
            itemId = "staff_key",
            displayName = "Staff Key",
            description = "A brass key marked with the staff insignia.",
            interactionType = InteractionType.Pickup,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            resultFlag = GameFlagNames.HasStaffKey,
            consumeAfterUse = true
        });

        lobby.interactables.Add(new InteractableData
        {
            id = "front_door",
            displayName = "Front Door",
            description = "A heavy door with an old brass keyhole.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            requiredItemId = "staff_key",
            resultFlag = GameFlagNames.DoorOpened
        });

        lobby.interactables.Add(new InteractableData
        {
            id = "hallway_entrance",
            displayName = "Main Hallway Entrance",
            description = "You can continue into the darkness.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = HallwayRoomId
        });

        var hallway = new RoomData
        {
            id = HallwayRoomId,
            displayName = "Main Hallway",
            connectedRoomIds = new List<string> { LobbyRoomId }
        };

        hallway.interactables.Add(new InteractableData
        {
            id = "hallway_obstacle",
            displayName = "Hallway Obstacle",
            description = "A massive figure blocks the path in this reality.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            resultFlag = GameFlagNames.RealityUnlocked
        });

        hallway.interactables.Add(new InteractableData
        {
            id = "shift_trigger",
            displayName = "Shifting Point",
            description = "Reality feels thin at this spot.",
            interactionType = InteractionType.Toggle,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.ShiftPerformed
        });

        hallway.interactables.Add(new InteractableData
        {
            id = "lobby_exit",
            displayName = "Lobby Exit",
            description = "Return to the lobby.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = LobbyRoomId
        });

        return new List<RoomData> { lobby, hallway };
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
