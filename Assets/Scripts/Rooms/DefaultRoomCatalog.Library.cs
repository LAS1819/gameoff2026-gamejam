using System.Collections.Generic;
using UnityEngine;

public static partial class DefaultRoomCatalog
{
    private static RoomData CreateLibrary()
    {
        var library = new RoomData
        {
            id = GameManager.LibraryRoomId,
            displayName = "Library",
            connectedRoomIds = new List<string> { GameManager.HallwayRoomId },
            lightingProfile = CreateLightingProfile(
                0.84f,
                new Color(1f, 0.94f, 0.78f),
                0.56f,
                new Color(0.68f, 0.72f, 0.88f))
        };

        AddLibraryInteractables(library);
        return library;
    }

    private static void AddLibraryInteractables(RoomData library)
    {
        library.interactables.Add(new InteractableData
        {
            id = "memory_04_wall_clock",
            displayName = "Broken Wall Clock",
            description = "The clock has stopped at 11:30.",
            descriptionInOtherReality = "The stopped hands preserve the last ordinary conversation I shared with my father.",
            memoryId = GameFlagNames.Memory_04,
            interactionType = InteractionType.Examine,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.Memory_04
        });

        library.interactables.Add(new InteractableData
        {
            id = "worn_book",
            itemId = "worn_book",
            displayName = "Worn Book",
            description = "A worn book half-hidden among the shelves. It feels older than the room itself.",
            descriptionInNormalReality = "The book looks ordinary at first glance, but it feels carefully preserved.",
            descriptionInOtherReality = "The spine is wrong. It seems to remember a version of the room that does not exist here.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            resultFlag = GameFlagNames.WornBookFound,
            consumeAfterUse = true
        });

        library.interactables.Add(new InteractableData
        {
            id = "shelf_interactive",
            displayName = "Interactive Shelf",
            description = "A shelf with a narrow gap between the books, as if something once belonged there.",
            descriptionInNormalReality = "The titles evoke memories of my own life, as if they were not written for me but for someone I used to be.",
            descriptionInOtherReality = "The titles show moments of my life that I had forgotten, arranged as if the room itself is trying to remember me.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.LibraryShelfActivated
        });

        library.interactables.Add(new InteractableData
        {
            id = "shelf_secret_door",
            displayName = "Shelf Secret Door",
            description = "The wall behind the shelf is no longer where it should be.",
            descriptionInNormalReality = "Nothing is here; only a shelf and the stillness of the room.",
            descriptionInOtherReality = "Behind the shelf, a hidden panel shifts as if it is trying to remember its own shape.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.LibrarySecretRevealed
        });

        library.interactables.Add(new InteractableData
        {
            id = "shelf_right",
            displayName = "Right Shelf",
            description = "A shelf lined with records and old memories, each one placed carefully by hand.",
            descriptionInNormalReality = "The books are neat, dated, and almost too organized.",
            descriptionInOtherReality = "The labels do not match the memories they describe. Something in the room has been refiled.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.LibraryMemoriesObserved
        });

        library.interactables.Add(new InteractableData
        {
            id = "basement_door",
            displayName = "Basement Door",
            description = "A heavy door hidden behind the shelf, leading somewhere underneath the building.",
            descriptionInNormalReality = "The wall is flat and empty. Nothing should be here.",
            descriptionInOtherReality = "The door is there now, as if the room has finally decided to remember it.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BasementUnlocked,
            consumeAfterUse = false,
            linkedRoomId = GameManager.BasementRoomId
        });

        library.interactables.Add(new InteractableData
        {
            id = "hallway_exit",
            displayName = "Hallway Exit",
            description = "Return to the hallway.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = GameManager.HallwayRoomId
        });

        library.interactables.Add(new InteractableData
        {
            id = "code_panel",
            displayName = "Code Panel",
            description = "A small panel with a numeric keypad, likely controlling access to the basement.",
            descriptionInOtherReality = "The keypad is already unlocked. The real barrier is the door itself.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            requiredItemId = "basement_code",
            consumeRequiredItemOnUse = true,
            resultFlag = GameFlagNames.BasementCodeUsed
        });
    }
}
