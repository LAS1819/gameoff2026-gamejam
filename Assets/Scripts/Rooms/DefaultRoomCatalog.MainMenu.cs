using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
    private static RoomData CreateMainMenu()
    {
        var mainMenu = new RoomData
        {
            id = GameManager.MainMenuRoomId,
            displayName = "Main Menu",
            connectedRoomIds = new List<string> { GameManager.LobbyRoomId }
        };

        mainMenu.interactables.Add(new InteractableData
        {
            id = "main_menu_start",
            displayName = "Start",
            description = "Begin the story.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = GameFlagNames.MainMenuIntroStarted
        });

        mainMenu.interactables.Add(new InteractableData
        {
            id = "main_menu_settings",
            displayName = "Settings",
            description = "Settings are not available in this prototype.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true
        });

        mainMenu.interactables.Add(new InteractableData
        {
            id = "main_menu_exit",
            displayName = "Exit",
            description = "Exit the game.",
            interactionType = InteractionType.Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true
        });

        mainMenu.interactables.Add(new InteractableData
        {
            id = "main_menu_entrance",
            displayName = "Entrance Door",
            description = "The entrance waits beyond the awakened sign.",
            interactionType = InteractionType.Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = GameManager.LobbyRoomId
        });

        return mainMenu;
    }
}
