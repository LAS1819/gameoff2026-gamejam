using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
    private static RoomData CreateForest()
    {
        var forest = new RoomData
        {
            id = GameManager.ForestRoomId,
            displayName = "Forest",
            connectedRoomIds = new List<string> { GameManager.BasementRoomId }
        };

        forest.interactables.Add(new InteractableData
        {
            id = "forest_advance",
            displayName = "Forest Path",
            description = "The path continues between the trees.",
            descriptionInNormalReality = "A step closer to the lights.",
            descriptionInOtherReality = "The path continues, but the way back becomes less certain.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.ForestProgressAdvanced
        });

        forest.interactables.Add(new InteractableData
        {
            id = "basement_exit_door",
            displayName = "Basement Exit Door",
            description = "Return to the Basement.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = GameManager.BasementRoomId,
            resultFlag = GameFlagNames.BasementExitUnlocked
        });

        forest.interactables.Add(new InteractableData
        {
            id = "forest_revelation_letter",
            displayName = "Forest Revelation Letter",
            description = "A letter rests on the table, written in my father's handwriting.",
            descriptionInNormalReality = "A letter rests on the table, written in my father's handwriting.",
            descriptionInOtherReality = "The letter is waiting for me to stop denying what it says.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.ForestLetterRead
        });

        return forest;
    }
}
