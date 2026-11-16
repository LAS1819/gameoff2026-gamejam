using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
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
            description = "A small nightstand with old objects on top. One of them is a flashlight.",
            itemId = "flashlight_tool",
            interactionType = InteractionType.Pickup,
            resultFlag = GameFlagNames.HasFlashlight,
            consumeAfterUse = false,
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
