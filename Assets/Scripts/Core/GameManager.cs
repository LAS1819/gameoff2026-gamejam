using System.Collections.Generic;
using UnityEngine;

public static class GameFlagNames
{
    public const string HasStaffKey = "HasStaffKey";
    public const string DoorOpened = "DoorOpened";
    public const string RealityUnlocked = "RealityUnlocked";
    public const string ShiftPerformed = "ShiftPerformed";
    public const string HasFlashlight = "HasFlashlight";
    public const string FlashlightActive = "FlashlightActive";
    public const string BedroomLightOn = "BedroomLightOn";
    public const string BedroomWardrobeOpened = "BedroomWardrobeOpened";
    public const string BedroomMonsterVisible = "BedroomMonsterVisible";
    public const string BedroomComputerPowered = "BedroomComputerPowered";
    public const string BedroomComputerDiskInserted = "BedroomComputerDiskInserted";
    public const string HasLibraryKey = "HasLibraryKey";
    public const string LibraryDoorUnlocked = "LibraryDoorUnlocked";
    public const string WornBookFound = "WornBookFound";
    public const string FloppyDiskRecovered = "FloppyDiskRecovered";
    public const string LibraryMemoriesObserved = "LibraryMemoriesObserved";
    public const string LibraryShelfActivated = "LibraryShelfActivated";
    public const string LibrarySecretRevealed = "LibrarySecretRevealed";
    public const string LibrarySecretDoorOpen = "LibrarySecretDoorOpen";
    public const string BasementDoorVisible = "BasementDoorVisible";
    public const string BasementUnlocked = "BasementUnlocked";
    public const string BasementCodeAcquired = "BasementCodeAcquired";
    public const string BasementEntered = "BasementEntered";
    public const string BasementShelfLeftExplored = "BasementShelfLeftExplored";
    public const string BasementShelfRightExplored = "BasementShelfRightExplored";
    public const string BasementPipelinesExplored = "BasementPipelinesExplored";
    public const string BasementEvidenceFound = "BasementEvidenceFound";
    public const string ValveHandleFound = "ValveHandleFound";
    public const string PressureValveRepaired = "PressureValveRepaired";
    public const string SteamEnabled = "SteamEnabled";
    public const string DenialManifestationVisible = "DenialManifestationVisible";
    public const string BasementExitUnlocked = "BasementExitUnlocked";
    public const string BasementResolved = "BasementResolved";
    public const string ForestExitUnlocked = "ForestExitUnlocked";
    public const string AmbulanceMemoryTriggered = "AmbulanceMemoryTriggered";
    public const string BasementCodeUsed = "BasementCodeUsed";
    public const string DenialManifestationRejected = "DenialManifestationRejected";
    public const string ForestEntered = "ForestEntered";
    public const string ForestProgressAdvanced = "ForestProgressAdvanced";
    public const string ForestFinalTableVisible = "ForestFinalTableVisible";
    public const string ForestLetterRead = "ForestLetterRead";
    public const string ForestEndingReached = "ForestEndingReached";
    public const string MainMenuIntroStarted = "MainMenuIntroStarted";
    public const string MainMenuEntryUnlocked = "MainMenuEntryUnlocked";
    public const string Memory_01 = "Memory_01";
    public const string Memory_02 = "Memory_02";
    public const string Memory_03 = "Memory_03";
    public const string Memory_04 = "Memory_04";
    public const string Memory_05 = "Memory_05";
}

public class GameManager : MonoBehaviour
{
    public const string MainSceneName = "Main";
    public const string MainMenuRoomId = "main_menu";
    public const string LobbyRoomId = "lobby";
    public const string HallwayRoomId = "hallway";
    public const string BedroomRoomId = "bedroom";
    public const string LibraryRoomId = "library";
    public const string BasementRoomId = "basement";
    public const string ForestRoomId = "forest";

    [Header("Development")]
    [SerializeField] private bool useDebugStartRoom = true;
    [SerializeField] private string debugStartRoomId = LibraryRoomId;

    public static GameManager Instance { get; private set; }

    public Dictionary<string, bool> Flags { get; } = new Dictionary<string, bool>();

    public string CurrentRoomId { get; private set; } = LobbyRoomId;
    public int ForestDepthLevel { get; private set; }

