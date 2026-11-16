using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
    public static List<RoomData> BuildMvpRooms()
    {
        var lobby = CreateLobby();
        var hallway = CreateHallway();
        var bedroom = CreateBedroom();
        var library = CreateLibrary();

        return new List<RoomData>
        {
            lobby,
            hallway,
            bedroom,
            library
        };
    }

    private static RoomData CreateLobby()
    {
        var lobby = new RoomData
        {
            id = GameManager.LobbyRoomId,
            displayName = "Lobby",
            connectedRoomIds = new List<string> { GameManager.HallwayRoomId }
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
            resultFlag = GameFlagNames.DoorOpened,
            consumeAfterUse = true
        });

        lobby.interactables.Add(new InteractableData
        {
            id = "hallway_entrance",
            displayName = "Main Hallway Entrance",
            description = "You can continue into the darkness.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = GameManager.HallwayRoomId
        });

        return lobby;
    }

    private static RoomData CreateHallway()
    {
        var hallway = new RoomData
        {
            id = GameManager.HallwayRoomId,
            displayName = "Main Hallway",
            connectedRoomIds = new List<string> { GameManager.LobbyRoomId }
        };

        AddHallwayInteractables(hallway);
        return hallway;
    }

    private static void AddHallwayInteractables(RoomData hallway)
    {
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
            linkedRoomId = GameManager.LobbyRoomId
        });

        hallway.interactables.Add(new InteractableData
        {
            id = "library_door",
            displayName = "Library Door",
            description = "A locked door to the library, half-hidden behind the shape of the corridor.",
            descriptionInOtherReality = "The Library is behind this door. It is locked, and the key is still somewhere in the house.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            requiredItemId = "library_key",
            linkedRoomId = GameManager.LibraryRoomId,
            consumeAfterUse = true
        });

        hallway.interactables.Add(new InteractableData
        {
            id = "bedroom_door",
            displayName = "Mysterious Door (Bedroom)",
            description = "This door leads to the Bedroom.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            linkedRoomId = GameManager.BedroomRoomId
        });
    }

}
