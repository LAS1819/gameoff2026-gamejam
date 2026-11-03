using System;
using System.Collections.Generic;

[Serializable]
public class InventoryItemData
{
    public string id;
    public string displayName;
    public string description;
    public bool isKeyItem;

    public InventoryItemData()
    {
    }

    public InventoryItemData(string id, string displayName, string description, bool isKeyItem = false)
    {
        this.id = id;
        this.displayName = displayName;
        this.description = description;
        this.isKeyItem = isKeyItem;
    }
}

public enum InteractionType
{
    Examine,
    Pickup,
    Use,
    Transition,
    Toggle,
    EnterRoom
}

public enum RealityType
{
    Normal,
    OtherSide
}

[Serializable]
public class InteractableData
{
    public string id;
    public string displayName;
    public string description;
    public InteractionType interactionType;
    public bool visibleInNormalReality = true;
    public bool visibleInOtherReality = true;
    public bool isVisible;
    public string itemId;
    public string requiredItemId;
    public string resultFlag;
    public bool isPickedUp;
    public bool isConsumed;
    public bool consumeAfterUse;
    public string linkedRoomId;

    public InteractionType type
    {
        get => interactionType;
        set => interactionType = value;
    }

    public string itemName
    {
        get => displayName;
        set => displayName = value;
    }

    public string itemDescription
    {
        get => description;
        set => description = value;
    }
}

[Serializable]
public class RoomData
{
    public string id;
    public string displayName;
    public List<InteractableData> interactables = new List<InteractableData>();
    public List<string> connectedRoomIds = new List<string>();
}
