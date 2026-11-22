using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
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
            id = "memory_01_family_picture",
            displayName = "Family Picture",
            description = "An old picture remains in the frame.",
            descriptionInNormalReality = "In the picture, my father and I stand together beneath the first steps of a memory.",
            descriptionInOtherReality = "The picture feels like the beginning of a memory I have kept safe.",
            memoryId = GameFlagNames.Memory_01,
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.Memory_01
        });

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
            consumeRequiredItemOnUse = true,
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
}
