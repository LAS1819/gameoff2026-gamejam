using System.Collections.Generic;
using UnityEngine;

public static class GameFlagNames
{
    public const string HasStaffKey = "HasStaffKey";
    public const string DoorOpened = "DoorOpened";
    public const string RealityUnlocked = "RealityUnlocked";
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Dictionary<string, bool> Flags { get; } = new Dictionary<string, bool>();

    public string CurrentRoomId { get; private set; } = "vestibulo";

    public event System.Action<string> FlagChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeMVP();
    }

    public void InitializeMVP()
    {
        Flags.Clear();
        SetFlag(GameFlagNames.HasStaffKey, false);
        SetFlag(GameFlagNames.DoorOpened, false);
        SetFlag(GameFlagNames.RealityUnlocked, false);

        EnsureRuntimeSystems();

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SetRooms(BuildDefaultRooms());
            RoomManager.Instance.LoadRoom("vestibulo");
        }

        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.SetReality(RealityType.Normal);
        }
    }

    public void EnsureRuntimeSystems()
    {
        if (RoomManager.Instance == null)
        {
            var roomObject = new GameObject("RoomManager");
            roomObject.transform.SetParent(transform);
            roomObject.AddComponent<RoomManager>();
        }

        if (InventoryManager.Instance == null)
        {
            var inventoryObject = new GameObject("InventoryManager");
            inventoryObject.transform.SetParent(transform);
            inventoryObject.AddComponent<InventoryManager>();
        }

        if (InteractionManager.Instance == null)
        {
            var interactionObject = new GameObject("InteractionManager");
            interactionObject.transform.SetParent(transform);
            interactionObject.AddComponent<InteractionManager>();
        }

        if (RealityManager.Instance == null)
        {
            var realityObject = new GameObject("RealityManager");
            realityObject.transform.SetParent(transform);
            realityObject.AddComponent<RealityManager>();
        }
    }

    public static List<RoomData> BuildDefaultRooms()
    {
        var vestibule = new RoomData
        {
            id = "vestibulo",
            displayName = "Vestíbulo",
            connectedRoomIds = new List<string> { "pasillo" }
        };

        vestibule.interactables.Add(new InteractableData
        {
            id = "staff_key",
            itemId = "staff_key",
            displayName = "Llave del personal",
            description = "Una llave con el distintivo del personal, todavía útil.",
            interactionType = InteractionType.Pickup,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            isVisible = true,
            resultFlag = GameFlagNames.HasStaffKey
        });

        vestibule.interactables.Add(new InteractableData
        {
            id = "front_door",
            displayName = "Puerta del pasillo",
            description = "La puerta está cerrada. Necesita la llave del personal.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            isVisible = true,
            requiredItemId = "staff_key",
            resultFlag = GameFlagNames.DoorOpened,
            consumeAfterUse = true
        });

        var hallway = new RoomData
        {
            id = "pasillo",
            displayName = "Pasillo principal",
            connectedRoomIds = new List<string> { "vestibulo" }
        };

        hallway.interactables.Add(new InteractableData
        {
            id = "hallway_obstacle",
            displayName = "Obstáculo del pasillo",
            description = "Un tornillo y un cable bloquean el paso en la realidad normal.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            isVisible = true,
            resultFlag = GameFlagNames.RealityUnlocked
        });

        return new List<RoomData> { vestibule, hallway };
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
