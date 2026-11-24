using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
    private static RoomData CreateBasement()
    {
        var basement = new RoomData
        {
            id = GameManager.BasementRoomId,
            displayName = "Basement",
            connectedRoomIds = new List<string> { GameManager.LibraryRoomId, GameManager.LobbyRoomId }
        };

        AddBasementInteractables(basement);
        return basement;
    }

    private static void AddBasementInteractables(RoomData basement)
    {
        basement.interactables.Add(new InteractableData
        {
            id = "memory_05_toolbox",
            displayName = "Toolbox",
            description = "A toolbox shows years of worn paint and repeated use.",
            descriptionInOtherReality = "The tools return me to my father's orchard and the quiet pride he found in helping others.",
            memoryId = GameFlagNames.Memory_05,
            interactionType = InteractionType.Examine,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.Memory_05
        });

        basement.interactables.Add(new InteractableData
        {
            id = "storage_shelf_left",
            displayName = "Storage Shelves",
            description = "The left shelves are packed with old boxes and rusted tools.",
            descriptionInNormalReality = "Boxes labeled years ago collect moisture and dust.",
            descriptionInOtherReality = "The same shelves feel re-sorted by memories that do not belong to this place.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BasementShelfLeftExplored
        });

        basement.interactables.Add(new InteractableData
        {
            id = "storage_shelf_right",
            displayName = "Storage Shelves",
            description = "The right shelves hold broken parts and forgotten belongings.",
            descriptionInNormalReality = "Everything is stacked in practical silence.",
            descriptionInOtherReality = "The arrangement feels deliberate, as if trying to hide one specific memory.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BasementShelfRightExplored
        });

        basement.interactables.Add(new InteractableData
        {
            id = "personal_effects_box",
            displayName = "Personal Effects Box",
            description = "A cardboard box marked as personal effects.",
            descriptionInNormalReality = "The label says PERSONAL. Photos and papers are tucked inside.",
            descriptionInOtherReality = "The same label now reads MIND. The contents no longer agree with each other.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BasementEvidenceFound
        });

        basement.interactables.Add(new InteractableData
        {
            id = "pipelines",
            displayName = "Pipelines",
            description = "The pipe network rattles with contained pressure.",
            descriptionInNormalReality = "The pipes are old but intact enough to carry pressure.",
            descriptionInOtherReality = "Cracks run across the metal like open veins.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.BasementPipelinesExplored
        });

        basement.interactables.Add(new InteractableData
        {
            id = "pressure_release_valve",
            displayName = "Pressure Release Valve",
            description = "A release valve missing its handle.",
            descriptionInNormalReality = "The valve can be repaired if I place the missing handle.",
            descriptionInOtherReality = "In this reality the mechanism is warped and unstable.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            requiredItemId = "valve_handle",
            consumeRequiredItemOnUse = true,
            resultFlag = GameFlagNames.PressureValveRepaired
        });

        basement.interactables.Add(new InteractableData
        {
            id = "ambulance_lights",
            displayName = "Ambulance Lights",
            description = "Red and blue reflections pulse in short, broken intervals.",
            descriptionInOtherReality = "The flashes drag a memory I cannot hold steady.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.AmbulanceMemoryTriggered
        });

        basement.interactables.Add(new InteractableData
        {
            id = "denial_manifestation",
            displayName = "Denial Manifestation",
            description = "A distorted shape trembles in front of the exit.",
            descriptionInOtherReality = "It does not attack. It only refuses to let me pass.",
            interactionType = InteractionType.Examine,
            visibleInNormalReality = false,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.DenialManifestationVisible
        });

        basement.interactables.Add(new InteractableData
        {
            id = "forest_exit_door",
            displayName = "Forest Exit Door",
            description = "A hidden path becomes accessible once the denial manifestation is rejected.",
            descriptionInNormalReality = "The path is clear now. A trail to the forest waits beyond.",
            descriptionInOtherReality = "The steam burns here. I cannot pass through it.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            linkedRoomId = GameManager.ForestRoomId,
            resultFlag = GameFlagNames.ForestExitUnlocked
        });

        basement.interactables.Add(new InteractableData
        {
            id = "library_exit_door",
            displayName = "Library Exit Door",
            description = "Return to the Library.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = GameManager.LibraryRoomId,
            resultFlag = GameFlagNames.BasementExitUnlocked
        });
    }
}
