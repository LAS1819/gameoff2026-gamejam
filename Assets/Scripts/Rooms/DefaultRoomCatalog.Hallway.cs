using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
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
            id = "memory_02_ammonite_stain",
            displayName = "Ammonite Stain",
            description = "A damp stain follows the shape of an ammonite.",
            descriptionInOtherReality = "The stain opens a childhood memory of fossils and summers spent beside my father.",
            memoryId = GameFlagNames.Memory_02,
            interactionType = InteractionType.Examine,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.Memory_02
        });

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
            consumeRequiredItemOnUse = true,
            resultFlag = GameFlagNames.LibraryDoorUnlocked,
            linkedRoomId = GameManager.LibraryRoomId,
            consumeAfterUse = false
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
