using System.Collections.Generic;
using UnityEngine;

public static partial class DefaultRoomCatalog
{
    private static RoomLightingProfile CreateLightingProfile(
        float normalIntensity,
        Color normalColor,
        float otherSideIntensity,
        Color otherSideColor,
        bool lanternAllowed = false,
        float lanternNormalBonus = 0.32f,
        float lanternOtherSideBonus = 0.26f)
    {
        return new RoomLightingProfile
        {
            normalIntensity = normalIntensity,
            normalColor = normalColor,
            otherSideIntensity = otherSideIntensity,
            otherSideColor = otherSideColor,
            lanternAllowed = lanternAllowed,
            lanternNormalBonus = lanternNormalBonus,
            lanternOtherSideBonus = lanternOtherSideBonus
        };
    }

    public static List<RoomData> BuildMvpRooms()
    {
        var mainMenu = CreateMainMenu();
        var lobby = CreateLobby();
        var hallway = CreateHallway();
        var bedroom = CreateBedroom();
        var library = CreateLibrary();
        var basement = CreateBasement();
        var forest = CreateForest();

        return new List<RoomData>
        {
            mainMenu,
            lobby,
            hallway,
            bedroom,
            library,
            basement,
            forest
        };
    }
}
