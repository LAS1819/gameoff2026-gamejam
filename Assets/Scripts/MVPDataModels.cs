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
    Toggle
}

public enum RealityType
{
    Normal,
    OtherSide
}

[Serializable]
public class InteractableData
{
    public string id;                               // Unique identifier for this interactable
    public string displayName;
    public string description;
    public string descriptionInNormalReality;
    public string descriptionInOtherReality;
    public InteractionType interactionType;
    
    // Visibility rules for reality system
    public bool visibleInNormalReality = true;
    public bool visibleInOtherReality = true;
    
    // For Pickup interactions: what item does the player receive?
    public string itemId;
    
    // For Use interactions: what item is required to use this?
    public string requiredItemId;
    
    // What flag to set when this interaction succeeds?
    public string resultFlag;
    
    // For Transition interactions: which room to go to?
    public string linkedRoomId;
    
    // Should this interactable be consumed after first use? (e.g., key disappears after unlocking door)
    public bool consumeAfterUse;

    public string GetDescriptionForReality(RealityType reality)
    {
        if (reality == RealityType.Normal && !string.IsNullOrWhiteSpace(descriptionInNormalReality))
        {
            return descriptionInNormalReality;
        }

        if (reality == RealityType.OtherSide && !string.IsNullOrWhiteSpace(descriptionInOtherReality))
        {
            return descriptionInOtherReality;
        }

        return string.IsNullOrWhiteSpace(description) ? displayName : description;
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