    public event System.Action<string> FlagChanged;
    public event System.Action<int> ForestDepthChanged;

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
        SetFlag(GameFlagNames.FlashlightActive, false);
        SetFlag(GameFlagNames.BedroomLightOn, false);
        SetFlag(GameFlagNames.BedroomWardrobeOpened, false);
        SetFlag(GameFlagNames.BedroomMonsterVisible, false);
        SetFlag(GameFlagNames.BedroomComputerPowered, false);
        SetFlag(GameFlagNames.BedroomComputerDiskInserted, false);
        SetFlag(GameFlagNames.HasLibraryKey, false);
        SetFlag(GameFlagNames.LibraryDoorUnlocked, false);
        SetFlag(GameFlagNames.WornBookFound, false);
        SetFlag(GameFlagNames.FloppyDiskRecovered, false);
        SetFlag(GameFlagNames.LibraryMemoriesObserved, false);
        SetFlag(GameFlagNames.LibraryShelfActivated, false);
        SetFlag(GameFlagNames.LibrarySecretRevealed, false);
        SetFlag(GameFlagNames.LibrarySecretDoorOpen, false);
        SetFlag(GameFlagNames.BasementDoorVisible, false);
        SetFlag(GameFlagNames.BasementUnlocked, false);
        SetFlag(GameFlagNames.BasementCodeAcquired, false);
        SetFlag(GameFlagNames.BasementEntered, false);
        SetFlag(GameFlagNames.BasementShelfLeftExplored, false);
        SetFlag(GameFlagNames.BasementShelfRightExplored, false);
        SetFlag(GameFlagNames.BasementPipelinesExplored, false);
        SetFlag(GameFlagNames.BasementEvidenceFound, false);
        SetFlag(GameFlagNames.ValveHandleFound, false);
        SetFlag(GameFlagNames.PressureValveRepaired, false);
        SetFlag(GameFlagNames.SteamEnabled, false);
        SetFlag(GameFlagNames.DenialManifestationVisible, false);
        SetFlag(GameFlagNames.BasementExitUnlocked, false);
        SetFlag(GameFlagNames.BasementResolved, false);
        SetFlag(GameFlagNames.ForestExitUnlocked, false);
        SetFlag(GameFlagNames.AmbulanceMemoryTriggered, false);
        SetFlag(GameFlagNames.BasementCodeUsed, false);
        SetFlag(GameFlagNames.DenialManifestationRejected, false);
        SetFlag(GameFlagNames.ForestEntered, false);
        SetFlag(GameFlagNames.ForestProgressAdvanced, false);
        SetFlag(GameFlagNames.ForestFinalTableVisible, false);
        SetFlag(GameFlagNames.ForestLetterRead, false);
        SetFlag(GameFlagNames.ForestEndingReached, false);
        SetFlag(GameFlagNames.MainMenuIntroStarted, false);
        SetFlag(GameFlagNames.MainMenuEntryUnlocked, false);
        SetFlag(GameFlagNames.Memory_01, false);
        SetFlag(GameFlagNames.Memory_02, false);
        SetFlag(GameFlagNames.Memory_03, false);
        SetFlag(GameFlagNames.Memory_04, false);
        SetFlag(GameFlagNames.Memory_05, false);
        ForestDepthLevel = 0;

        EnsureRuntimeSystems();
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ResetInventory();
        }

        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.ResetRuntimeState();
        }

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SetRooms(DefaultRoomCatalog.BuildMvpRooms());

            var targetRoomId = MainMenuRoomId;
            if (Application.isEditor && useDebugStartRoom && !string.IsNullOrWhiteSpace(debugStartRoomId))
            {
                targetRoomId = debugStartRoomId;
            }

            RoomManager.Instance.LoadRoom(targetRoomId);
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
        var systems = FindObjectsByType<T>(FindObjectsInactive.Include);

        T primarySystem = null;
        for (var index = 0; index < systems.Length; index++)
        {
            var system = systems[index];
            if (system == null)
            {
                continue;
            }

            if (primarySystem == null)
            {
                primarySystem = system;
                continue;
            }

            Destroy(system.gameObject);
        }

        if (primarySystem == null)
        {
            var systemObject = new GameObject(objectName);
            systemObject.transform.SetParent(transform);
            primarySystem = systemObject.AddComponent<T>();
        }
        else
        {
            if (primarySystem.transform.parent != transform)
            {
                primarySystem.transform.SetParent(transform);
            }

            if (!primarySystem.gameObject.name.Equals(objectName, System.StringComparison.Ordinal))
            {
                primarySystem.gameObject.name = objectName;
            }
        }
    }

    public static List<RoomData> BuildDefaultRooms()
    {
        return DefaultRoomCatalog.BuildMvpRooms();
    }

    public bool HasFlag(string flagName)
    {
        return Flags.TryGetValue(flagName, out var value) && value;
    }

    public int CountRecoveredMemories()
    {
        var count = 0;
        if (HasFlag(GameFlagNames.Memory_01)) count++;
        if (HasFlag(GameFlagNames.Memory_02)) count++;
        if (HasFlag(GameFlagNames.Memory_03)) count++;
        if (HasFlag(GameFlagNames.Memory_04)) count++;
        if (HasFlag(GameFlagNames.Memory_05)) count++;
        return count;
    }

    public bool SetFlag(string flagName, bool value)
    {
        if (!Flags.TryGetValue(flagName, out var existingValue) || existingValue != value)
        {
            Flags[flagName] = value;
            if (FlagChanged != null)
            {
                FlagChanged.Invoke(flagName);
            }
            return true;
        }

        return false;
    }

    public void SetCurrentRoom(string roomId)
    {
        CurrentRoomId = roomId;
        if (roomId.Equals(ForestRoomId, System.StringComparison.OrdinalIgnoreCase))
        {
            SetFlag(GameFlagNames.ForestEntered, true);
        }
    }

    public bool AdvanceForestDepth()
    {
        if (ForestDepthLevel >= 6)
        {
            return false;
        }

        ForestDepthLevel++;
        SetFlag(GameFlagNames.ForestProgressAdvanced, true);
        if (ForestDepthLevel == 6)
        {
            SetFlag(GameFlagNames.ForestFinalTableVisible, true);
        }
        if (ForestDepthChanged != null)
        {
            ForestDepthChanged.Invoke(ForestDepthLevel);
        }
        return true;
    }

    public void ResetForestDepth()
    {
        ForestDepthLevel = 0;
        SetFlag(GameFlagNames.ForestProgressAdvanced, false);
        SetFlag(GameFlagNames.ForestFinalTableVisible, false);
        SetFlag(GameFlagNames.ForestLetterRead, false);
        SetFlag(GameFlagNames.ForestEndingReached, false);
        if (ForestDepthChanged != null)
        {
            ForestDepthChanged.Invoke(ForestDepthLevel);
        }
    }
}
