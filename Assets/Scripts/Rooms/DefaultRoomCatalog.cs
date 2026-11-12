using System.Collections.Generic;

public static class DefaultRoomCatalog
{
    public static List<RoomData> BuildMvpRooms()
    {
        var lobby = CreateLobby();
        var hallway = CreateHallway();
        var bedroom = CreateBedroom();

        return new List<RoomData>
        {
            lobby,
            hallway,
            bedroom
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
            displayName = "Mysterious Door (Library)",
            description = "This door leads to the Library. [NOT YET IMPLEMENTED]",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = false,
            visibleInOtherReality = true
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

    private static RoomData CreateBedroom()
    {
        var bedroom = new RoomData
        {
            id = GameManager.BedroomRoomId,
            displayName = "Bedroom",
            connectedRoomIds = new List<string> { GameManager.HallwayRoomId }
        };

        AddBedroomInteractables(bedroom);
        return bedroom;
    }

    private static void AddBedroomInteractables(RoomData bedroom)
    {
        bedroom.interactables.Add(new InteractableData
        {
            id = "nightstand",
            displayName = "Nightstand",
            description = "A small nightstand with old objects on top.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "ceiling_light",
            displayName = "Ceiling Light",
            description = "A worn ceiling light hanging over the room.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BedroomLightOn
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "switch",
            displayName = "Light Switch",
            description = "The switch that controls the room light.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BedroomLightOn
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "wardrobe",
            displayName = "Wardrobe",
            description = "An old wardrobe. Something is hiding inside.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BedroomWardrobeOpened
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "wardrobe_monster",
            displayName = "Monster",
            description = "A looming shape blocks the wardrobe from the darkness.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = false,
            visibleInOtherReality = true
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "window",
            displayName = "Window",
            description = "The window looks out into a sealed, impossible darkness.",
            descriptionInNormalReality = "The window looks out into a sealed, impossible darkness. The outside is still and empty, but it feels wrong to look at it too long.",
            descriptionInOtherReality = "The glass does not reflect the room anymore. Beyond it, the darkness is not empty: it is waiting.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "flashlight_tool",
            displayName = "Flashlight",
            description = "A practical flashlight. It feels like it should always be ready.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            resultFlag = GameFlagNames.HasFlashlight,
            consumeAfterUse = false
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "library_key",
            itemId = "library_key",
            displayName = "Library Key",
            description = "A small key hidden in the wardrobe. It opens the library door.",
            interactionType = InteractionType.Pickup,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.HasLibraryKey,
            consumeAfterUse = true
        });

        bedroom.interactables.Add(new InteractableData
        {
            id = "hallway_exit",
            displayName = "Hallway Exit",
            description = "Leave the bedroom and return to the hallway.",
            descriptionInNormalReality = "The bedroom door leads back to the hallway. Beyond it, the corridor is quiet and familiar.",
            descriptionInOtherReality = "The same doorway feels wrong. The hallway beyond is not the same place it was a moment ago.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = GameManager.HallwayRoomId
        });
    }
}
