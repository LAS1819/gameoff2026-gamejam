using System.Collections.Generic;

public static partial class DefaultRoomCatalog
{
    public static List<RoomData> BuildMvpRooms()
    {
        var lobby = CreateLobby();
        var hallway = CreateHallway();
        var bedroom = CreateBedroom();
        var library = CreateLibrary();
        var basement = CreateBasement();
        var forest = CreateForest();

        return new List<RoomData>
        {
            lobby,
            hallway,
            bedroom,
            library,
            basement,
            forest
        };
    }
}
